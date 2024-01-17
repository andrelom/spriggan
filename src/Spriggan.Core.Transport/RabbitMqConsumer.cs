using System.Data;
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
            var generic = request.GetInterfaces().FirstOrDefault(item => item.IsGenericType && item.GetGenericTypeDefinition() == target);

            if (generic == null) throw new NoNullAllowedException();

            var response = generic.GetGenericArguments().First();

            if (response == null) throw new NoNullAllowedException();

            return new KeyValuePair<Type, Type>(request, response);
        });
    }

    private Task BindRequestHandlers()
    {
        var source = _bus.Rpc.GetType().GetMethod(nameof(_bus.Rpc.RequestAsync));

        if (source == null) throw new NoNullAllowedException();

        foreach (var kvp in Requests)
        {
            var method = source.MakeGenericMethod(kvp.Key, kvp.Value);

            if (method == null) throw new NoNullAllowedException();

            var action = new Action<dynamic>((message) => { });

            var task = method.Invoke(_bus.Rpc, new object[] { action, null, null }) as Task;

            if (task == null) throw new NoNullAllowedException();
        }

        return Task.CompletedTask;
    }

    private Task BindNotificationHandlers()
    {
        return Task.CompletedTask;
    }

    #endregion
}
