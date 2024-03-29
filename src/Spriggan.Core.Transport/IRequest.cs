namespace Spriggan.Core.Transport;

public interface IRequest<out TResponse> : IMessage, MediatR.IRequest<TResponse> where TResponse : IResult
{
    // Intentionally left empty.
}
