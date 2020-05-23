namespace Mohammad.CqrsInfra.CommandInfra
{
    public class CommandResult
    {
        public bool Status { get; }
        public static CommandResult<Nothing> Empty => new CommandResult<Nothing>(true, Nothing.Instance);
        public CommandResult(bool success) => this.Status = success;
    }
}