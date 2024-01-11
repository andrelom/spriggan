using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Spriggan.Core.Extensions;
using Spriggan.Core.Transport.Extensions;

namespace Spriggan.Core.Transport;

public class RabbitMqBus : IRabbitMqBus
{
    private readonly IRabbitMqClient _client;

    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _pending = new();

    public RabbitMqBus(IRabbitMqClient client)
    {
        _client = client;

        _client.Consumer.Received += (_, deliver) => HandleResponse(deliver);
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancel = default) where TResponse : class
    {
        var type = request.GetType();

        var id = Guid.NewGuid().ToString();
        var queue = _client.Channel.QueueDeclare(type.ToQueueName("response"));
        var properties = _client.Channel.CreateBasicProperties();

        properties.CorrelationId = id;
        properties.ReplyTo = queue.QueueName;

        var body = Encoding.UTF8.GetBytes(request.ToJson());
        var source = new TaskCompletionSource<string>();

        _pending[id] = source;

        _client.Channel.BasicPublish(
            exchange: string.Empty,
            routingKey: type.ToQueueName("request"),
            basicProperties: properties,
            body: body
        );

        var json = await source.Task;
        var response = JsonSerializer.Deserialize<TResponse>(json);

        if (response == default)
        {
            throw new Exception();
        }

        return response;
    }

    #region Private Methods

    private void HandleResponse(BasicDeliverEventArgs deliver)
    {
        var id = deliver.BasicProperties.CorrelationId;
        var response = Encoding.UTF8.GetString(deliver.Body.ToArray());

        if (_pending.TryRemove(id, out var source))
        {
            source.SetResult(response);
        }
    }

    #endregion
}
