using System.Collections.Immutable;

namespace Library.Results;

public sealed record TryMethodResult<TValue>(in TValue? Value,
                                             in bool? Succeed = null,
                                             in object? Status = null,
                                             in string? Message = null,
                                             in IEnumerable<(object Id, object Error)>? Errors = null,
                                             in ImmutableDictionary<string, object>? ExtraData = null)
    : Result<TValue?>(Value, Succeed, Status, Message, Errors, ExtraData)
{
    public static TryMethodResult<TValue> TryParseResult(bool? succeed, TValue? value, string? message = null)
        => new(value, succeed, Message: message);

    public static explicit operator bool(TryMethodResult<TValue?> result) => result.IsSucceed;

    public static explicit operator TValue?(TryMethodResult<TValue> result) => result.Value;
}