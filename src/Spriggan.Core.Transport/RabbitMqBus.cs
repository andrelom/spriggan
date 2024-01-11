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
    private readonly IModel _channel;

    private readonly EventingBasicConsumer _consumer;

    private readonly ConcurrentDictionary<string, TaskCompletionSource<dynamic>> _pending = new();

    public RabbitMqBus(IRabbitMqClient client)
    {
        _channel = client.Connection.CreateModel();
        _consumer = new EventingBasicConsumer(_channel);

        _consumer.Received += (_, deliver) => HandleResponse(deliver);
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancel = default) where TResponse : class
    {
        var id = Guid.NewGuid().ToString();
        var type = request.GetType();

        var queue = _channel.QueueDeclare(
            queue: type.ToQueueName("request"),
            durable: true,
            exclusive: false,
            autoDelete: false);

        var properties = _channel.CreateBasicProperties();

        properties.CorrelationId = id;
        properties.ReplyTo = queue.QueueName;

        var body = Encoding.UTF8.GetBytes(request.ToJson());
        var source = new TaskCompletionSource<TResponse>();

        _pending[id] = new TaskCompletionSource<dynamic>(source);

        _channel.BasicPublish(
            exchange: string.Empty,
            routingKey: type.ToQueueName("response"),
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
