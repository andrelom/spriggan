using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Spriggan.Core.Transport;

public interface IRabbitMqClient : IDisposable
{
    IModel Channel { get; }

    EventingBasicConsumer Consumer { get; }
}
