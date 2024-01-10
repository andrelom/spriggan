using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Spriggan.Core.Extensions;
using Spriggan.Core.Transport.Options;

namespace Spriggan.Core.Transport;

public class ConsumerHostedService : IHostedService
{
    private readonly ILogger<ConsumerHostedService> _logger;

    private readonly RabbitMqOptions _options;

    private readonly IConnection _connection;

    private readonly IModel _channel;

    private readonly IEnumerable<Type> _types = GetRequestHandlerTypes();

    public ConsumerHostedService(ILogger<ConsumerHostedService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _options = configuration.Load<RabbitMqOptions>();
        _connection = CreateConnection();
        _channel = _connection.CreateModel();
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

    private static IEnumerable<Type> GetRequestHandlerTypes()
    {
        var target = typeof(IRequestHandler<,>);
        var types = Dependencies.Domain.SelectMany(type => type.GetInterfaces()).Where(type => type.IsGenericType);

        return types.Where(source => source.GetGenericTypeDefinition() == target);
    }

    #endregion
}
