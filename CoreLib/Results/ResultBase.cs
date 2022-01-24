namespace Library.Results;

public abstract class ResultBase : IEquatable<ResultBase?>
{
    private bool? _isSucceed;
    private string? _message;

    public int? StatusCode { get; }
    public string? Message { get => this._message ?? string.Join(Environment.NewLine, this.Errors.Select(x => x.Message)); set => this._message = value; }
    public virtual bool IsSucceed { get => this._isSucceed ?? (this.StatusCode?.ToInt() is null or 0 or 200) && (!this.Errors.Any()); init => this._isSucceed = value; }
    public bool IsFailure => !this.IsSucceed;
    public Dictionary<string, object> Extra { get; } = new();
    public List<(object? Id, object Message)> Errors { get; } = new();

    protected ResultBase(int? statusCode = null, string? message = null)
        => (this.StatusCode, this.Message) = (statusCode, message);

    public void Deconstruct(out int? statusCode, out string? message)
        => (statusCode, message) = (this.StatusCode, this.Message);

    public override bool Equals(object? obj) => this.Equals(obj as ResultBase);
    public bool Equals(ResultBase? other) => other is not null && this.StatusCode == other.StatusCode;
    public override int GetHashCode() => HashCode.Combine(this.StatusCode);

    public static bool operator ==(ResultBase? left, ResultBase? right)
        => EqualityComparer<ResultBase>.Default.Equals(left, right);
    public static bool operator !=(ResultBase? left, ResultBase? right)
        => !(left == right);
}