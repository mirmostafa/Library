using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

using Library.Exceptions.Validations;
using Library.Results;

using ValidationException = Library.Exceptions.Validations.ValidationException;

namespace Library.Validations;

[DebuggerStepThrough]
[StackTraceHidden]
public sealed class Check
{
    private static Check? _that;

    private Check()
    { }

    /// <summary>
    /// Gets the singleton instance of the <see cref="Check"/> functionality.
    /// </summary>
    /// <remarks>
    /// Users can use this to plug-in custom assertions through C# extension methods.
    /// For instance, the signature of a custom assertion provider could be "public static
    /// void IsOfType<T>(this Assert assert, object obj)" Users could then use a syntax
    /// similar to the default assertions which in this case is "Assert.That.IsOfType<Dog>(animal);"
    /// </remarks>
    public static Check That => _that ??= new();

    public static void If([DoesNotReturnIf(false)] bool ok, in Func<Exception> getExceptionIfNot)
    {
        if (!ok)
        {
            Throw(getExceptionIfNot);
        }
    }

    public static void If<TValidationException>([DoesNotReturnIf(false)] bool ok) where TValidationException : Exception, new()
        => If(ok, () => new TValidationException());

    public static void If([DoesNotReturnIf(false)] bool required)
        => If<ValidationException>(required);

    public static void If([DoesNotReturnIf(false)] bool required, Func<string> getMessage)
        => If(required, () => new ValidationException(getMessage()));

