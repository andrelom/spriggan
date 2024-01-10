namespace Spriggan.Core.Transport;

public class Bus : IBus
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancel = default) where TResponse : class
    {
        throw new NotImplementedException();
    }
}
