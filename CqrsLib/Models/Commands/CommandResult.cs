namespace Library.Cqrs;

public class CommandResult
{
    public CommandResult(bool isSucceed) => this.IsSucceed = isSucceed;

    public bool IsSucceed { get; }
    public static CommandResult<Nothing> Empty => new(true, Nothing.Instance);
}
