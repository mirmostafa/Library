using System.Collections.Immutable;
using System.Diagnostics;

using Library.DesignPatterns.Markers;
using Library.Validations;

namespace Library.Results;

[DebuggerStepThrough, StackTraceHidden]
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
[Immutable]
public abstract class ResultBase(
    in bool? isSucceed = null,
    in string? message = null,
    in IEnumerable<Exception>? errors = null,
    ResultBase? innerResult = null) : IResult
{
    private readonly bool? _isSucceed = isSucceed;
    private ImmutableArray<Exception>? _errors = errors?.ToImmutableArray();

    protected ResultBase(ResultBase result)
        : this(result.ArgumentNotNull().IsSucceed, result.Message, result.Errors, result.InnerResult)
    {
    }

    [NotNull]
    public ImmutableArray<Exception> Errors
    {
        get => this._errors ??= [];
        init => this._errors = value;
    }

    public Exception? Exception => this.Errors.FirstOrDefault();

    public ResultBase? InnerResult { get; init; } = innerResult;

    /// <summary>
    /// Gets a value indicating whether the operation has failed.
    /// </summary>
    public virtual bool IsFailure => !this.IsSucceed;

    /// <summary>
    /// Checks if the operation was successful by checking the Succeed flag, Status and Errors.
    /// </summary>
    public virtual bool IsSucceed
    {
        get => this._isSucceed ?? !this.Errors.Any();
        init => this._isSucceed = value;
    }

    public string? Message { get; init; } = message;

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared.</returns>
    public virtual bool Equals(ResultBase? other) =>
        other is not null && this.GetHashCode() == other.GetHashCode();

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>The hash code for the current instance.</returns>
    public override int GetHashCode() =>
        HashCode.Combine(this.IsSucceed, this.Message);

    /// <summary>
    /// Generates a string representation of the result object.
    /// </summary>
    /// <returns>A string representation of the result object.</returns>
    public override string ToString() => !this.Message.IsNullOrEmpty()
            ? this.Message
            : this.Errors.FirstOrDefault()?.ToString() is { } error ? error : $"IsSucceed: {this.IsSucceed}";

    private string GetDebuggerDisplay() =>
        this.ToString();
}