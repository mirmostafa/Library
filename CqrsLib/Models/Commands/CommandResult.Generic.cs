namespace Library.Cqrs.Models.Commands;

public class CommandResult<T> : CommandResult
{
    public CommandResult(bool status, T result)
        : base(status)
        => this.Result = result;

    public T Result { get; }

    public static new CommandResult<T> Empty() => new(true, default);

    public static async Task<CommandResult<int>> FromDb(Task<int> saveResult)
    {
        var result = await saveResult;
        return new(result > 0, result);
    }

    public static CommandResult<T> Success(T item)
        => new(true, item);

    public static Task<CommandResult<T>> SuccessTask(T item)
        => Task.FromResult(Success(item));
}