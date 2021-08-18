using Library.Collections;

namespace Library.Progressive
{
    public sealed class ProgressiveEngineAsync
    {
        public UniqueList<IProgressiveActionAsync> Actions { get; } = new UniqueList<IProgressiveActionAsync>();
        public async Task ExecuteAsync()
        {
            var actions = this.Actions.Compact().ToArray();
            if (actions.Any(a => a.Sequence is not null))
            {
                var actionGroups = from a in actions
                                   group a by a.Sequence;
                var orderedActions = from a in actionGroups
                                     orderby a.Key
                                     select a;
                using var tasks = new TaskList();
                foreach (var actionGroup in orderedActions)
                {
                    tasks.Clear();
                    foreach (var action in actionGroup)
                    {
                        if (action?.IsEnabled is true)
                        {
                            tasks.Add(action.Action());
                        }
                    }
                    await tasks.WaitAllAsync();
                }
            }
            else
            {
                foreach (var action in actions)
                {
                    if (action?.IsEnabled is true)
                    {
                        await action.Action();
                    }
                }
            }
        }
    }

    public interface IProgressiveActionAsync
    {
        bool IsEnabled { get; }
        string? Title { get; }
        string? Description { get; }
        int? Sequence { get; }
        Func<Task> Action { get; }
    }
}
