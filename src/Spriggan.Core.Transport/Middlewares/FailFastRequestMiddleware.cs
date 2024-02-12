using System.Collections.Immutable;
using FluentValidation;

namespace Spriggan.Core.Transport.Middlewares;

public class FailFastRequestMiddleware<TRequest, TResponse> :
    IMiddleware<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class, IResult, new()
{
    private readonly IEnumerable<IValidator> _validators;

    public FailFastRequestMiddleware(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestDelegate<TRequest, TResponse> next, CancellationToken cancel)
    {
        if (!_validators.Any())
        {
            return await next(request);
        }

        var validations = GetErrorMessages(request);

        if (!validations.Any())
        {
            return await next(request);
        }

        return new TResponse
        {
            Ok = false,
            Error = Errors.Validation,
            Metadata = new Dictionary<string, object>
            {
                { "Validations", validations },
            }
        };
    }

    #region Private Methods

    private ImmutableArray<string> GetErrorMessages(TRequest request)
    {
        var context = new ValidationContext<TRequest>(request);

        return _validators
            .Select(validator => validator.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .Select(failure => failure.ErrorMessage)
            .ToImmutableArray();
    }

    #endregion
}
