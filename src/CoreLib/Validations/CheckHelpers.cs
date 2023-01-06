#nullable disable

using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using Library.Exceptions.Validations;

namespace Library.Validations;

[DebuggerStepThrough]
public static class CheckHelpers
{
    [return: NotNull]
    public static string ArgumentNotNull([NotNull][AllowNull] this string obj, [CallerArgumentExpression("obj")] string argName = null)
            => obj.NotValid(x => x.IsNullOrEmpty(), () => new ArgumentNullException(argName));

    [return: NotNull]
    public static T ArgumentNotNull<T>([NotNull][AllowNull] this T obj, [CallerArgumentExpression("obj")] string argName = null)
        => obj.NotValid(x => x is null, () => new ArgumentNullException(argName));

    public static TEnumerable HasAny<TEnumerable>([NotNull] this TEnumerable obj, [DisallowNull] Func<Exception> getException)
            where TEnumerable : IEnumerable
    {
        if (!NotNull(obj).Any())
        {
            Throw(getException);
        }
        return obj;
    }

    [return: NotNull]
    public static string NotNull([AllowNull][NotNull] this string obj,/*[InvokerParameterName]*/ [CallerArgumentExpression("obj")] string argName = null)
        => obj.NotValid(x => x.IsNullOrEmpty(), () => new NullValueValidationException(argName!));

    [return: NotNull]
    public static T NotNull<T>([NotNull] this T obj, [DisallowNull] Func<Exception> getException)
        => obj.NotValid(x => x is null, getException)!;

    [return: NotNull]
    public static T NotNull<T, TException>([AllowNull][NotNull] this T obj) where TException : Exception, new()
        => obj.NotValid(x => x is null, () => new TException());

    [return: NotNull]
    public static T NotNull<T>([NotNull] this T obj, [CallerArgumentExpression("obj")] string argName = null)
        => obj.NotValid(x => x is null, () => new NullValueValidationException(argName!))!;

    [return: NotNull]
    public static T NotNull<T>([NotNull] this T @this, [NotNull] object obj, [CallerArgumentExpression("obj")] string argName = null)
        => @this.NotValid(_ => obj is null, () => new NullValueValidationException(argName!))!;

    [return: NotNull]
    public static T NotNull<T>([NotNull] this T obj, Func<string> getMessage)
        => obj.NotValid(x => x is null, () => new NullValueValidationException(getMessage(), null))!;

    [return: NotNull]
    public static async Task<TResult> NotNullAsync<TResult>([NotNull] this Task<TResult> task, Func<Exception> getException)
    {
        var result = await task;
        return NotNull(result, getException);
    }

    [return: NotNull]
    public static async Task<TResult> NotNullAsync<TResult>([NotNull] this Func<Task<TResult>> asyncFunc, Func<Exception> getException)
    {
        var result = await asyncFunc();
        return NotNull(result, getException);
    }

    public static T ThrowIfDisposed<T>(this T @this, [DoesNotReturnIf(true)] bool disposed)
        where T : IDisposable
    {
        Check.If(!disposed, () => new ObjectDisposedException(@this?.GetType().Name));
        return @this;
    }

    internal static T NotValid<T>([AllowNull] this T obj, [DisallowNull] in Func<T, bool> validate, [DisallowNull] in Func<Exception> getException)
        => !(validate?.Invoke(obj) ?? false) ? obj : getException is null ? obj : Throw<T>(getException);
}