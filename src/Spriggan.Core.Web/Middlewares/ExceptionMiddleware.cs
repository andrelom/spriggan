using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Spriggan.Core.Extensions;

namespace Spriggan.Core.Web.Middlewares;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    private readonly RequestDelegate _next;

    public ExceptionMiddleware(
        ILogger<ExceptionMiddleware> logger,
        RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    #region Private Methods

    private async Task HandleException(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Exception Handler Middleware ({0})", context.TraceIdentifier);

        context.Response.Headers.Add(HeaderNames.CacheControl, "no-cache, no-store, must-revalidate");

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = MediaTypeNames.Application.Json;

        var result = Result.Fail(Errors.Whoops, new
        {
            context.TraceIdentifier
        });

        await context.Response.WriteAsync(result.ToJson());
    }

    #endregion
}
