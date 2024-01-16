using EasyNetQ;
using Microsoft.Extensions.Hosting;

namespace Spriggan.Core.Transport;

public class RabbitMqConsumer : IHostedService
{
    private readonly EasyNetQ.IBus _bus;

    private static readonly IEnumerable<Type> Requests = GetGenericTypes(typeof(IRequest<>));

    private static readonly IEnumerable<Type> Notifications = GetGenericTypes(typeof(INotification));

    public RabbitMqConsumer()
    {
        _bus = RabbitHutch.CreateBus("host=localhost");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await BindRequestHandlers();
        await BindNotificationHandlers();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    #region Private Methods

    private static IEnumerable<Type> GetGenericTypes(Type target)
    {
        return Dependencies.Domain.Where(type =>
        {
            return type.GetInterfaces().Any(source => source.IsGenericType && source.GetGenericTypeDefinition() == target);
        });
    }

    private Task BindRequestHandlers()
    {
        var source = _bus.PubSub.GetType().GetMethod(nameof(_bus.PubSub.SubscribeAsync));

        if (source == null)
        {
            throw new Exception();
        }

        foreach (var type in Requests)
        {
            var method = source.MakeGenericMethod(type);
        }

        return Task.CompletedTask;
    }

    private Task BindNotificationHandlers()
    {
        return Task.CompletedTask;
    }

    #endregion
}
