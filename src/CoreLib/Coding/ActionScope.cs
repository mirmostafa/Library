using Library.DesignPatterns.Markers;
using Library.Logging;
using Library.Results;
using Library.Validations;

namespace Library.Coding;

[Fluent]
[Immutable]
public sealed class ActionScope
{
    public const string BEGINING_MESSAGE = null;
    public const string ENDING_MESSAGE = "Ready";
    private readonly ILogger _logger;

    public ActionScope(ILogger logger)
        => this._logger = logger.ArgumentNotNull();

    public ActionScope(ILoggerContainer loggerContainer)
        => this._logger = loggerContainer.ArgumentNotNull().Logger;

    public static ActionScope Begin(ILogger logger, string message = BEGINING_MESSAGE)
        => new ActionScope(logger).Begin(message);

    public static ActionScope Begin(ILoggerContainer loggerContainer, string message = BEGINING_MESSAGE)
        => new ActionScope(loggerContainer).Begin(message);

    public ActionScope Begin(string message = BEGINING_MESSAGE)
        => this.Prompt(message);

    public ActionScope End(string message = ENDING_MESSAGE)
        => this.Prompt(message);

    public ActionScope Prompt(string message)
    {
        this._logger?.Debug(message);
        return this;
    }
}

public static class ActionScopeExtensions
{
    public static ActionScope ActionScopeBegin(this ILoggerContainer loggerContainer, string beginMessage = ActionScope.BEGINING_MESSAGE)
        => ActionScope.Begin(loggerContainer, beginMessage);

    public static void ActionScopeEnd(this ILogger logger, string endMessage = ActionScope.ENDING_MESSAGE)
        => logger.Debug(ActionScope.ENDING_MESSAGE);

    public static void ActionScopeEnd(this ILoggerContainer logger, string endMessage = ActionScope.ENDING_MESSAGE)
        => logger.Debug(ActionScope.ENDING_MESSAGE);

    public static Result? ActionScopeEnd(this ILoggerContainer logger, Result? result, string endMessage = ActionScope.ENDING_MESSAGE)
    {
        logger.Debug(result?.Message ?? endMessage);
        return result;
    }

    public static Result<TResult>? ActionScopeEnd<TResult>(this ILoggerContainer logger, Result<TResult>? result, string endMessage = ActionScope.ENDING_MESSAGE)
    {
        logger.Debug(result?.Message ?? endMessage);
        return result;
    }

    public static Result ActionScopeRun(this ILogger logger, Func<Result> action, string beginMessage = ActionScope.BEGINING_MESSAGE, string endMessage = ActionScope.ENDING_MESSAGE)
            => ActionScope
                .Begin(logger, beginMessage)
                .Do(action)
                .End();

    public static void ActionScopeRun(this ILogger logger, Action action, string beginMessage = ActionScope.BEGINING_MESSAGE, string endMessage = ActionScope.ENDING_MESSAGE)
    {
        var scope = ActionScope.Begin(logger, beginMessage);
        action();
        _ = scope.End(endMessage);
    }

    public static TResult ActionScopeRun<TResult>(this ILogger logger, Func<TResult> action, string beginMessage = ActionScope.BEGINING_MESSAGE, string endMessage = ActionScope.ENDING_MESSAGE)
    {
        var scope = ActionScope.Begin(logger, beginMessage);
        var result = action();
        _ = scope.End(endMessage);
        return result;
    }

    public static TResult ActionScopeRun<TResult>(this ILoggerContainer loggerContainer, Func<TResult> action, string beginMessage = ActionScope.BEGINING_MESSAGE, string endMessage = ActionScope.ENDING_MESSAGE)
    {
        var scope = ActionScope.Begin(loggerContainer, beginMessage);
        var result = action();
        _ = scope.End(endMessage);
        return result;
    }

    public static async Task ActionScopeRunAsync(this ILogger logger, Func<Task> action, string beginMessage = ActionScope.BEGINING_MESSAGE, string endMessage = ActionScope.ENDING_MESSAGE)
    {
        var scope = ActionScope.Begin(logger, beginMessage);
        await action();
        _ = scope.End(endMessage);
    }

    public static async Task ActionScopeRunAsync(this ILoggerContainer loggerContainer, Func<Task> action, string beginMessage = ActionScope.BEGINING_MESSAGE, string endMessage = ActionScope.ENDING_MESSAGE)
    {
        var scope = ActionScope.Begin(loggerContainer, beginMessage);
        await action();
        _ = scope.End(endMessage);
    }

    public static (TResult Result, ActionScope Scope) Do<TResult>(this ActionScope scope, Func<TResult> action, string beginMessage = ActionScope.BEGINING_MESSAGE, string endMessage = ActionScope.ENDING_MESSAGE)
    {
        _ = scope.Begin(beginMessage);
        var result = action();
        return (result, scope);
    }

    public static Result End(this (Result Result, ActionScope Scope) result, string endMessage = ActionScope.ENDING_MESSAGE)
        => result.Scope.End(result.Result, endMessage);

    public static Result End(this ActionScope scope, Result result, string endMessage = ActionScope.ENDING_MESSAGE)
    {
        _ = scope.End(result.Message ?? endMessage);
        return result;
    }
}