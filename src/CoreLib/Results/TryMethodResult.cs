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

    public static new TryMethodResult<TValue> CreateSuccess(in TValue value,
        in object? status = null,
        in string? message = null,
        in IEnumerable<(object Id, object Error)>? errors = null,
        in ImmutableDictionary<string, object>? extraData = null)
        => new(value, true, status, message, errors, extraData);
    public static new TryMethodResult<TValue?> CreateFail(in TValue? value = default,
        in object? status = null,
        in string? message = null,
        in IEnumerable<(object Id, object Error)>? errors = null,
        in ImmutableDictionary<string, object>? extraData = null)
        => new(value, false, status, message, errors, extraData);
}