    [return: NotNull]
    public static void IfArgumentBiggerThan(in int arg, in int min, [CallerArgumentExpression(nameof(arg))] in string? argName = null)
    {
        if (arg <= min)
        {
            Throw(new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The argument {0}(:'{1}') cannot be lass than {2}", argName, arg, min)));
        }
    }

    [return: NotNull]
    public static void IfArgumentNotNull([NotNull][AllowNull] object? obj, [CallerArgumentExpression(nameof(obj))] string? argName = null)
        => IfArgumentNotNull(obj is not null, argName!);

    [return: NotNull]
    public static void IfArgumentNotNull([NotNull][AllowNull] in string? obj, [CallerArgumentExpression(nameof(obj))] string? argName = null)
        => IfArgumentNotNull(!obj.IsNullOrEmpty(), argName!);

    public static void IfArgumentNotNull([DoesNotReturnIf(false)] bool isNotNull, [DisallowNull] string argName)
        => If(isNotNull, () => new ArgumentNullException(argName));

    [return: NotNull]
    public static void IfHasAny([NotNull][AllowNull] IEnumerable? obj, [CallerArgumentExpression(nameof(obj))] string? argName = null)
        => NotValid(obj, _ => !obj?.Any() ?? false, () => new NoItemValidationException(argName!));

    public static void IfHasAny([NotNull][AllowNull] IEnumerable? obj, [DisallowNull] Func<Exception> getException)
    {
        NotNull(obj, getException);
        if (!obj.Any())
        {
            Throw(getException);
        }
    }

    //public static void IfHasAny<TEnumerable>([NotNull][AllowNull] IEnumerable? obj, [DisallowNull] Func<Exception> getException)
    //    => _ = obj.HasAny(getException);

    public static void IfIs<T>([NotNull][AllowNull] object? obj, [CallerArgumentExpression(nameof(obj))] string? argName = null)
        => NotValid(obj, x => x is T, () => new TypeMismatchValidationException(argName!));

    public static void IfRequired([DoesNotReturnIf(false)] bool required)
        => If<RequiredValidationException>(required);

    /// <summary>
    /// Returns a result object based on a given boolean value.
    /// </summary>
    /// <param name="ok">The boolean value to check.</param>
    /// <returns>A result object that indicates success or failure.</returns>
    public static Result MustBe(in bool ok)
        => ok ? Result.Success : Result.Failure;

    public static Result<T> MustBe<T>(in T obj, in bool ok)
        => ok ? Result<T>.CreateSuccess(obj) : Result<T>.CreateFailure(value: obj);

    /// <summary>
    /// Checks if a given object satisfies a given predicate and returns a result object accordingly.
    /// </summary>
    /// <typeparam name="T">The type of the object to check.</typeparam>
    /// <param name="obj">The object to check.</param>
    /// <param name="predicate">The predicate to evaluate on the object.</param>
    /// <param name="getException">The function to get the exception to throw in case of failure.</param>
    /// <returns>A result object that contains either the original object or an exception.</returns>
    public static Result<T> MustBe<T>(in T obj, in Func<bool> predicate, in Func<Exception> getException)
        => predicate() ? Result<T>.CreateSuccess(obj) : Result<T?>.CreateFailure(getException(), value: obj);

    public static Result<T> MustBe<T>(in T obj, in bool ok, in Func<Exception> getException)
        => ok ? Result<T>.CreateSuccess(obj) : Result<T>.CreateFailure(getException(), value: obj);

    public static bool MustBe<T>(in T obj, in Func<bool> predicate, in Func<Exception> getException, out Result<T> result)
    {
        result = predicate() ? Result<T>.CreateSuccess(obj) : Result<T>.CreateFailure(getException(), value: obj);
        return result.IsSucceed;
    }

    public static Result<T> MustBe<T>(in T obj, in Func<T, bool> predicate, in Func<Exception> getException)
        => predicate(obj) ? Result<T>.CreateSuccess(obj) : Result<T>.CreateFailure(getException(), value: obj);

    public static Result MustBe(in bool ok, in Func<string> getErrorMessage)
        => ok ? Result.CreateSuccess() : Result.CreateFailure(message: getErrorMessage());

    public static Result MustBe(in Func<bool> predicate, in Func<string> getErrorMessage)
        => predicate() ? Result.CreateSuccess() : Result.CreateFailure(getErrorMessage());

    /// <summary>
    /// Checks if a given predicate evaluates to true and returns a result object accordingly.
    /// </summary>
    /// <param name="predicate">The predicate to evaluate.</param>
    /// <param name="getException">The function to get the exception to throw in case of failure.</param>
    /// <returns>A result object that indicates success or contains an exception.</returns>
    public static Result MustBe(in Func<bool> predicate, in Func<Exception> getException)
        => predicate() ? Result.Success : Result.CreateFailure(error: getException());

    public static Result MustBe(in bool ok, in Func<Exception> getExceptionIfNot)
        => !ok ? Result.CreateFailure(error: getExceptionIfNot()) : Result.CreateSuccess();

    public static Result<TValue?> MustBeArgumentNotNull<TValue>([AllowNull] TValue? obj, [CallerArgumentExpression(nameof(obj))] string? argName = null)
                => Result<TValue?>.From(MustBeArgumentNotNull(obj is not null, argName!), obj);

    public static Result MustBeArgumentNotNull(in bool isNotNull, [DisallowNull] string argName)
        => MustBe(isNotNull, () => new ArgumentNullException(argName));

    public static Result<T> MustBeNotNull<T>(T obj) where T : class
        => MustBe(obj!, () => obj is not null, () => new NullValueValidationException());

    public static Result<TInstance> MustBeNotNull<TInstance>(in TInstance instance, object? obj, [CallerArgumentExpression(nameof(obj))] string? argName = null)
        => MustBe(instance, () => obj is not null, () => new NullValueValidationException(argName!));

    public static Result<TInstance> MustBeNotNull<TInstance>(TInstance instance, Func<string> getExceptionMessage)
        => MustBe(instance, () => instance is not null, () => new NullValueValidationException(getExceptionMessage(), null));

    public static Result<string> MustBeNotNull(string? obj)
        => MustBe(obj!, () => !obj.IsNullOrEmpty(), () => new NullValueValidationException());

    public static Result<T> MustBeNotNull<T>(in T instance, string? obj, [CallerArgumentExpression(nameof(obj))] string? argName = null)
        => MustBe(instance, () => !obj.IsNullOrEmpty(), () => new NullValueValidationException(argName!));

    public static void NotNull([NotNull][AllowNull] object? obj, [CallerArgumentExpression(nameof(obj))] string? argName = null)
        => NotNull(obj is not null, argName!);

    public static void NotNull([NotNull][AllowNull] object? obj, Func<string> getMessage)
        => NotValid(obj, x => x is not null, () => new NullValueValidationException(getMessage()));

    public static void NotNull(in object? @this, [NotNull][AllowNull] string? obj, [CallerArgumentExpression(nameof(obj))] string? argName = null)
        => NotValid(@this, _ => !obj.IsNullOrEmpty(), () => new NullValueValidationException(argName!));

    public static void NotNull([DoesNotReturnIf(false)] bool isNotNull, string argName)
        => If(isNotNull, () => new NullValueValidationException(argName));

    /// <summary>
    /// Makes sure the specified argument is not null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">    </param>
    /// <param name="argName"></param>
    /// <exception cref="ArgumentNullException"/>
    public static void NotNull([NotNull][AllowNull] object? obj, [DisallowNull] Func<Exception> getException)
        => NotValid(obj, x => x is not null, getException);

    public static void NotValid([AllowNull] object? obj, [DisallowNull] in Func<object?, bool> validate, in Func<Exception> getException)
    {
        if (!validate(obj))
        {
            Throw(getException);
        }
    }

    public static bool TryMustBeArgumentNotNull<TValue>([AllowNull][NotNullWhen(true)] TValue? obj, out Result<TValue?> result, [CallerArgumentExpression(nameof(obj))] string? argName = null)
        => MustBeArgumentNotNull(obj, argName).TryParse(out result);
}