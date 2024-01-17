using EasyNetQ;
using Microsoft.Extensions.Hosting;

namespace Spriggan.Core.Transport;

public class RabbitMqConsumer : IHostedService
{
    private readonly EasyNetQ.IBus _bus;

    private static readonly IEnumerable<KeyValuePair<Type, Type>> Requests = GetRequestTypes();

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

    private static IEnumerable<KeyValuePair<Type, Type>> GetRequestTypes()
    {
        var target = typeof(IRequest<>);

        var requests = Dependencies.Domain.Where(type =>
        {
            return type.GetInterfaces().Any(source => source.IsGenericType && source.GetGenericTypeDefinition() == target);
        });

        return requests.Select((request) =>
        {
            var generic = request.GetInterfaces().FirstOrDefault(item => item.IsGenericType && item.GetGenericTypeDefinition() == typeof(IRequest<>));

            if (generic == null) throw new Exception();

            var response = generic.GetGenericArguments().First();

            if (response == null) throw new Exception();

            return new KeyValuePair<Type, Type>(request, response);
        });
    }

    private Task BindRequestHandlers()
    {
        var source = _bus.Rpc.GetType().GetMethod(nameof(_bus.Rpc.RequestAsync));

        if (source == null) throw new Exception();

        foreach (var kvp in Requests)
        {
            var method = source.MakeGenericMethod(kvp.Key, kvp.Value);

            if (method == null) throw new Exception();
        }

        return Task.CompletedTask;
    }

    private Task BindNotificationHandlers()
    {
        return Task.CompletedTask;
    }

    #endregion
}
