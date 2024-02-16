using Library.Exceptions;
using Library.Results;
using Library.Validations;

namespace Library.Helpers;

public static class TaskHelper
{
    /// <summary>
    /// Invokes an action asynchronously.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    /// <param name="scheduler">The scheduler to use for the task.</param>
    /// <param name="token">The cancellation token to use for the task.</param>
    public static async void InvokeAsync(this Action action, TaskScheduler? scheduler = null, CancellationToken token = default)
    {
        Check.MustBeArgumentNotNull(action);

        if (scheduler != null)
        {
            await Task.Factory.StartNew(action, token, TaskCreationOptions.None, scheduler);
        }
        else
        {
            await Task.Factory.StartNew(action, token, TaskCreationOptions.None, TaskScheduler.Default);
        }
    }

    /// <summary>
    /// Creates a task that will complete when all of the <see cref="Task"/> objects in an array
    /// have completed.
    /// </summary>
    /// <remarks>Returns all the exceptions occurred, if any</remarks>
    /// <typeparam name="TResult">The type of the completed task.</typeparam>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <returns>A task that represents the completion of all of the supplied tasks.</returns>
    public static async Task<IEnumerable<TResult>> WhenAllAsync<TResult>(IEnumerable<Task<TResult>> tasks)
        => await WhenAllAsync(tasks.ToArray());

    /// <summary>
    /// Creates a task that will complete when all of the <see cref="Task"/> objects in an array
    /// have completed.
    /// </summary>
    /// <remarks>Returns all the exceptions occurred, if any</remarks>
    /// <typeparam name="TResult">The type of the completed task.</typeparam>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <returns>A task that represents the completion of all of the supplied tasks.</returns>
    public static async Task<IEnumerable<TResult>> WhenAllAsync<TResult>(params Task<TResult>[] tasks)
    {
        var allTasks = Task.WhenAll(tasks);
        try
        {
            return await allTasks;
        }
        catch (Exception)
        {
            return Enumerable.Empty<TResult>();
        }
    }

    /// <summary>
    /// Creates a task that will complete when all of the <see cref="Task"/> objects in an array
    /// have completed.
    /// </summary>
    /// <remarks>Returns all the exceptions occurred, if any</remarks>
    /// <typeparam name="TResult">The type of the completed task.</typeparam>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <returns>A task that represents the completion of all of the supplied tasks.</returns>
    public static async Task WhenAllAsync(IEnumerable<Task> tasks)
        => await WhenAllAsync(tasks.ToArray());

    /// <summary>
    /// Creates a task that will complete when all of the <see cref="Task"/> objects in an array
    /// have completed.
    /// </summary>
    /// <remarks>Returns all the exceptions occurred, if any</remarks>
    /// <typeparam name="TResult">The type of the completed task.</typeparam>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <returns>A task that represents the completion of all of the supplied tasks.</returns>
    public static async Task WhenAllAsync(params Task[] tasks)
    {
        var allTasks = Task.WhenAll(tasks);
        try
        {
            await allTasks;
        }
        catch (Exception)
        {
            //ignore
        }
    }

    public static async Task<Result> RunAllAsync(this IEnumerable<Func<Task<Result>>> funcs, CancellationToken token)
    {
        Check.MustBeArgumentNotNull(funcs);

        var result = Result.Success;
        foreach (var func in funcs)
        {
            if (token.IsCancellationRequested)
            {
                result = Result.CreateFailure<OperationCancelledException>();
                break;
            }

            var current = await func();
            if (current.IsFailure)
            {
                result = current;
                break;
            }
        }
        return result;
    }
    
    public static async Task<Result> RunAllAsync<TState>(this IEnumerable<Func<TState, Task<Result>>> funcs, TState initiaState, CancellationToken token = default)
    {
        Check.MustBeArgumentNotNull(funcs);

        var result = Result.Success;
        foreach (var func in funcs)
        {
            if (token.IsCancellationRequested)
            {
                result = Result.CreateFailure<OperationCancelledException>();
                break;
            }

            var current = await func(initiaState);
            if (current.IsFailure)
            {
                result = current;
                break;
            }
        }
        return result;
    }
}