using System.Collections.Immutable;
using FluentValidation;
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

    private readonly IEnumerable<IValidator>? _validators;

    protected Consumer(IServiceProvider services)
    {
        _logger = services.GetService<ILogger<Consumer<TRequest, TResponse>>>();
        _validators = services.GetService<IEnumerable<IValidator<TRequest>>>();
    }

    protected abstract Task<TResponse> Handle(TRequest request);

    public async Task Consume(ConsumeContext<TRequest> context)
    {
        if (HandleValidation(context.Message) is { } response)
        {
            await context.RespondAsync(response);

            return;
        }

        try
        {
            response = await Handle(context.Message);
        }
        catch (Exception ex)
        {
            response = HandleException(ex);
        }

        await context.RespondAsync(response);
    }

    #region Private Methods

    private ImmutableArray<string>? GetValidationMessages(TRequest request)
    {
        var context = new ValidationContext<TRequest>(request);

        return _validators?
            .Select(validator => validator.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .Select(failure => failure.ErrorMessage)
            .ToImmutableArray();
    }

    private TResponse? HandleValidation(TRequest request)
    {
        if (_validators == null) return null;

        if (!_validators.Any()) return null;

        var context = new ValidationContext<TRequest>(request);

        var messages = _validators
            .Select(validator => validator.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .Select(failure => failure.ErrorMessage)
            .ToImmutableArray();

        if (!messages.Any()) return null;

        return new TResponse
        {
            Ok = false,
            Error = Errors.Validation,
            Metadata = new Dictionary<string, object>
            {
                { "Validations", messages },
            }
        };
    }

    private TResponse HandleException(Exception ex)
    {
        var id = Guid.NewGuid();

        _logger?.LogError(ex, "Transport Consumer Exception ({0})", id);

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
