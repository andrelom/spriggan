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

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await BindRequestHandlers();
        await BindSubscribeHandlers();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
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

    private async Task BindRequestHandlers()
    {
        var types = GetRequestTypes();
        var source = _bus.PubSub.GetType().GetMethod(nameof(_bus.PubSub.SubscribeAsync));

        if (source == null) return;

        foreach (var type in types)
        {
            var method = source.MakeGenericMethod(type);
            var action = typeof(Action<>).MakeGenericType(type);

            var activator = Activator.CreateInstance(action, new Action<object>(response =>
            {
                Console.WriteLine(response);
            }));

            method.Invoke(_bus.PubSub, new[] { "id", activator });
        }
    }

    private Task BindSubscribeHandlers()
    {
        return Task.CompletedTask;
    }

    #endregion
}
