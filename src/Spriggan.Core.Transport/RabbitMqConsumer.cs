using System.Text;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Spriggan.Core.Transport;

public class RabbitMqConsumer : IHostedService
{
    private readonly IRabbitMqClient _client;

    private readonly IModel _channel;

    private readonly EventingBasicConsumer _consumer;

    public RabbitMqConsumer(IRabbitMqClient client)
    {
        _client = client;
        _channel = client.Connection.CreateModel();
        _consumer = new EventingBasicConsumer(_channel);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
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

    private void HandleRequest(BasicDeliverEventArgs deliver)
    {
        var id = deliver.BasicProperties.CorrelationId;
        var message = Encoding.UTF8.GetString(deliver.Body.ToArray());
    }

    #endregion
}
