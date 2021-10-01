using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Library.Exceptions.Validations;

namespace Library.Validations;
#nullable disable
[DebuggerStepThrough]
public static class Check
{
    public static void IfArgumentBiggerThan(in int arg, in int min, in string argName)
    {
        argName.IfArgumentNotNull(nameof(argName));
        if (arg <= min)
        {
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                "The argument {0}(:'{1}') cannot be lass than {2}",
                argName,
                arg,
                min));
        }
    }
    public static void IfArgumentIsNotValid<T>(this T obj, Func<T, bool> validator, string argName)
        => _ = obj.NotValid(validator.ArgumentNotNull(nameof(validator)), () => new ArgumentNullException(argName))!;

    [return: NotNull]
    public static string ArgumentNotNull([NotNull] this string obj, string argName = null)
        => obj.NotValid(x => x.IsNullOrEmpty(), () => new ArgumentNullException(argName));

    [return: NotNull]
    public static T ArgumentNotNull<T>([NotNull] this T obj, string argName = null)
        => obj.NotValid(x => x is null, () => new ArgumentNullException(argName));

    [return: NotNull]
    public static void IfIs<T>(this object obj, string argName)
        => _ = obj.NotValid(x => x is not T, () => new TypeMismatchValidationException(argName));

    [return: NotNull]
    public static T Is<T>(this object obj, string argName)
        => obj.NotValid(x => x is not T, () => new TypeMismatchValidationException(argName)).To<T>();

    public static T ArgumentOutOfRange<T>(this T obj, Func<T, bool> validate, string argName = null)
        => obj.NotValid(validate, () => new ArgumentOutOfRangeException(argName));


    internal static void Require<TValidationException>(bool required)
        where TValidationException : ValidationExceptionBase, new()
    {
        if (!required)
        {
            throw new TValidationException();
        }
    }

    internal static void Require(bool required)
        => Require<ValidationException>(required);

    public static void IfArgumentNotNull([NotNull] this string obj, string argName)
        => _ = obj.NotValid(x => x is null, () => new ArgumentNullException(argName));

    public static void IfArgumentNotNull<T>([NotNull] this string obj, string argName = null)
        => _ = obj.NotValid(StringHelper.IsNullOrEmpty, () => new ArgumentNullException(argName));

    /// <summary>
    /// Makes sure the specified argument is not null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="argName"></param>
    /// <exception cref="ArgumentNullException"/>
    public static void IfArgumentNotNull<T>([NotNull] this T obj, string argName)
        => _ = obj.NotValid(x => x is null, () => new ArgumentNullException(argName));

    public static void IfNotNull<T>([NotNull] this T obj, Func<Exception> getException)
        => _ = obj.NotValid(x => x is null, getException);

    public static void IfNotNull<T>([NotNull] this T obj, string name)
        => _ = obj.NotValid(x => x is null, () => new NullValueValidationException(name));

    public static void IfNotValid(object obj, [DisallowNull] in Func<object, bool> validate, in Func<Exception> getException)
        => obj.NotValid(validate, getException);

    [return: NotNull]
    public static string NotNull([NotNull] this string obj, string name)
        => obj.NotValid(x => x.IsNullOrEmpty(), () => new NullValueValidationException(name))!;

    public static void IfAny([NotNull] IEnumerable obj, string name)
    {
        obj.IfNotNull(name);
        IfNotValid(obj, _ => !obj.Any(), () => new NoItemValidationException(name));
    }

    [return: NotNull]
    public static T NotNull<T>([NotNull] this T obj, Func<Exception> getException)
        => obj.NotValid(x => x is null, getException);

    [return: NotNull]
    public static T NotNull<T>([NotNull] this T obj, string name)
        => obj.NotValid(x => x is null, () => new NullValueValidationException(name))!;

    public static T NotNull<T>(this T @this, [NotNull] object obj, string name)
        => @this.NotValid(_ => obj is null, () => new NullValueValidationException(name))!;

    public static T NotNull<T>(this T @this, string obj, string name)
        => @this.NotValid(_ => obj.IsNullOrEmpty(), () => new NullValueValidationException(name))!;

    public static void IfNotNull<T>(this T @this, string obj, string name)
        => _ = @this.NotValid(_ => obj.IsNullOrEmpty(), () => new NullValueValidationException(name))!;

    public static T NotValid<T>(this T obj, [DisallowNull] in Func<T, bool> validate, [DisallowNull] in Func<Exception> getException)
        => !(validate?.Invoke(obj) ?? false) ? obj : getException is null ? obj : throw getException();

    public static T NotValid<T, TValidationException>(this T t, [DisallowNull] in Func<T, bool> validate, [DisallowNull] in string argName, [DisallowNull] in Func<string, TValidationException> getException)
        where TValidationException : ValidationExceptionBase
        => !validate.ArgumentNotNull(nameof(validate))(t) ? t : throw getException(argName.ArgumentNotNull(nameof(argName)));
}