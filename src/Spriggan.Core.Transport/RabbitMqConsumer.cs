using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Spriggan.Core.Transport.Extensions;

namespace Spriggan.Core.Transport;

public class RabbitMqConsumer : IHostedService
{
    private readonly IMediator _mediator;

    private readonly IRabbitMqClient _client;

    private readonly IModel _channel;

    private readonly EventingBasicConsumer _consumer;

    public RabbitMqConsumer(IMediator mediator, IRabbitMqClient client)
    {
        _mediator = mediator;
        _client = client;
        _channel = client.Connection.CreateModel();
        _consumer = new EventingBasicConsumer(_channel);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        DeclareQueues("request");
        DeclareQueues("response");

        _consumer.Received += (_, deliver) => HandleRequest(deliver);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channel.Close();
        _client.Connection.Close();

        return Task.CompletedTask;
    }

    #region Private Methods

    private static IEnumerable<Type> GetRequestTypes()
    {
        var target = typeof(IRequest<>);

        return Dependencies.Domain.Where(type =>
        {
            return type.GetInterfaces().Any(source => source.IsGenericType && source.GetGenericTypeDefinition() == target);
        });
    }

    private void DeclareQueues(string prefix)
    {
        var names = GetRequestTypes().Select(type => type.ToQueueName(prefix));

        foreach (var name in names)
        {
            _channel.QueueDeclare(
                queue: name,
                durable: true,
                exclusive: false,
                autoDelete: false);
        }
    }

    private void HandleRequest(BasicDeliverEventArgs deliver)
    {
        var json = Encoding.UTF8.GetString(deliver.Body.ToArray());
        var request = JsonSerializer.Deserialize<object>(json);

        var abc = 0;
    }

    #endregion
}
