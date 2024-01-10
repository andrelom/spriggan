using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Spriggan.Core.Extensions;
using Spriggan.Core.Transport.Options;

namespace Spriggan.Core.Transport;

public class Consumer : IHostedService
{
    private readonly ILogger<Consumer> _logger;

    private readonly RabbitMqOptions _options;

    private readonly IConnection _connection;

    private readonly IModel _channel;

    private readonly IEnumerable<string> _names = GetQueueNames();

    public Consumer(ILogger<Consumer> logger, IConfiguration configuration)
    {
        _logger = logger;
        _options = configuration.Load<RabbitMqOptions>();
        _connection = CreateConnection();
        _channel = _connection.CreateModel();

        DeclareQueues();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channel.Close();
        _connection.Close();

        return Task.CompletedTask;
    }

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

    private void DeclareQueues()
    {
        foreach (var name in _names)
        {
            var queue = $"request#{name}";

            _channel.QueueDeclare(
                queue: queue,
                durable: false,
                exclusive: false,
                autoDelete: false);
        }
    }

    #endregion
}