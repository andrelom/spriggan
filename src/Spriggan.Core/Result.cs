namespace Spriggan.Core;

/// <summary>
/// It provides a protocol to standardize communication between local and remote services.
/// </summary>
public sealed class Result : IResult
{
    /// <summary>
    /// Whether the process was successful.
    /// </summary>
    public bool Ok { get; set; }

    /// <summary>
    /// Error that occurred during the process.
    /// </summary>
    public string Error { get; set; } = null!;

    /// <summary>
    /// Any information related to the Error.
    /// </summary>
    public IDictionary<string, object>? Metadata { get; set; }

    /// <summary>
    /// Returns True if any of the reported errors match the current error.
    /// </summary>
    /// <param name="values">The error types.</param>
    /// <returns>If it matches the current error.</returns>
    public bool IsError(params string[] values)
    {
        return !Ok && values.Any(value => value.Equals(Error, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Create a new successful result.
    /// </summary>
    /// <returns>A result.</returns>
    public static Result Success()
    {
        return new()
        {
            Ok = true
        };
    }

    /// <summary>
    /// Create a new unsuccessful result.
    /// </summary>
    /// <param name="error">The resulting error.</param>
    /// <param name="metadata">Any information related to the Error.</param>
    /// <returns>A result.</returns>
    public static Result Fail(string error, IDictionary<string, object>? metadata = null)
    {
        return new()
        {
            Ok = false,
            Error = error,
            Metadata = metadata
        };
    }

    /// <summary>
    /// Create a new unsuccessful result from a source.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <returns>A result.</returns>
    public static Result Fail(Result source)
    {
        return Fail(source.Error, source.Metadata);
    }

    /// <summary>
    /// Create a new unsuccessful result from a source.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <returns>A result.</returns>
    public static Result Fail(Result<object> source)
    {
        return Fail(source.Error, source.Metadata);
    }
}

/// <summary>
/// It provides a protocol to standardize communication between local and remote services.
/// </summary>
/// <typeparam name="T">The resulting data type.</typeparam>
public sealed class Result<T> : IResult<T>
{
    /// <summary>
    /// Whether the process was successful.
    /// </summary>
    public bool Ok { get; set; }

    /// <summary>
    /// Data that was produced during the process.
    /// </summary>
    public T Data { get; set; } = default!;

    /// <summary>
    /// Error that occurred during the process.
    /// </summary>
    public string Error { get; set; } = null!;

    /// <summary>
    /// Any information related to the Error.
    /// </summary>
    public IDictionary<string, object>? Metadata { get; set; }

    /// <summary>
    /// Returns True if any of the reported errors match the current error.
    /// </summary>
    /// <param name="values">The error types.</param>
    /// <returns>If it matches the current error.</returns>
    public bool IsError(params string[] values)
    {
        return !Ok && values.Any(value => value.Equals(Error, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Create a new successful result.
    /// </summary>
    /// <param name="data">The resulting data.</param>
    /// <returns>A result.</returns>
    public static Result<T> Success(T data)
    {
        return new()
        {
            Ok = true,
            Data = data
        };
    }

    /// <summary>
    /// Create a new unsuccessful result.
    /// </summary>
    /// <param name="error">The resulting error.</param>
    /// <param name="metadata">Any information related to the Error.</param>
    /// <returns>A result.</returns>
    public static Result<T> Fail(string error, IDictionary<string, object>? metadata = null)
    {
        return new()
        {
            Ok = false,
            Error = error,
            Metadata = metadata
        };
    }

    /// <summary>
    /// Create a new unsuccessful result from a source.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <returns>A result.</returns>
    public static Result<T> Fail(Result source)
    {
        return Fail(source.Error, source.Metadata);
    }

    /// <summary>
    /// Create a new unsuccessful result from a source.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <returns>A result.</returns>
    public static Result<T> Fail<TSource>(Result<TSource> source)
    {
        return Fail(source.Error, source.Metadata);
    }
}
