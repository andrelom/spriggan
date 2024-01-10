using MassTransit;

namespace Spriggan.Core.Transport;

public interface IBus
{
    Task<TResponse> Request<TRequest, TResponse>(
        TRequest message,
        CancellationToken token = default,
        RequestTimeout timeout = default)
        where TRequest : class
        where TResponse : class;
}


public interface ILocalBus : MassTransit.IBus
{
    // Intentionally left empty.
}

public interface IRemoteBus : MassTransit.IBus
{
    // Intentionally left empty.
}
