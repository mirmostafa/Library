using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

using Library.Exceptions.Validations;

namespace Library.Validations;

[DebuggerStepThrough]
[StackTraceHidden]
public sealed class Check
{
    private static Check? _that;

    private Check()
    { }

    /// <summary>
    /// To support extension methods.
    /// </summary>
    /// <value>
    /// The that.
    /// </value>
    public static Check That => _that ??= new();

    [return: NotNull]
    public static void IfAny([NotNull][AllowNull] IEnumerable? obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        IfNotValid(obj, _ => !obj?.Any() ?? false, () => new NoItemValidationException(argName!));

    [return: NotNull]
    public static void IfArgumentBiggerThan(in int arg, in int min, [CallerArgumentExpression("arg")] in string? argName = null)
    {
        if (arg <= min)
        {
            Throw(new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The argument {0}(:'{1}') cannot be lass than {2}", argName, arg, min)));
        }
    }

    [return: NotNull]
    public static void IfArgumentNotNull([NotNull][AllowNull] in string? obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        MustBeArgumentNotNull(!obj.IsNullOrEmpty(), argName!);

    [return: NotNull]
    public static void IfArgumentNotNull([NotNull][AllowNull] object? obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        MustBeArgumentNotNull(obj is not null, argName!);

    public static void IfHasAny([NotNull][AllowNull] IEnumerable? obj, [DisallowNull] Func<Exception> getException)
    {
        if (!obj.NotNull(getException).Any())
        {
            Throw(getException);
        }
    }

    public static void IfHasAny<TEnumerable>([NotNull][AllowNull] IEnumerable? obj, [DisallowNull] Func<Exception> getException) =>
        _ = obj.HasAny(getException);

    public static void IfIs<T>([NotNull][AllowNull] object? obj, [CallerArgumentExpression("obj")] string? argName = null) =>
                        _ = obj.NotValid(x => x is not T, () => new TypeMismatchValidationException(argName!));

    /// <summary>
    /// Makes sure the specified argument is not null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="argName"></param>
    /// <exception cref="ArgumentNullException"/>
    public static void IfNotNull<T>([NotNull][AllowNull] T? obj, [DisallowNull] Func<Exception> getException) =>
        _ = obj.NotValid(x => x is null, getException);

    public static void IfNotNull<T>([NotNull][AllowNull] T? obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        MustBeNotNull(obj is not null, argName!);

    public static void IfNotNull<T>([NotNull][AllowNull] T? obj, Func<string> getMessage) =>
        obj.NotValid(x => x is null, () => new ValidationException(getMessage()));

    public static void IfNotNull<T>([NotNull][AllowNull] T? @this, string obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        _ = @this.NotValid(_ => obj.IsNullOrEmpty(), () => new NullValueValidationException(argName!))!;

    public static void IfNotValid<T>([AllowNull] T? obj, [DisallowNull] in Func<T, bool> validate, in Func<Exception> getException) =>
                    obj.NotValid(validate, getException);

    public static void MustBe([DoesNotReturnIf(false)] bool ok, in Func<Exception> getExceptionIfNot)
    {
        if (!ok)
        {
            Throw(getExceptionIfNot);
        }
    }

    public static void MustBe<TValidationException>([DoesNotReturnIf(false)] bool ok)
        where TValidationException : Exception, new()
        => MustBe(ok, () => new TValidationException());

    public static void MustBe([DoesNotReturnIf(false)] bool required)
        => MustBe<RequiredValidationException>(required);

    public static void MustBe([DoesNotReturnIf(false)] bool required, Func<string> getMessage)
        => MustBe(required, () => new ValidationException(getMessage.NotNull()()));

    public static void MustBeArgumentNotNull([DoesNotReturnIf(false)] bool isNotNull, [DisallowNull] string argName)
        => MustBe(isNotNull, () => new ArgumentNullException(argName));

    public static void MustBeNotNull([DoesNotReturnIf(false)] bool isNotNull, string argName)
        => MustBe(isNotNull, () => new NullValueValidationException(argName));
}

[DebuggerStepThrough]
public static class CheckHelpers
{
    [return: NotNull]
    public static string ArgumentNotNull([NotNull][AllowNull] this string obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        obj.NotValid(x => x.IsNullOrEmpty(), () => new ArgumentNullException(argName));

    [return: NotNull]
    public static T ArgumentNotNull<T>([NotNull][AllowNull] this T obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        obj.NotValid(x => x is null, () => new ArgumentNullException(argName));

    public static TEnumerable HasAny<TEnumerable>([NotNull] this TEnumerable obj, [DisallowNull] Func<Exception> getException)
        where TEnumerable : IEnumerable
    {
        if (!obj.NotNull().Any())
        {
            Throw(getException);
        }
        return obj;
    }

    public static T Is<T>([NotNull] this object obj, [CallerArgumentExpression("obj")] string? argName = null) =>
                obj.NotValid(x => x is not T, () => new TypeMismatchValidationException(argName!)).To<T>();

    [return: NotNull]
    public static string NotNull([NotNull] this string obj, [CallerArgumentExpression("obj")] string? argName = null) =>
            obj ?? throw new NullValueValidationException(argName ?? "obj");

    [return: NotNull]
    public static T NotNull<T>([NotNull] this T obj, [DisallowNull] Func<Exception> getException) =>
        obj.NotValid(x => x is null, getException)!;

    [return: NotNull]
    public static T NotNull<T, TException>([NotNull] this T obj)
        where TException : Exception, new() =>
        obj.NotValid(x => x is null, () => new TException());

    [return: NotNull]
    public static T NotNull<T>([NotNull] this T obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        obj.NotValid(x => x is null, () => new NullValueValidationException(argName!))!;

    [return: NotNull]
    public static T NotNull<T>([NotNull] this T @this, [NotNull] object? obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        @this.NotValid(_ => obj is null, () => new NullValueValidationException(argName!))!;

    [return: NotNull]
    public static T NotNull<T>([NotNull] this T @this, string obj, [CallerArgumentExpression("obj")] string? argName = null)
        => @this.NotValid(_ => obj.IsNullOrEmpty(), () => new NullValueValidationException(argName!))!;

    [return: NotNull]
    public static T NotNull<T>([NotNull] this T obj, Func<string> getMessage)
        => obj.NotValid(x => x is null, () => new NullValueValidationException(getMessage(), null))!;

    [return: NotNull]
    public static async Task<T> NotNullAsync<T>([DisallowNull] this Task<T> task, [CallerArgumentExpression("task")] string? message = null)
        => (await task).NotNull(() => new NullValueValidationException(message, null));

    [return: NotNull]
    public static async Task<T> NotNullAsync<T>([DisallowNull] this Task<T?> task, [DisallowNull] Func<Exception> getException)
        => (await task).NotNull(getException);

    public static T? NotValid<T>([AllowNull] this T? obj, [DisallowNull] in Func<T, bool> validate, [DisallowNull] in Func<Exception> getException)
        => !(validate?.Invoke(obj) ?? false) ? obj : getException is null ? obj : Throw<T>(getException);

    public static void OnFalse([DoesNotReturnIf(true)] this bool ok, Func<Exception> getException)
        => Check.MustBe(ok, getException);

    public static T ThrowIfDisposed<T>(this T @this, [DoesNotReturnIf(true)] bool disposed)
        where T : IDisposable
    {
        Check.MustBe(!disposed, () => new ObjectDisposedException(@this?.GetType().Name));
        return @this;
    }
}