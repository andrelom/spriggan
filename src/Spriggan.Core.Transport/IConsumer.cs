namespace Spriggan.Core.Transport;

public interface IConsumer<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    Task<TResponse> Consume(TRequest message);
}
