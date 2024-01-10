using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Spriggan.Core.Transport;

public class RabbitMqConsumer : IHostedService
{
    private readonly ILogger<RabbitMqConsumer> _logger;

    private readonly IRabbitMqClient _client;

    public RabbitMqConsumer(ILogger<RabbitMqConsumer> logger, IRabbitMqClient client)
    {
        _logger = logger;
        _client = client;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _client.Consumer.Received += OnReceived;

        return Task.CompletedTask;
    }

    private void OnReceived(object? model, BasicDeliverEventArgs deliver)
    {
        var body = deliver.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        _logger.LogInformation($"Received message: {message}");

        var reply = _client.Channel.CreateBasicProperties();

        reply.CorrelationId = deliver.BasicProperties.CorrelationId;

        var bytes = Encoding.UTF8.GetBytes(message);

        _client.Channel.BasicPublish(
            exchange: "",
            routingKey: deliver.BasicProperties.ReplyTo,
            basicProperties: reply,
            body: bytes);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _client.Dispose();

        return Task.CompletedTask;
    }
}
