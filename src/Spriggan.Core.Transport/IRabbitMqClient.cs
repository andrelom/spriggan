using RabbitMQ.Client;

namespace Spriggan.Core.Transport;

public interface IRabbitMqClient
{
    IConnection Connection { get; }
}
