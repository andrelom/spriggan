namespace Spriggan.Core.Transport;

public delegate Task<TResponse> RequestDelegate<TRequest, TResponse>(TRequest request) where TRequest : class, IRequest<TResponse> where TResponse : class;

public interface IMiddleware<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class
{
    Task<TResponse> Handle(TRequest request, RequestDelegate<TRequest, TResponse> next, CancellationToken cancel);
}
