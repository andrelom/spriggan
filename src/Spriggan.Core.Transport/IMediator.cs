namespace Spriggan.Core.Transport;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancel = default)
        where TResponse : IResult;

    Task Publish<TNotification>(
        TNotification notification,
        CancellationToken cancel = default)
        where TNotification : INotification;
}
