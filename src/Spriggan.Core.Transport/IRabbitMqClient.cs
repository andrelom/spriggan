using RabbitMQ.Client.Events;

namespace Spriggan.Core.Transport;

public interface IRabbitMqClient : IDisposable
{
    void AddConsumer(EventHandler<BasicDeliverEventArgs> consumer);
}
