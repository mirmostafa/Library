using System.Collections.Immutable;

using Library.Interfaces;

namespace Library.Results;

public interface IResult
{
    ImmutableArray<Exception> Errors { get; }
    Exception? Exception { get; }
    ResultBase? InnerResult { get; }
    bool IsFailure { get; }
    bool IsSucceed { get; }
    string? Message { get; }
}

public interface IResult<TValue> : IResult
{
    TValue Value { get; }
}