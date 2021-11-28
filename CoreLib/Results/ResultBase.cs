namespace Library.Results;

public abstract class ResultBase : IEquatable<ResultBase?>
{
    public int? StatusCode { get; }
    public string? Message { get; set; }
    public virtual bool IsSucceed => this.StatusCode?.ToInt() is 0;
    public bool Failure => !this.IsSucceed;
    public Dictionary<string, object> Extra { get; } = new();
    public List<(object Id, object Message)> Errors { get; } = new();

    protected ResultBase(int? statusCode = null, string? message = null)
        => (this.StatusCode, this.Message) = (statusCode, message);

    public void Deconstruct(out int? statusCode, out string? message)
        => (statusCode, message) = (this.StatusCode, this.Message);
    public override bool Equals(object? obj) => this.Equals(obj as ResultBase);
    public bool Equals(ResultBase? other) => other != null && this.StatusCode == other.StatusCode;
    public override int GetHashCode() => HashCode.Combine(this.StatusCode);

    public static bool operator ==(ResultBase? left, ResultBase? right)
        => EqualityComparer<ResultBase>.Default.Equals(left, right);
    public static bool operator !=(ResultBase? left, ResultBase? right)
        => !(left == right);
}