namespace Library.Helpers;

public static class TaskHelper
{
    public static async void InvokeAsync(this Action? action, TaskScheduler? scheduler = null, CancellationToken token = default)
    {
        if (action == null)
        {
            return;
        }

        if (scheduler != null)
        {
            await Task.Factory.StartNew(action, token, TaskCreationOptions.None, scheduler);
        }
        else
        {
            await Task.Factory.StartNew(action, token);
        }
    }

    /// <summary>
    /// Creates a task that will complete when all of the <see cref="System.Threading.Tasks.Task"/>
    /// objects in an array have completed.
    /// </summary>
    /// <remarks>Returns all the excpetions occurred, if any</remarks>
    /// <typeparam name="TResult">The type of the completed task.</typeparam>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <returns>A task that represents the completion of all of the supplied tasks.</returns>
    public static async Task<IEnumerable<TResult>> WhenAllAsync<TResult>(IEnumerable<Task<TResult>> tasks)
        => await WhenAllAsync(tasks.ToArray());

    /// <summary>
    /// Creates a task that will complete when all of the <see cref="System.Threading.Tasks.Task"/>
    /// objects in an array have completed.
    /// </summary>
    /// <remarks>Returns all the excpetions occurred, if any</remarks>
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
            //ignore
        }
        throw allTasks.Exception ?? throw new Exception("This can't possibly happen");
    }

    /// <summary>
    /// Creates a task that will complete when all of the <see cref="System.Threading.Tasks.Task"/>
    /// objects in an array have completed.
    /// </summary>
    /// <remarks>Returns all the excpetions occurred, if any</remarks>
    /// <typeparam name="TResult">The type of the completed task.</typeparam>
    /// <param name="tasks">The tasks to wait on for completion.</param>
    /// <returns>A task that represents the completion of all of the supplied tasks.</returns>
    public static async Task WhenAllAsync(IEnumerable<Task> tasks)
        => await WhenAllAsync(tasks.ToArray());

    /// <summary>
    /// Creates a task that will complete when all of the <see cref="System.Threading.Tasks.Task"/>
    /// objects in an array have completed.
    /// </summary>
    /// <remarks>Returns all the excpetions occurred, if any</remarks>
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
        throw allTasks.Exception ?? throw new Exception("This can't possibly happen");
    }
}