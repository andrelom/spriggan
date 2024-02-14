namespace Spriggan.Core.Transport;

public interface IRequestHandler<in TRequest, TResponse> :
    MediatR.IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    // Intentionally left empty.
}
