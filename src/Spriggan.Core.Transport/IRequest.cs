namespace Spriggan.Core.Transport;

public interface IRequest<out TResponse> : IMessage where TResponse : class, IResult
{
    // Intentionally left empty.
}
