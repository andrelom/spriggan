using EasyNetQ;

namespace Spriggan.Core.Transport.RabbitMq;

public class RabbitMqBus : IBus
{
    private readonly EasyNetQ.IBus _bus;

    public RabbitMqBus()
    {
        _bus = RabbitHutch.CreateBus("host=localhost");
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancel = default) where TResponse : class
    {
        return await _bus.Rpc.RequestAsync<IRequest<TResponse>, TResponse>(request, cancellationToken: cancel);
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancel = default) where TNotification : INotification
    {
        await _bus.PubSub.PublishAsync(notification, cancellationToken: cancel);
    }
}
