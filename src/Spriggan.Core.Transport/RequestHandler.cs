using MassTransit;

namespace Spriggan.Core.Transport;

public abstract class RequestHandler<TRequest, TResponse> :
    IConsumer<TRequest>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class
{
    protected abstract Task<TResponse> Handle(TRequest request, CancellationToken cancel = default);

    public async Task Consume(ConsumeContext<TRequest> context)
    {
        var request = context.Message;
        var response = await Handle(request);

        await context.RespondAsync(response);
    }
}
