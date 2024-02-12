namespace Spriggan.Core.Transport;

public interface IRequest<out TResponse> : IMessage where TResponse : IResult
{
    // Intentionally left empty.
}
