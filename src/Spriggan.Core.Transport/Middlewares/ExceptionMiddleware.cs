using Microsoft.Extensions.Logging;

namespace Spriggan.Core.Transport.Middlewares;

public class ExceptionMiddleware<TRequest, TResponse>:
    IMiddleware<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class, IResult, new()
{
    private readonly ILogger<ExceptionMiddleware<TRequest, TResponse>> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestDelegate<TRequest, TResponse> next, CancellationToken cancel)
    {
        try
        {
            return await next(request);
        }
        catch (Exception ex)
        {
            return await HandleException(ex);
        }
    }

    #region Private Methods

    private Task<TResponse> HandleException(Exception ex)
    {
        var id = Guid.NewGuid();

        _logger.LogError(ex, "Transport Exception Behavior ({0})", id);

        return Task.FromResult(new TResponse
        {
            Ok = false,
            Error = Errors.Whoops,
            Metadata = new Dictionary<string, object>
            {
                { "Trace", id },
            }
        });
    }

    #endregion
}
