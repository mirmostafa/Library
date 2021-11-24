using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using Library.Exceptions.Validations;

namespace Library.Validations;

[DebuggerStepThrough]
public static class Check
{
    public static void IfArgumentBiggerThan(in int arg, in int min, [CallerArgumentExpression("arg")] in string? argName = null)
    {
        if (arg <= min)
        {
            Throw(new ArgumentException(string.Format(
                CultureInfo.CurrentCulture,
                "The argument {0}(:'{1}') cannot be lass than {2}",
                argName,
                arg,
                min)));
        }
    }

    [return: NotNull]
    public static string ArgumentNotNull([NotNull] this string? obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        obj.NotValid(x => x.IsNullOrEmpty(), () => new ArgumentNullException(argName));

    [return: NotNull]
    public static T ArgumentNotNull<T>([NotNull] this T obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        obj.NotValid(x => x is null, () => new ArgumentNullException(argName));

    public static void IfIs<T>([NotNull] this object obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        _ = obj.NotValid(x => x is not T, () => new TypeMismatchValidationException(argName!));

    [return: NotNull]
    public static T Is<T>(this object obj, string argName) =>
        obj.NotValid(x => x is not T, () => new TypeMismatchValidationException(argName)).To<T>();

    public static T ArgumentOutOfRange<T>(this T obj, Func<T, bool> validate, [CallerArgumentExpression("obj")] string? argName = null) =>
        obj.NotValid(validate, () => new ArgumentOutOfRangeException(argName));

    public static void IfArgumentNotNull([NotNull] in string? obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        _ = obj.NotValid(x => x is null, () => new ArgumentNullException(argName));

    public static void IfArgumentNotNull<T>([NotNull] this string? obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        _ = obj.NotValid(StringHelper.IsNullOrEmpty, () => new ArgumentNullException(argName));

    /// <summary>
    /// Makes sure the specified argument is not null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="argName"></param>
    /// <exception cref="ArgumentNullException"/>
    public static void IfArgumentNotNull([NotNull] this object? obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        MustBeArgumentNotNull(obj is not null, argName!);

    public static void IfNotNull<T>([NotNull] this T obj, [DisallowNull] Func<Exception> getException) =>
        _ = obj.NotValid(x => x is null, getException)!;

    public static void IfHasAny([NotNull] IEnumerable obj, [DisallowNull] Func<Exception> getException)
    {
        if (!obj.NotNull(getException).Any())
        {
            Throw(getException);
        }
    }
    public static TEnumerable HasAny<TEnumerable>([NotNull] this TEnumerable obj, [DisallowNull] Func<Exception> getException)
        where TEnumerable : IEnumerable
    {
        if (!obj.NotNull().Any())
        {
            Throw(getException);
        }
        return obj;
    }
    public static void IfHasAny<TEnumerable>([NotNull] IEnumerable obj, [DisallowNull] Func<Exception> getException) =>
        _ = obj.HasAny(getException);
    public static void IfNotNull<T>([NotNull] this T obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        MustBeNotNull(obj is not null, argName!);

    public static void IfNotValid<T>(T obj, [DisallowNull] in Func<T, bool> validate, in Func<Exception> getException) =>
        obj.NotValid(validate, getException);

    [return: NotNull]
    public static string NotNull([NotNull] this string? obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        obj.NotValid(x => x.IsNullOrEmpty(), () => new NullValueValidationException(argName!))!;

    public static void IfAny([NotNull] IEnumerable obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        IfNotValid(obj, _ => !obj.Any(), () => new NoItemValidationException(argName!));

    [return: NotNull]
    public static T NotNull<T>([NotNull] this T? obj, [DisallowNull] Func<Exception> getException) =>
        obj.NotValid(x => x is null, getException)!;

    [return: NotNull]
    public static T NotNull<T, TException>([NotNull] this T obj)
        where TException : Exception, new() =>
        obj.NotValid(x => x is null, () => new TException());

    [return: NotNull]
    public static T NotNull<T>([NotNull] this T? obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        obj.NotValid(x => x is null, () => new NullValueValidationException(argName!))!;

    public static T NotNull<T>([NotNull] this T @this, [DisallowNull] object obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        @this.NotValid(_ => obj is null, () => new NullValueValidationException(argName!))!;

    public static T NotNull<T>([NotNull] this T @this, string obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        @this.NotValid(_ => obj.IsNullOrEmpty(), () => new NullValueValidationException(argName!))!;

    public static void IfNotNull<T>([NotNull] this T @this, string? obj, [CallerArgumentExpression("obj")] string? argName = null) =>
        _ = @this.NotValid(_ => obj.IsNullOrEmpty(), () => new NullValueValidationException(argName!))!;

    public static T NotValid<T>(this T obj, [DisallowNull] in Func<T, bool> validate, [DisallowNull] in Func<Exception> getException) =>
        !(validate?.Invoke(obj) ?? false) ? obj : getException is null ? obj : Throw<T>(getException);

    public static void MustBe([DoesNotReturnIf(false)] bool ok, in Func<Exception> getException)
    {
        if (!ok)
        {
            Throw(getException);
        }
    }

    public static void MustBe<TValidationException>([DoesNotReturnIf(false)] bool ok)
        where TValidationException : Exception, new() =>
        MustBe(ok, () => new TValidationException());

    public static void Require([DoesNotReturnIf(false)] bool required) =>
        MustBe<RequiredValidationException>(required);

    public static void Require([DoesNotReturnIf(false)] bool required, string message, string? instruction = null, string? title = null) =>
        MustBe(required, () => new RequiredValidationException(message, instruction, title));

    public static void MustBeNotNull([DoesNotReturnIf(false)] bool isNotNull, string argName) =>
        MustBe(isNotNull, () => new NullValueValidationException(argName));

    public static void MustBeArgumentNotNull([DoesNotReturnIf(false)] bool isNotNull, [DisallowNull] string argName) =>
        MustBe(isNotNull, () => new ArgumentNullException(argName));

    public static T HasAny<T>(this T obj, IEnumerable items, [CallerArgumentExpression("items")] string? atemsName = null) =>
        NotValid(obj, _ => items?.Any() ?? false, () => new NoItemValidationException($"No items founds in '{atemsName}'."));
}