﻿using Library.Validations;

namespace Library.Coding;

public struct IfStatememt
{
    public IfStatememt(Func<bool> @if)
        : this()
        => this.If = @if ?? throw new ArgumentNullException(nameof(@if));

    public Func<bool> If { get; set; }
    public Action ThenAction { get; set; }
    public Action ElseAction { get; set; }
}
public struct IfStatememt<TResult>
{
    public IfStatememt(Func<bool> @if)
        : this()
        => this.If = @if ?? throw new ArgumentNullException(nameof(@if));

    public Func<bool> If { get; set; }
    public Func<TResult> Then { get; set; }
    public Func<TResult> Else { get; set; }
}

public static class IfStatementExtensions
{
    public static IfStatememt If(this Func<bool> @condition)
        => new(condition);
    public static IfStatememt If(this bool @x)
        => new(() => x);

    public static IfStatememt Then(this IfStatememt ifStatememt, Action @then)
        => ifStatememt.Fluent(ifStatememt.ThenAction = then);

    public static IfStatememt Else(this IfStatememt ifStatememt, Action @else)
        => ifStatememt.Fluent(ifStatememt.ElseAction = @else);

    public static void Build(this IfStatememt ifStatememt)
        => 0.If(ifStatememt.If, ifStatememt.ThenAction, ifStatememt.ElseAction);


    public static IfStatememt<TResult> If<TResult>(this Func<bool> @condition)
        => new(condition);

    public static IfStatememt<TResult> If<TResult>(this bool @x)
        => new(() => x);

    public static IfStatememt<TResult> Then<TResult>(this IfStatememt<TResult> ifStatememt, Func<TResult> @then)
        => ifStatememt.Fluent(ifStatememt.Then = then);

    public static IfStatememt<TResult> Else<TResult>(this IfStatememt<TResult> ifStatememt, Func<TResult> @else)
        => ifStatememt.Fluent(ifStatememt.Else = @else);

    public static TResult Build<TResult>(this IfStatememt<TResult> ifStatememt) => ifStatememt.If.NotNull(nameof(ifStatememt.If))()
                   ? ifStatememt.Then.NotNull(nameof(ifStatememt.Then))()
                   : ifStatememt.Else.NotNull(nameof(ifStatememt.Else))();
}