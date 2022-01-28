using System.Diagnostics;

namespace Library.Results;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract class ResultBase : IEquatable<ResultBase?>
{
    private bool? _isSucceed;

    public object? StatusCode
    {
        get;
        protected set;
    }
    public string? Message { get; set; }
    public virtual bool IsSucceed
    {
        get => this._isSucceed ?? (this.StatusCode?.ToInt() is null or 0 or 200) && (!this.Errors.Any());
        init => this._isSucceed = value;
    }
    public bool IsFailure => !this.IsSucceed;
    public Dictionary<string, object> Extra { get; } = new();
    public List<(object? Id, object Message)> Errors { get; } = new();

    protected ResultBase(object? statusCode = null, string? message = null)
        => (this.StatusCode, this.Message) = (statusCode, message);

    public void Deconstruct(out object? statusCode, out string? message)
        => (statusCode, message) = (this.StatusCode, this.Message);

    public override bool Equals(object? obj) => this.Equals(obj as ResultBase);
    public bool Equals(ResultBase? other) => other is not null && this.StatusCode == other.StatusCode;
    public override int GetHashCode() => HashCode.Combine(this.StatusCode);

    public static bool operator ==(ResultBase? left, ResultBase? right)
        => EqualityComparer<ResultBase>.Default.Equals(left, right);
    public static bool operator !=(ResultBase? left, ResultBase? right)
        => !(left == right);

    private string GetDebuggerDisplay() =>
        this.ToString();
    public override string ToString()
    {
        StringBuilder result = new();
        if (!this.Message.IsNullOrEmpty())
        {
            result.AppendLine(this.Message);
        }
        //string.Join(Environment.NewLine, this.Errors.Select(x => x.Message))
        foreach (var errorMessage in this.Errors.Select(x => x.Message?.ToString()).Compact())
        {
            result.AppendLine(errorMessage);
        }
        return result.ToString();
    }
}