using MediatR;
using Microsoft.Extensions.Logging;

namespace Spriggan.Core.Transport.Behaviors;

public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class, IResult, new()
{
    private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;

    public ExceptionBehavior(ILogger<ExceptionBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancel)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #region Private Methods

    private TResponse HandleException(Exception ex)
    {
        var id = Guid.NewGuid();

        _logger.LogError(ex, "Transport Exception Behavior ({0})", id);

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
