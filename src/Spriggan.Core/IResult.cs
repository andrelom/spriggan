namespace Spriggan.Core;

public interface IResult
{
    bool Ok { get; set; }

    string Error { get; set; }

    IDictionary<string, object>? Metadata { get; set; }
}

public interface IResult<T> : IResult
{
    T Data { get; set; }
}
