﻿namespace Library.Coding;

[Obsolete("Subject to remove", true)]
public static class FluentCondition
{
    #region Type 1

    [Obsolete("Subject to remove", true)]
    public static IfCondition Else(this IfCondition condition, Action action)
    {
        if (!condition.Condition)
        {
            action?.Invoke();
        }
        return condition;
    }

    [Obsolete("Subject to remove", true)]
    public static IfCondition If(this Fluency<bool> condition)
        => new(condition);

    [Obsolete("Subject to remove", true)]
    public static IfCondition Then(this IfCondition condition, Action action)
    {
        if (condition.Condition)
        {
            action?.Invoke();
        }
        return condition;
    }

    #endregion Type 1

    #region Type 2

    [Obsolete("Subject to remove", true)]
    public static IfCondition<T> Else<T>(this IfCondition<T> condition, Func<T> action)
    {
        if (!condition.Condition && action is not null)
        {
            condition.Result = action();
        }
        return condition;
    }

    [Obsolete("Subject to remove", true)]
    public static IfCondition<T> If<T>(this Fluency<bool> condition)
        => new(condition);

    [Obsolete("Subject to remove", true)]
    public static IfCondition<T> Then<T>(this IfCondition<T> condition, Func<T> action)
    {
        if (condition.Condition && action is not null)
        {
            condition.Result = action();
        }
        return condition;
    }

    #endregion Type 2

    #region Type 3

    [Obsolete("Subject to remove", true)]
    public static IfCondition If(this Fluency<Func<bool>> condition)
        => new(condition.GetValue()());

    [Obsolete("Subject to remove", true)]
    public static IfCondition<T> If<T>(this Fluency<Func<bool>> condition)
        => new(condition.GetValue()());

    #endregion Type 3
}

[Obsolete("Subject to remove", true)]
public record struct IfCondition(bool Condition);
[Obsolete("Subject to remove", true)]
public record struct IfCondition<T>(bool Condition)
{
    public T Result { get; internal set; }

    public static implicit operator T(IfCondition<T> condition)
        => condition.Result;
}