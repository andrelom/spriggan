namespace Spriggan.Core.Transport;

public interface IRabbitMqBus
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancel = default) where TResponse : class;
}
