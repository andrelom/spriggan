using System.Data;
using System.Reflection;
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
        var source = _bus.Rpc.GetType().GetMethod(nameof(_bus.Rpc.RespondAsync));

        if (source == null) throw new NoNullAllowedException();

        foreach (var kvp in Requests)
        {
            var method = source.MakeGenericMethod(kvp.Key, kvp.Value);

            if (method == null) throw new NoNullAllowedException();

            var genericMethod = GetType()
                .GetMethod(nameof(CreateResponder), BindingFlags.Instance | BindingFlags.NonPublic)
                ?.MakeGenericMethod(kvp.Key, kvp.Value);

            if (genericMethod == null) throw new NoNullAllowedException();

            var responder = genericMethod.Invoke(this, null);

            Action<IResponderConfiguration> configure = configuration => { };

            var task = method.Invoke(_bus.Rpc, new object[] { responder, configure, null });
        }

        return Task.CompletedTask;
    }

    private Task BindNotificationHandlers()
    {
        return Task.CompletedTask;
    }

    // Define a generic method that matches the signature of RespondAsync
    private Func<TRequest, CancellationToken, Task<TResponse>> CreateResponder<TRequest, TResponse>()
    {
        return (request, cancellationToken) => Task.FromResult(HandleRequest<TRequest, TResponse>(request));
    }

    private TResponse HandleRequest<TRequest, TResponse>(TRequest request)
    {
        // Your implementation for handling the request
        return default;
    }

    #endregion
}
