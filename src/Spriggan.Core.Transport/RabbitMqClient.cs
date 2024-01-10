using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Spriggan.Core.Extensions;
using Spriggan.Core.Transport.Options;

namespace Spriggan.Core.Transport;

public class RabbitMqClient : IRabbitMqClient
{
    private readonly ILogger<RabbitMqConsumer> _logger;

    private readonly RabbitMqOptions _options;

    private readonly IConnection _connection;

    private readonly IModel _channel;

    private readonly EventingBasicConsumer _consumer;

    private readonly IEnumerable<string> _names = GetQueueNames();

    public RabbitMqClient(ILogger<RabbitMqConsumer> logger, IConfiguration configuration)
    {
        _logger = logger;
        _options = configuration.Load<RabbitMqOptions>();

        var connection = CreateConnection();
        var channel = connection.CreateModel();
        var consumer = new EventingBasicConsumer(channel);

        _connection = connection;
        _channel = channel;
        _consumer = consumer;

        DeclareQueues("request");
        DeclareQueues("response");
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }

    public IModel Channel => _channel;

    public EventingBasicConsumer Consumer => _consumer;

    #region Private Methods

    private static string ToQueueName(Type type)
    {
        var name = type.FullName;

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception();
        }

        return Regex.Replace(name, "[^a-zA-Z0-9]", string.Empty);
    }

    private static IEnumerable<string> GetQueueNames()
    {
        return GetRequestHandlerTypes().Select(ToQueueName);
    }

    private static IEnumerable<Type> GetRequestHandlerTypes()
    {
        var target = typeof(IRequestHandler<,>);

        return Dependencies.Domain.Where(type =>
        {
            return type.GetInterfaces().Any(source => source.IsGenericType && source.GetGenericTypeDefinition() == target);
        });
    }

    private IConnection CreateConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            UserName = _options.User,
            Password = _options.Password,
        };

        return factory.CreateConnection();
    }

    private void DeclareQueues(string prefix)
    {
        foreach (var name in _names)
        {
            _channel.QueueDeclare(
                queue: $"{prefix}#{name}",
                durable: false,
                exclusive: false,
                autoDelete: false);
        }
    }

    #endregion
}
