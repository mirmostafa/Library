using System.Threading.Tasks;

namespace Mohammad.CqrsInfra.CommandInfra
{
    public class CommandResult<T> : CommandResult
    {
        public T Result { get; }

        public CommandResult(bool status, T result)
            : base(status) => this.Result = result;

        public static Task<CommandResult<T>> SuccessTask(T item) => Task.FromResult(Success(item));
        public static CommandResult<T> Success(T item) => new CommandResult<T>(true, item);
        public new static CommandResult<T> Empty() => new CommandResult<T>(true, default);

        public static async Task<CommandResult<int>> FromDb(Task<int> saveResult)
        {
            var result = await saveResult;
            return new CommandResult<int>(result > 0, result);
        }
    }
}