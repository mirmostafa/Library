//using System.Diagnostics;

//using Library.Validations;

//namespace Library.Coding;

//[DebuggerStepThrough]
//[StackTraceHidden]
//public struct IfStatememt
//{
//    public IfStatememt(Func<bool> condition)
//        : this()
//        => this.Condition = condition ?? throw new ArgumentNullException(nameof(condition));

//    public Func<bool> Condition { get; set; }
//    public Action ThenAction { get; set; }
//    public Action ElseAction { get; set; }
//}
//[DebuggerStepThrough]
//[StackTraceHidden]
//public struct IfStatememt<TResult>
//{
//    public IfStatememt(Func<bool> condition)
//        : this()
//        => this.If = condition ?? throw new ArgumentNullException(nameof(condition));

//    public Func<bool> If { get; set; }
//    public Func<TResult> Then { get; set; }
//    public Func<TResult> Else { get; set; }
//}

//[DebuggerStepThrough]
//[StackTraceHidden]
//public static class IfStatementExtensions
//{
//    public static IfStatememt If(this Func<bool> @condition)
//        => new(condition);
//    public static IfStatememt If(this bool @x)
//        => new(() => x);

//    public static IfStatememt Then(this IfStatememt ifStatememt, Action @then)
//        => ifStatememt.Fluent(ifStatememt.ThenAction = then);

//    public static IfStatememt Else(this IfStatememt ifStatememt, Action @else)
//        => ifStatememt.Fluent(ifStatememt.ElseAction = @else);

//    public static void Build(this IfStatememt ifStatememt)
//        => ifStatememt.Condition.If(ifStatememt.ThenAction, ifStatememt.ElseAction);

//    public static IfStatememt<TResult> If<TResult>(this Func<bool> @condition)
//        => new(condition);

//    public static IfStatememt<TResult> If<TResult>(this bool @x)
//        => new(() => x);

//    public static IfStatememt<TResult> Then<TResult>(this IfStatememt<TResult> ifStatememt, Func<TResult> @then)
//    {
//        ifStatememt.Then = then;
//        return ifStatememt;
//    }

//    public static IfStatememt<TResult> Else<TResult>(this IfStatememt<TResult> ifStatememt, Func<TResult> @else)
//    {
//        ifStatememt.Else = @else;
//        return ifStatememt;
//    }

//    public static TResult Build<TResult>(this IfStatememt<TResult> ifStatememt) =>
//        ifStatememt.If.NotNull(nameof(ifStatememt.If))()
//                   ? ifStatememt.Then.NotNull(nameof(ifStatememt.Then))()
//                   : ifStatememt.Else.NotNull(nameof(ifStatememt.Else))();
//}