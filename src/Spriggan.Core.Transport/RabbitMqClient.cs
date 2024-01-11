using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Spriggan.Core.Extensions;
using Spriggan.Core.Transport.Extensions;
using Spriggan.Core.Transport.Options;

namespace Spriggan.Core.Transport;

public class RabbitMqClient : IRabbitMqClient
{
    private readonly IConnection _connection;

    private readonly IModel _channel;

    public RabbitMqClient(IConfiguration configuration)
    {
        var options = configuration.Load<RabbitMqOptions>();

        var factory = new ConnectionFactory
        {
            HostName = options.Host,
            UserName = options.User,
            Password = options.Password,
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        RequestQueues = DeclareQueues("request");
        ResponseQueues = DeclareQueues("response");
    }

    public IModel Channel => _channel;

    public IEnumerable<string> RequestQueues { get; }

    public IEnumerable<string> ResponseQueues { get; }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
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

    private IEnumerable<string> DeclareQueues(string prefix)
    {
        var names = GetRequestTypes().Select(type => type.ToQueueName(prefix)).ToArray();

        foreach (var name in names)
        {
            _channel.QueueDeclare(
                queue: name,
                durable: true,
                exclusive: false,
                autoDelete: false);
        }

        return names;
    }

    #endregion
}
