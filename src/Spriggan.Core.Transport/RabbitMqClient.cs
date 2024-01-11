using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Spriggan.Core.Extensions;
using Spriggan.Core.Transport.Options;

namespace Spriggan.Core.Transport;

public class RabbitMqClient : IRabbitMqClient
{
    private readonly IConnection _connection;

    private readonly IModel _channel;

    private readonly EventingBasicConsumer _consumer;

    public RabbitMqClient(IConfiguration configuration)
    {
        var options = configuration.Load<RabbitMqOptions>();

        var factory = new ConnectionFactory
        {
            HostName = options.Host,
            UserName = options.User,
            Password = options.Password,
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        var consumer = new EventingBasicConsumer(channel);

        _connection = connection;
        _channel = channel;
        _consumer = consumer;
    }

    public IModel Channel => _channel;

    public EventingBasicConsumer Consumer => _consumer;

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
