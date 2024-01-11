using RabbitMQ.Client;

namespace Spriggan.Core.Transport;

public interface IRabbitMqClient : IDisposable
{
    IModel Channel { get; }

    IEnumerable<string> RequestQueues { get; }

    IEnumerable<string> ResponseQueues { get; }
}
