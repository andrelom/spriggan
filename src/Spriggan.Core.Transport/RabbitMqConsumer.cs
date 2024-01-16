using EasyNetQ;
using Microsoft.Extensions.Hosting;

namespace Spriggan.Core.Transport;

public class RabbitMqConsumer : IHostedService
{
    private readonly EasyNetQ.IBus _bus;

    public RabbitMqConsumer()
    {
        _bus = RabbitHutch.CreateBus("host=localhost");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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

    private async Task HandleRequests()
    {
        var types = GetRequestTypes();
        var method = _bus.PubSub.GetType().GetMethod(nameof(_bus.PubSub.SubscribeAsync));

        if (method == null) return;

        foreach (var type in types)
        {
            var generic = method.MakeGenericMethod(type);
        }
    }

    private Task HandleSubscribers()
    {
        throw new NotImplementedException();
    }

    #endregion
}
