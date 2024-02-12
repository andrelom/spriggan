namespace Spriggan.Core.Transport;

internal class Mediator : IMediator
{
    private readonly MediatR.IMediator _mediator;

    public Mediator(MediatR.IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancel = default) where TResponse : IResult
    {
        return await _mediator.Send(request, cancel);
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancel = default) where TNotification : INotification
    {
        await _mediator.Publish(notification, cancel);
    }
}
