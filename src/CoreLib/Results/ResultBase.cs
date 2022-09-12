using System.Diagnostics;

using Library.Exceptions;
using Library.Validations;
using Library.Windows;

namespace Library.Results;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract class ResultBase : IEquatable<ResultBase?>
{
    private NotificationMessage? _fullMessage;
    private bool? _isSucceed;

    protected ResultBase(object? status = null, string? message = null)
        => (this.Status, this.FullMessage) = (status, message);

    protected ResultBase(object? status, NotificationMessage? fullMessage)
        => (this.Status, this.FullMessage) = (status, fullMessage);

    protected ResultBase(object? status, [DisallowNull] IException exception)
        => (this.Status, this.FullMessage) = (status, exception.NotNull().ToFullMessage());

    public List<(object? Id, object Data)> Errors { get; } = new();

    public Dictionary<string, object> Extra { get; } = new();

    public NotificationMessage? FullMessage { get => this._fullMessage; init => this._fullMessage = value; }

    public bool IsFailure => !this.IsSucceed;

    public virtual bool IsSucceed
    {
        get => this._isSucceed ?? ((this.Status is null or 0 or 200) && (!this.Errors.Any()));
        init => this._isSucceed = value;
    }

    public string? Message => this.FullMessage?.Text;

    public object? Status
    {
        get;
        protected set;
    }

    public static bool operator !=(ResultBase? left, ResultBase? right)
        => !(left == right);

    public static bool operator ==(ResultBase? left, ResultBase? right)
        => EqualityComparer<ResultBase>.Default.Equals(left, right);

    public void Deconstruct(out bool isSucceed, out string? message)
                => (isSucceed, message) = (this.IsSucceed, this.Message);

    public override bool Equals(object? obj) =>
        this.Equals(obj as ResultBase);

    public bool Equals(ResultBase? other) =>
        other is not null && this.Status == other.Status;

    public override int GetHashCode() =>
        HashCode.Combine(this.Status, this.Message, this.Errors);

    public override string ToString()
    {
        var result = (this.IsSucceed ? new StringBuilder($"IsSucceed: {this.IsSucceed}") : new StringBuilder()).AppendLine();
        if (!this.Message.IsNullOrEmpty())
        {
            _ = result.AppendLine(this.Message);
        }
        if (this.Message.IsNullOrEmpty() && this.Errors.Count == 1)
        {
            _ = result.AppendLine(this.Errors[0].Data?.ToString() ?? "An error occurred.");
        }
        else
        {
            foreach (var errorMessage in this.Errors.Select(x => x.Data?.ToString()).Compact())
            {
                _ = result.AppendLine($"- {errorMessage}");
            }
        }

        return result.ToString();
    }

    internal static TResult From<TResult>(in ResultBase source, in TResult dest)
        where TResult : ResultBase
    {
        dest.Status = source.Status;
        dest._fullMessage = source.FullMessage;

        dest.Errors.AddRange(source.Errors);
        _ = dest.Extra.AddRange(source.Extra);
        return dest.As<TResult>()!;
    }

    internal void SetIsSucceed(bool? isSucceed)
            => this._isSucceed = isSucceed;

    private string GetDebuggerDisplay()
            => this.ToString();
}