namespace Library.Results;

public sealed class TryMethodResult
    : ResultBase
{
    private TryMethodResult(
    in bool? succeed = null,
    in string? message = null,
    in IEnumerable<Exception>? errors = null,
    in IEnumerable<object>? extraData = null)
    : base(succeed, message, errors, innerResult: null) { }

    /// <summary>
    /// Creates a new TryMethodResult with a given value, status, message, errors, and extra data.
    /// </summary>
    /// <param name="value">The value of the TryMethodResult.</param>
    /// <param name="status">The status of the TryMethodResult.</param>
    /// <param name="message">The message of the TryMethodResult.</param>
    /// <param name="errors">The errors of the TryMethodResult.</param>
    /// <param name="extraData">The extra data of the TryMethodResult.</param>
    /// <returns>
    /// A new TryMethodResult with the given value, status, message, errors, and extra data.
    /// </returns>
    public static TryMethodResult Fail(
        in string? message = null,
        in IEnumerable<Exception>? errors = null,
        in IEnumerable<object>? extraData = null)
        => new(false, message, errors, extraData);

    public static TryMethodResult Fail(
        in Exception exception)
        => new(false, errors: [exception]);

    /// <summary>
    /// Creates a new TryMethodResult with the given value, status, message, errors, and extraData.
    /// </summary>
    /// <param name="value">The value of the TryMethodResult.</param>
    /// <param name="status">The status of the TryMethodResult.</param>
    /// <param name="message">The message of the TryMethodResult.</param>
    /// <param name="errors">The errors of the TryMethodResult.</param>
    /// <param name="extraData">The extraData of the TryMethodResult.</param>
    /// <returns>A new TryMethodResult with the given value, status, message, errors, and extraData.</returns>
    public static TryMethodResult Success(
        in string? message = null,
        in IEnumerable<Exception>? errors = null,
        in IEnumerable<object>? extraData = null) =>
        new(true, message, errors, extraData);

    public static explicit operator bool(TryMethodResult result) =>
        result.IsSucceed;

    public static TryMethodResult New(bool? succeed, string? message = null) =>
        new(succeed, message: message);

    /// <summary>
    /// Creates a new TryMethodResult object with the specified value, success status, and optional message.
    /// </summary>
    public static TryMethodResult TryParseResult(bool? succeed, string? message = null) =>
        New(succeed, message: message);
}

public sealed class TryMethodResult<TValue>(in TValue? value,
    in bool? succeed = null,
    in string? message = null,
    in IEnumerable<Exception>? errors = null,
    in IEnumerable<object>? ExtraData = null)
    : Result<TValue?>(value, succeed, message, errors, ExtraData)
{
    /// <summary>
    /// Creates a new TryMethodResult with a given value, status, message, errors, and extra data.
    /// </summary>
    /// <param name="value">The value of the TryMethodResult.</param>
    /// <param name="message">The message of the TryMethodResult.</param>
    /// <param name="errors">The errors of the TryMethodResult.</param>
    /// <param name="extraData">The extra data of the TryMethodResult.</param>
    /// 
    /// <returns>
    /// A new TryMethodResult with the given value, status, message, errors, and extra data.
    /// </returns>
    public static TryMethodResult<TValue?> CreateFailure(in TValue? value = default,
        in string? message = null,
        in IEnumerable<Exception>? errors = null,
        in IEnumerable<object>? extraData = null)
        => new(value, false, message, errors, extraData);

    /// <summary>
    /// Creates a new TryMethodResult with the given value, status, message, errors, and extraData.
    /// </summary>
    /// <param name="value">The value of the TryMethodResult.</param>
    /// <param name="message">The message of the TryMethodResult.</param>
    /// <param name="errors">The errors of the TryMethodResult.</param>
    /// <param name="extraData">The extraData of the TryMethodResult.</param>
    /// 
    /// <returns>A new TryMethodResult with the given value, status, message, errors, and extraData.</returns>
    public static TryMethodResult<TValue> CreateSuccess(in TValue value,
        in string? message = null,
        in IEnumerable<Exception>? errors = null,
        in IEnumerable<object>? extraData = null) =>
        new(value, true, message, errors, extraData);

    public static explicit operator bool(TryMethodResult<TValue?> result) =>
        result.IsSucceed;

    public static explicit operator TValue?(TryMethodResult<TValue> result) =>
        result.Value;

    /// <summary>
    /// Creates a new TryMethodResult object with the specified value, success status, and optional message.
    /// </summary>
    public static TryMethodResult<TValue> TryParseResult(bool? succeed, TValue? value, string? message = null) =>
        new(value, succeed, message: message);
}