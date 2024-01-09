using MassTransit;

namespace Spriggan.Core.Transport;

public class Bus
{
    private readonly ILocalBus _local;

    private readonly IRemoteBus _remote;

    public Bus(ILocalBus local, IRemoteBus remote)
    {
        _local = local;
        _remote = remote;
    }

    public async Task<Response<TResponse>> Request<TRequest, TResponse>(TRequest message, CancellationToken token = default, RequestTimeout timeout = default) where TRequest : class where TResponse : class
    {
        Response<GetUserResponse> response;

        var local = Dependencies.Types.Any(item => item.GetInterfaces().Any(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IConsumer<TRequest>)));

        if (local)
        {
            return await _remote.Request<TRequest, TResponse>(message, token, timeout);
        }

        return await _local.Request<TRequest, TResponse>(message, token, timeout);
    }

    #region Private Methods

    // ...?

    #endregion
}

public interface ILocalBus : IBus
{
    // TODO: ...?
}

public interface IRemoteBus : IBus
{
    // TODO: ...?
}

public class GetUserRequest
{
    public string Name { get; set; }
}

public class GetUserResponse
{
    public string Name { get; set; }
}
