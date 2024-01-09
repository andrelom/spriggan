using MassTransit;

namespace Spriggan.Core.Transport;

public class Bus : IBus
{
    private readonly ILocalBus _local;

    private readonly IRemoteBus _remote;

    private readonly IEnumerable<Type> _types = GetGenericTypeDefinitions();

    public Bus(ILocalBus local, IRemoteBus remote)
    {
        _local = local;
        _remote = remote;
    }

    public async Task<Response<TResponse>> Request<TRequest, TResponse>(
        TRequest message,
        CancellationToken token = default,
        RequestTimeout timeout = default)
        where TRequest : class
        where TResponse : class
    {
        var isLocal = _types.Any(type => type == typeof(IConsumer<TRequest>));

        if (isLocal)
        {
            return await _local.Request<TRequest, TResponse>(message, token, timeout);
        }

        return await _remote.Request<TRequest, TResponse>(message, token, timeout);
    }

    #region Private Methods

    private static IEnumerable<Type> GetGenericTypeDefinitions()
    {
        return Dependencies.Types
            .SelectMany(type => type.GetInterfaces())
            .Where(type => type.IsGenericType)
            .Select(type => type.GetGenericTypeDefinition());
    }

    #endregion
}
