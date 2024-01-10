namespace Spriggan.Core.Transport;

public delegate Task<TResponse> RequestDelegate<out TRequest, TResponse>() where TRequest : class, IRequest<TResponse> where TResponse : class;

public interface IMiddleware<in TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class
{
    Task<TResponse> Handle(RequestDelegate<TRequest, TResponse> next, CancellationToken cancel);
}
