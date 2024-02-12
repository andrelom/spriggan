using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Spriggan.Core.Transport;

public abstract class Consumer<TRequest, TResponse> :
    IConsumer<TRequest>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class, IResult, new()
{
    private readonly ILogger<Consumer<TRequest, TResponse>>? _logger;

    protected Consumer(IServiceProvider services)
    {
        _logger = services.GetService<ILogger<Consumer<TRequest, TResponse>>>();
    }

    protected abstract Task<TResponse> Handle(TRequest request);

    public async Task Consume(ConsumeContext<TRequest> context)
    {
        try
        {
            var response = await Handle(context.Message);

            await context.RespondAsync(response);
        }
        catch (Exception ex)
        {
            var response = HandleException(ex);

            await context.RespondAsync(response);
        }
    }

    #region Private Methods

    private TResponse HandleException(Exception ex)
    {
        var id = Guid.NewGuid();

        _logger?.LogError(ex, "Transport Exception Middleware ({0})", id);

        return new TResponse
        {
            Ok = false,
            Error = Errors.Whoops,
            Metadata = new Dictionary<string, object>
            {
                { "Trace", id },
            }
        };
    }

    #endregion
}
