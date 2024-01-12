using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Spriggan.Core.Extensions;

namespace Spriggan.Core.Transport;

public class RabbitMqConsumer : IHostedService
{
    private readonly IMediator _mediator;

    private readonly IRabbitMqClient _client;

    public RabbitMqConsumer(IMediator mediator, IRabbitMqClient client)
    {
        _mediator = mediator;
        _client = client;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_client.Channel);

        foreach (var name in _client.RequestQueues)
        {
            _client.Channel.BasicConsume(
                consumer: consumer,
                queue: name,
                autoAck: true
            );
        }

        consumer.Received += async (_, deliver) => await HandleRequest(deliver);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _client.Dispose();

        return Task.CompletedTask;
    }

    #region Private Methods

    private async Task HandleRequest(BasicDeliverEventArgs deliver)
    {
        var id = Guid.NewGuid().ToString();
        var name = deliver.RoutingKey.Split('#').Last();

        var properties = _client.Channel.CreateBasicProperties();

        properties.CorrelationId = id;
        properties.ReplyTo = $"response#{name}";

        var type = Type.GetType(name);
        var json = Encoding.UTF8.GetString(deliver.Body.ToArray());

        dynamic request = JsonSerializer.Deserialize(json, type, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var response = await _mediator.Send(request);
        var body = Encoding.UTF8.GetBytes(response.ToJson());

        _client.Channel.BasicPublish(
            exchange: string.Empty,
            routingKey: $"request#{name}",
            mandatory: true,
            basicProperties: properties,
            body: body
        );
    }

    #endregion
}
