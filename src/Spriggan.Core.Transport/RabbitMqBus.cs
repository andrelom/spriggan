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

    private readonly ConcurrentDictionary<string, TaskCompletionSource<dynamic>> _pending = new();

    public RabbitMqBus(IRabbitMqClient client)
    {
        _client = client;

        var consumer = new EventingBasicConsumer(_client.Channel);

        foreach (var name in _client.ResponseQueues)
        {
            _client.Channel.BasicConsume(
                consumer: consumer,
                queue: name,
                autoAck: true
            );
        }

        consumer.Received += (_, deliver) => HandleResponse(deliver);
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancel = default) where TResponse : class
    {
        var id = Guid.NewGuid().ToString();
        var type = request.GetType();

        var properties = _client.Channel.CreateBasicProperties();

        properties.CorrelationId = id;
        properties.ReplyTo = type.ToQueueName("response");

        var body = Encoding.UTF8.GetBytes(request.ToJson());
        var source = new TaskCompletionSource<TResponse>();

        _pending[id] = new TaskCompletionSource<dynamic>(source);

        _client.Channel.BasicPublish(
            exchange: string.Empty,
            routingKey: type.ToQueueName("request"),
            mandatory: true,
            basicProperties: properties,
            body: body
        );

        return source.Task;
    }

    #region Private Methods

    private void HandleResponse(BasicDeliverEventArgs deliver)
    {
        var id = deliver.BasicProperties.CorrelationId;
        var json = Encoding.UTF8.GetString(deliver.Body.ToArray());
        var response = JsonSerializer.Deserialize<dynamic>(json);

        if (_pending.TryRemove(id, out var source))
        {
            source.SetResult(response);
        }
    }

    #endregion
}
