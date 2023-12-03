namespace Spriggan.Core.Transport;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancel = default) where TResponse : class;

    Task Publish<TNotification>(TNotification notification, CancellationToken cancel = default) where TNotification : INotification;
}
