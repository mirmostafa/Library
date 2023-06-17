namespace Library.Interfaces;

public interface IAsyncCommand
{
    Task ExecuteAsync();
}

public interface IAsyncCommand<TResult>
{
    Task<TResult> ExecuteAsync();
}

public interface ICommand
{
    void Execute();
}

public interface ICommand<out TResult>
{
    TResult Execute();
}