using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Spriggan.Core.Extensions;
using Spriggan.Core.Transport.Extensions;

namespace Spriggan.Core.Transport;

public class Bus : IBus
{
    private readonly IConnection _connection;

    private readonly IModel _channel;

    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _pending = new();

    public Bus()
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (_, deliver) => HandleResponse(deliver);
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancel = default) where TResponse : class
    {
        var type = request.GetType();

        var id = Guid.NewGuid().ToString();
        var queue = _channel.QueueDeclare(type.ToQueueName("response"));
        var properties = _channel.CreateBasicProperties();

        properties.CorrelationId = id;
        properties.ReplyTo = queue.QueueName;

        var message = Encoding.UTF8.GetBytes(request.ToJson());
        var completion = new TaskCompletionSource<string>();

        _pending[id] = completion;

        _channel.BasicPublish(
            exchange: string.Empty,
            routingKey: type.ToQueueName("request"),
            basicProperties: properties,
            body: message
        );

        var json = await completion.Task;
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
