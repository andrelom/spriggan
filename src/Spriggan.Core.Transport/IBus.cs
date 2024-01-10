namespace Spriggan.Core.Transport;

public interface IBus
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancel = default) where TResponse : class;
}
