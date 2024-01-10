namespace Spriggan.Core.Transport;

public interface IRequest<out TResponse> : IMessage where TResponse : class
{
    // Intentionally left empty.
}
