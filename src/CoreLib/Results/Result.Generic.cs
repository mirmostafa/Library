using Library.Interfaces;

namespace Library.Results;

public class Result<TValue>(TValue value,
    in bool? succeed = null,
    in object? status = null,
    in string? message = null,
    in IEnumerable<(object Id, object Error)>? errors = null,
    in IEnumerable<(string Id, object Data)>? extraData = null)
    : ResultBase(succeed, status, message, errors, extraData)
    , IAdditionOperators<Result<TValue>, ResultBase, Result<TValue>>
    , IEquatable<Result<TValue>>
{
    private static Result<TValue?>? _failure;

    public Result(Result<TValue> original)
            : this(original.Value, original.Succeed, original.Status, original.Message, original.Errors, original.ExtraData)
    {
    }

    /// <summary>
    /// Gets a Result object representing a failed operation.
    /// </summary>
    /// <returns>A Result object representing a failed operation.</returns>
    public static Result<TValue?> Failure => _failure ??= CreateFailure();

    public TValue Value => value;

    /// <summary> Combines multiple Result<TValue> objects into a single Result<TValue> object.
    /// </summary> <param name="results">The Result<TValue> objects to combine.</param> <returns>A
    /// single Result<TValue> object containing the combined results.</returns>
    public static Result<TValue> Combine(IEnumerable<Result<TValue>> results, Func<TValue, TValue, TValue> add) =>
        Combine(add, results.ToArray());

    public static Result<TValue> Combine(Func<TValue, TValue, TValue> add, params Result<TValue>[] resultArray)
    {
        var data = Combine(resultArray);
        var valueArray = resultArray.Select(x => x.Value).ToArray();
        var value = valueArray[0];
        foreach (var v in valueArray.Skip(1))
        {
            value = add(value, v);
        }
        var result = new Result<TValue>(value, data.Succeed, data.Status, data.Message, data.Errors, data.ExtraData);
        return result;
    }

    /// <summary>
    /// Creates a new Result with the given parameters and a success value of false.
    /// </summary>
    /// <param name="value">The value of the Result.</param>
    /// <param name="status">The status of the Result.</param>
    /// <param name="message">The message of the Result.</param>
    /// <param name="errors">The errors of the Result.</param>
    /// <param name="extraData">The extra data of the Result.</param>
    /// <returns>A new Result with the given parameters and a success value of false.</returns>
    public static Result<TValue?> CreateFailure(in TValue? value = default,
        in object? status = null,
        in string? message = null,
        in IEnumerable<(object Id, object Error)>? errors = null,
        in IEnumerable<(string Id, object Data)>? extraData = null)
        => new(value, false, status, message, errors, extraData);

    /// <summary>
    /// Creates a new Result with the given value, status, message, and error.
    /// </summary>
    /// <param name="value">The value of the Result.</param>
    /// <param name="status">The status of the Result.</param>
    /// <param name="message">The message of the Result.</param>
    /// <param name="error">The error of the Result.</param>
    /// <returns>A new Result with the given value, status, message, and error.</returns>
    public static Result<TValue?> CreateFailure(in TValue? value,
        in object? status,
        in string? message,
        in (object Id, object Error) error)
        => new(value, false, status, message, EnumerableHelper.ToEnumerable(error), null);

    /// <summary>
    /// Creates a failure result with the specified message, exception and value.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="ex">The exception.</param>
    /// <param name="value">The value.</param>
    /// <returns>A failure result.</returns>
    public static Result<TValue?> CreateFailure(in string message, in Exception ex, in TValue? value)
        => CreateFailure(value, ex, message);

    /// <summary>
    /// Creates a Result with a failure status and an Exception.
    /// </summary>
    /// <param name="error">The Exception to be stored in the Result.</param>
    /// <param name="value">The value to be stored in the Result.</param>
    /// <returns>A Result with a failure status and an Exception.</returns>
    public static Result<TValue?> CreateFailure(in Exception error)
        => CreateFailure(default, error, null);

    /// <summary>
    /// Creates a Result with a failure status and an Exception.
    /// </summary>
    /// <param name="error">The Exception to be stored in the Result.</param>
    /// <param name="value">The value to be stored in the Result.</param>
    /// <returns>A Result with a failure status and an Exception.</returns>
    public static Result<TValue> CreateFailure(in Exception error, in TValue value)
        => CreateFailure(value, error, null)!;

    public static Result<TValue?> CreateFailure(in string message, in TValue value)
            => CreateFailure(value, null, message);

    /// <summary>
    /// Creates a new successful Result with the given value, status, message, errors and extra data.
    /// </summary>
    /// <param name="value">The value of the Result.</param>
    /// <param name="status">The status of the Result.</param>
    /// <param name="message">The message of the Result.</param>
    /// <param name="errors">The errors of the Result.</param>
    /// <param name="extraData">The extra data of the Result.</param>
    /// <returns>A new successful Result.</returns>
    public static Result<TValue> CreateSuccess(in TValue value,
        in object? status = null,
        in string? message = null,
        in IEnumerable<(object Id, object Error)>? errors = null,
        in IEnumerable<(string Id, object Data)>? extraData = null)
        => new(value, true, status, message, errors, extraData);

    public static Result<TValue> From(in Result result, in TValue value)
            => new(value, result.Succeed, result.Status, result.Message, result.Errors, result.ExtraData);

    public static implicit operator Result(Result<TValue> result) =>
            new(result.Succeed, result.Status, result.Message, result.Errors, result.ExtraData);

    public static implicit operator TValue(Result<TValue> result) =>
            result.Value;

    /// <summary>
    /// Creates a new Result object with the given value, succeed, status, message, errors, and extraData.
    /// </summary>
    public static Result<TValue> New(in TValue value,
        in bool? succeed = null,
        in object? status = null,
        in string? message = null,
        in IEnumerable<(object Id, object Error)>? errors = null,
        in IEnumerable<(string Id, object Data)>? extraData = null)
        => new(value, succeed, status, message, errors, extraData);

    public static Result<TValue> operator +(Result<TValue> left, ResultBase right)
    {
        var total = Combine(left, right);
        var result = new Result<TValue>(left.Value, total.Succeed, total.Status, total.Message, total.Errors, total.ExtraData);
        return result;
    }

    public Result<TValue> Add(Result<TValue> item, Func<TValue, TValue, TValue> add) =>
        Result<TValue>.Combine(add, item);

    public void Deconstruct(out bool IsSucceed, out TValue Value) =>
        (IsSucceed, Value) = (this.IsSucceed, this.Value);

    public bool Equals(Result<TValue>? other) => throw new NotImplementedException();

    public override bool Equals(object? obj) => this.Equals(obj as Result<TValue>);

    public override int GetHashCode() => throw new NotImplementedException();

    /// <summary>
    /// Gets the value of the current instance.
    /// </summary>
    /// <returns>The value of the current instance.</returns>
    public TValue GetValue()
        => this.Value;

    /// <summary>
    /// Converts the current Result object to an asynchronous Task.
    /// </summary>
    public Task<Result<TValue>> ToAsync()
        => Task.FromResult(this);

    /// <summary> Converts a Result<TValue> to a Result. </summary>
    public Result ToResult(in Result<TValue> result) =>
        result;

    public Result<TNewValue?> WithValue<TNewValue>(TNewValue? newValue) =>
        Result<TNewValue?>.New(newValue, this.Succeed, this.Status, this.Message, this.Errors, this.ExtraData);

    /// <summary>
    /// Creates a new instance of the <see cref="Result{TValue}"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value to set.</param>
    /// <returns>
    /// A new instance of the <see cref="Result{TValue}"/> class with the specified value.
    /// </returns>
    public Result<TValue> WithValue(in TValue value)
        => this.WithValue(value);
}