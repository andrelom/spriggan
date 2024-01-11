using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Spriggan.Core.Extensions;
using Spriggan.Core.Transport.Options;

namespace Spriggan.Core.Transport;

public class RabbitMqClient : IRabbitMqClient
{
    public RabbitMqClient(IConfiguration configuration)
    {
        var options = configuration.Load<RabbitMqOptions>();

        var factory = new ConnectionFactory
        {
            HostName = options.Host,
            UserName = options.User,
            Password = options.Password,
        };

        Connection = factory.CreateConnection();
    }

    public IConnection Connection { get; }
}
