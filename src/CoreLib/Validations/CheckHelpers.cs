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
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static string ArgumentNotNull([NotNull][AllowNull] string obj, [CallerArgumentExpression("obj")] string argName = null)
            => obj.NotValid(x => x.IsNullOrEmpty(), () => new ArgumentNullException(argName));

    [return: NotNull]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static T ArgumentNotNull<T>([NotNull][AllowNull] T obj, [CallerArgumentExpression("obj")] string argName = null)
        => obj.NotValid(x => x is null, () => new ArgumentNullException(argName));

    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static TEnumerable HasAny<TEnumerable>([NotNull] TEnumerable obj, [DisallowNull] Func<Exception> getException)
            where TEnumerable : IEnumerable
    {
        if (!NotNull(obj).Any())
        {
            Throw(getException);
        }
        return obj;
    }

    [return: NotNull]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static string NotNull([AllowNull][NotNull] string obj,/*[InvokerParameterName]*/ [CallerArgumentExpression("obj")] string argName = null)
        => obj.NotValid(x => x.IsNullOrEmpty(), () => new NullValueValidationException(argName!));

    [return: NotNull]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static T NotNull<T>([NotNull] T obj, [DisallowNull] Func<Exception> getException)
        => obj.NotValid(x => x is null, getException)!;

    [return: NotNull]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static T NotNull<T, TException>([AllowNull][NotNull] T obj) where TException : Exception, new()
        => obj.NotValid(x => x is null, () => new TException());

    [return: NotNull]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static T NotNull<T>([NotNull] T obj, [CallerArgumentExpression(nameof(obj))] string argName = null)
        => obj.NotValid(x => x is null, () => new NullValueValidationException(argName!))!;

    [return: NotNull]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static T NotNull<T>([NotNull] T @this, [NotNull] object obj, [CallerArgumentExpression(nameof(obj))] string argName = null)
        => @this.NotValid(_ => obj is null, () => new NullValueValidationException(argName!))!;

    [return: NotNull]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static T NotNull<T>([NotNull] T obj, Func<string> getMessage)
        => obj.NotValid(x => x is null, () => new NullValueValidationException(getMessage(), null))!;

    [return: NotNull]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static async Task<TResult> NotNullAsync<TResult>([NotNull] this Task<TResult> task, Func<Exception> getException)
    {
        var result = await task;
        return result.NotNull(getException);
    }

    [return: NotNull]
    [Obsolete("Use `ValidationResultSet<TValue>` instead.", true)]
    public static async Task<TResult> NotNullAsync<TResult>([NotNull] this Func<Task<TResult>> asyncFunc, Func<Exception> getException)
    {
        var result = await asyncFunc();
        return result.NotNull(getException);
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