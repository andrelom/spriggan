using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;

namespace Spriggan.Core.Transport;

public class RabbitMqConsumer : IHostedService
{
    private readonly IMediator _mediator;

    private readonly IRabbitMqClient _client;

    private readonly EventingBasicConsumer _consumer;

    public RabbitMqConsumer(IMediator mediator, IRabbitMqClient client)
    {
        _mediator = mediator;
        _client = client;
        _consumer = new EventingBasicConsumer(client.Channel);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.Received += (_, deliver) => HandleRequest(deliver);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _client.Dispose();

        return Task.CompletedTask;
    }

    #region Private Methods

    private void HandleRequest(BasicDeliverEventArgs deliver)
    {
        var json = Encoding.UTF8.GetString(deliver.Body.ToArray());
        var request = JsonSerializer.Deserialize<object>(json);

        var abc = 0;
    }

    #endregion
}
