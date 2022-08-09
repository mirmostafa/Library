using System.Diagnostics;

using Library.Exceptions;
using Library.Validations;
using Library.Windows;

namespace Library.Results;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract class ResultBase : IEquatable<ResultBase?>
{
    private bool? _isSucceed;

    protected ResultBase(object? statusCode = null, string? message = null)
        => (this.StatusCode, this.FullMessage) = (statusCode, message);

    protected ResultBase(object? statusCode, NotificationMessage? fullMessage)
        => (this.StatusCode, this.FullMessage) = (statusCode, fullMessage);

    protected ResultBase(object? statusCode, [DisallowNull] IException exception)
        => (this.StatusCode, this.FullMessage) = (statusCode, exception.NotNull().ToFullMessage());

    public List<(object? Id, object Message)> Errors { get; } = new();

    public Dictionary<string, object> Extra { get; } = new();

    public NotificationMessage? FullMessage { get; init; }

    public bool IsFailure => !this.IsSucceed;

    public virtual bool IsSucceed
    {
        get => this._isSucceed ?? ((this.StatusCode?.ToInt() is null or 0 or 200) && (!this.Errors.Any()));
        init => this._isSucceed = value;
    }

    public string? Message => this.FullMessage?.Text;

    public object? StatusCode
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
        other is not null && this.StatusCode == other.StatusCode;

    public override int GetHashCode() =>
        HashCode.Combine(this.StatusCode, this.Message, this.Errors);

    public override string ToString()
    {
        StringBuilder result = new();
        if (!this.Message.IsNullOrEmpty())
        {
            _ = result.AppendLine(this.Message);
        }
        if (this.Message.IsNullOrEmpty() && this.Errors.Count == 1)
        {
            _ = result.AppendLine(this.Errors[0].Message?.ToString() ?? "An error occurred.");
        }
        else
        {
            foreach (var errorMessage in this.Errors.Select(x => x.Message?.ToString()).Compact())
            {
                _ = result.AppendLine($"- {errorMessage}");
            }
        }

        return result.ToString();
    }

    internal void SetIsSucceed(bool? isSucceed) 
        => this._isSucceed = isSucceed;

    private string GetDebuggerDisplay() =>
            this.ToString();
}