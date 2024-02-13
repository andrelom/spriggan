using MassTransit;

namespace Spriggan.Core.Transport;

public class MassTransitConsumer<TRequest, TResponse> :
    IConsumer<TRequest>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class, IResult
{
    private readonly IConsumer<TRequest, TResponse> _consumer;

    public MassTransitConsumer(IConsumer<TRequest, TResponse> consumer)
    {
        _consumer = consumer;
    }

    public async Task Consume(ConsumeContext<TRequest> context)
    {
        var response = await _consumer.Consume(context.Message);

        await context.RespondAsync(response);
    }
}
