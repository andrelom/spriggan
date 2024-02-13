namespace Spriggan.Core.Transport;

public interface IConsumer<in TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class, IResult
{
    Task<TResponse> Consume(TRequest message);
}
