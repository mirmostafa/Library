using System.Collections.Immutable;
using System.Diagnostics;

using Library.Validations;

namespace Library.Results;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract class ResultBase(
    in bool? succeed = null,
    in object? status = null,
    in string? message = null,
    in IEnumerable<(object Id, object Error)>? errors = null,
    in IEnumerable<object>? extraData = null, ResultBase? innerResult = null)
{
    private ImmutableArray<(object Id, object Error)>? _errors = errors?.ToImmutableArray();

    private ImmutableArray<object>? _extraData = extraData?.ToImmutableArray();

    protected ResultBase(ResultBase result)
        : this(result.ArgumentNotNull().Succeed, result.Status, result.Message, result.Errors, result.ExtraData, result.InnerResult)
    {
    }

    [NotNull]
    public ImmutableArray<(object Id, object Error)> Errors
    {
        get => this._errors ??= ImmutableArray.Create<(object, object)>();
        init => this._errors = value;
    }

    [NotNull]
    public ImmutableArray<object> ExtraData => this._extraData ??= ImmutableArray.Create<object>();

    public ResultBase? InnerResult { get; init; } = innerResult;

    /// <summary>
    /// Gets a value indicating whether the operation has failed.
    /// </summary>
    public virtual bool IsFailure => !this.IsSucceed;

    /// <summary>
    /// Checks if the operation was successful by checking the Succeed flag, Status and Errors.
    /// </summary>
    public virtual bool IsSucceed => this.Succeed ?? ((this.Status is null or 0 or 200) && (!this.Errors.Any()));

    public string? Message { get; init; } = message;
    public object? Status { get; init; } = status;
    public bool? Succeed { get; init; } = succeed;

    //public static implicit operator bool(ResultBase result) =>
    //    result.NotNull().IsSucceed;

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared.</returns>
    public virtual bool Equals(ResultBase? other) =>
        other is not null && this.Status == other.Status;

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>The hash code for the current instance.</returns>
    public override int GetHashCode() =>
        HashCode.Combine(this.Status, this.Message);

    [return: NotNull]
    public IEnumerable<(object Id, object Error)>? SelectAllErrors() =>
        this.IterateOnAll<IEnumerable<(object, object)>>(x => x.Errors).SelectAll();

    [return: NotNull]
    public IEnumerable<object> SelectAllExtraData() =>
        this.IterateOnAll<IEnumerable<object>>(x => x.ExtraData).SelectAll();

    /// <summary>
    /// Generates a string representation of the result object.
    /// </summary>
    /// <returns>A string representation of the result object.</returns>
    public override string ToString()
    {
        if (!this.Message.IsNullOrEmpty())
        {
            return this.Message;
        }
        if (this.Status?.ToString() is { } status)
        {
            return status;
        }
        else if (this.Errors.FirstOrDefault().Error?.ToString() is { } error)
        {
            return error;
        }
        return $"IsSucceed: {this.IsSucceed}";
    }

    private string GetDebuggerDisplay() =>
        this.ToString();

    private IEnumerable<T> IterateOnAll<T>(Func<ResultBase, T> selector)
    {
        yield return selector(this);
        var innerResult = this.InnerResult;
        while (innerResult != null)
        {
            yield return selector(innerResult);
            innerResult = innerResult.InnerResult;
        }
    }
}