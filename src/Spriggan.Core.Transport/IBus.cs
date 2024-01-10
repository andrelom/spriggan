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

    #region Nested Interfaces

    public interface ILocal : MassTransit.IBus;

    public interface IRemote : MassTransit.IBus;

    #endregion
}
