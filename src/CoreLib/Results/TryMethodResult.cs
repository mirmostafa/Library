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
    public TryMethodResult(bool? succeed, TValue? value)
        : this(value, succeed)
    {
    }

    public static explicit operator bool(TryMethodResult<TValue?> result) => result.IsSucceed;

    public static explicit operator TValue?(TryMethodResult<TValue> result) => result.Value;
}