namespace Spriggan.Core.Transport;

public interface IRequestHandler<in TRequest, TResponse> : MediatR.IRequestHandler<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class
{
    // Intentionally left empty.
}
