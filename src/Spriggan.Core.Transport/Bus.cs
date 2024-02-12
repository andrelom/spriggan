using MassTransit;

namespace Spriggan.Core.Transport;

public class Bus : IBus
{
    private readonly IBus.ILocal _local;

    private readonly IBus.IRemote _remote;

    private readonly IEnumerable<Type> _types = GetGenericTypeDefinitions();

    public Bus(IBus.ILocal local, IBus.IRemote remote)
    {
        _local = local;
        _remote = remote;
    }

    public async Task<TResponse> Request<TRequest, TResponse>(
        TRequest message,
        CancellationToken token = default,
        RequestTimeout timeout = default)
        where TRequest : class, IRequest<TResponse>
        where TResponse : class, IResult
    {
        Response<TResponse> response;

        var type = typeof(IConsumer<TRequest>);
        var isLocalBus = _types.Any(target => target == type);

        if (isLocalBus)
        {
            response = await _local.Request<TRequest, TResponse>(message, token, timeout);
        }
        else
        {
            response = await _remote.Request<TRequest, TResponse>(message, token, timeout);
        }

        return response.Message;
    }

    #region Private Methods

    private static IEnumerable<Type> GetGenericTypeDefinitions()
    {
        return Dependencies.Types
            .SelectMany(type => type.GetInterfaces())
            .Where(type => type.IsGenericType);
    }

    #endregion
}
