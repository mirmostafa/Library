using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;

using Library.Exceptions.Validations;
using Library.Results;

using ValidationException = Library.Exceptions.Validations.ValidationException;

namespace Library.Validations;

#if !DEBUG
[DebuggerStepThrough]
[StackTraceHidden]
#endif
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
        => If(required, () => new ValidationException(getMessage.NotNull()()));

    [return: NotNull]
    public static void IfHasAny([NotNull][AllowNull] IEnumerable? obj, [CallerArgumentExpression("obj")] string? argName = null)
        => IfNotValid(obj, _ => !obj?.Any() ?? false, () => new NoItemValidationException(argName!));

    [return: NotNull]
    public static void IfArgumentBiggerThan(in int arg, in int min, [CallerArgumentExpression("arg")] in string? argName = null)
    {
        if (arg <= min)
        {
            Throw(new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The argument {0}(:'{1}') cannot be lass than {2}", argName, arg, min)));
        }
    }

    [return: NotNull]
    public static void IfArgumentNotNull([NotNull][AllowNull] in string? obj, [CallerArgumentExpression("obj")] string? argName = null)
        => IfArgumentNotNull(!obj.IsNullOrEmpty(), argName!);

    [return: NotNull]
    public static void IfArgumentNotNull([NotNull][AllowNull] object? obj, [CallerArgumentExpression("obj")] string? argName = null)
        => IfArgumentNotNull(obj is not null, argName!);

    public static void IfArgumentNotNull([DoesNotReturnIf(false)] bool isNotNull, [DisallowNull] string argName)
        => If(isNotNull, () => new ArgumentNullException(argName));

    public static void IfHasAny([NotNull][AllowNull] IEnumerable? obj, [DisallowNull] Func<Exception> getException)
    {
        if (!obj.NotNull(getException).Any())
        {
            Throw(getException);
        }
    }

    //public static void IfHasAny<TEnumerable>([NotNull][AllowNull] IEnumerable? obj, [DisallowNull] Func<Exception> getException)
    //    => _ = obj.HasAny(getException);

    public static void IfIs<T>([NotNull][AllowNull] object? obj, [CallerArgumentExpression("obj")] string? argName = null)
        => _ = obj.NotValid(x => x is not T, () => new TypeMismatchValidationException(argName!));

    /// <summary>
    /// Makes sure the specified argument is not null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">    </param>
    /// <param name="argName"></param>
    /// <exception cref="ArgumentNullException"/>
    public static void IfNotNull([NotNull][AllowNull] object? obj, [DisallowNull] Func<Exception> getException)
        => _ = obj.NotValid(x => x is null, getException);

    public static void IfNotNull([NotNull][AllowNull] object? obj, [CallerArgumentExpression("obj")] string? argName = null)
        => IfNotNull(obj is not null, argName!);

    public static void IfNotNull([NotNull][AllowNull] object? obj, Func<string> getMessage)
        => obj.NotValid(x => x is null, () => new NullValueValidationException(getMessage()));

    public static void IfNotNull(in object? @this, [NotNull][AllowNull] string? obj, [CallerArgumentExpression("obj")] string? argName = null)
        => _ = @this.NotValid(_ => obj.IsNullOrEmpty(), () => new NullValueValidationException(argName!))!;

    public static void IfNotNull([DoesNotReturnIf(false)] bool isNotNull, string argName)
        => If(isNotNull, () => new NullValueValidationException(argName));

    public static void IfNotValid([AllowNull] object? obj, [DisallowNull] in Func<object?, bool> validate, in Func<Exception> getException)
        => obj.NotValid(validate, getException);

    public static void IfRequired([DoesNotReturnIf(false)] bool required)
        => If<RequiredValidationException>(required);

    public static Result MustBe(in bool ok)
        => ok ? Result.Success : Result.Fail;

    public static Result<T> MustBe<T>(in T obj, in bool ok)
        => ok ? Result<T>.CreateSuccess(obj) : Result<T>.CreateFail(value: obj);

    public static Result<T> MustBe<T>(in T obj, in Func<bool> predicate, in Func<Exception> getException)
        => predicate() ? Result<T>.CreateSuccess(obj) : Result<T?>.CreateFail(getException(), value: obj);

    public static Result<T> MustBe<T>(in T obj, in bool ok, in Func<Exception> getException)
        => ok ? Result<T>.CreateSuccess(obj) : Result<T>.CreateFail(getException(), value: obj);

    public static bool MustBe<T>(in T obj, in Func<bool> predicate, in Func<Exception> getException, out Result<T> result)
    {
        result = predicate() ? Result<T>.CreateSuccess(obj) : Result<T>.CreateFail(getException(), value: obj);
        return result.IsSucceed;
    }

    public static Result<T> MustBe<T>(in T obj, in Func<T, bool> predicate, in Func<Exception> getException)
        => predicate(obj) ? Result<T>.CreateSuccess(obj) : Result<T>.CreateFail(getException(), value: obj);

    public static Result MustBe(in bool ok, in Func<string> getErrorMessage)
        => ok ? Result.CreateSuccess() : Result.CreateFail(message: getErrorMessage());

    public static Result MustBe(in Func<bool> predicate, in Func<string> getErrorMessage)
        => predicate() ? Result.CreateSuccess() : Result.CreateFail(getErrorMessage());

    public static Result MustBe(in Func<bool> predicate, in Func<Exception> getException)
        => predicate() ? Result.Success : Result.CreateFail(error: getException());

    public static Result MustBe(in bool ok, in Func<Exception> getExceptionIfNot)
        => !ok ? Result.CreateFail(error: getExceptionIfNot()) : Result.CreateSuccess();

    public static Result<TValue?> MustBeArgumentNotNull<TValue>([AllowNull] TValue? obj, [CallerArgumentExpression("obj")] string? argName = null)
                => Result<TValue?>.From(MustBeArgumentNotNull(obj is not null, argName!), obj);

    public static Result MustBeArgumentNotNull(in bool isNotNull, [DisallowNull] string argName)
        => MustBe(isNotNull, () => new ArgumentNullException(argName));

    public static Result<T> MustBeNotNull<T>(T obj) where T : class
        => MustBe(obj!, () => obj is not null, () => new NullValueValidationException());

    public static Result<TInstance> MustBeNotNull<TInstance>(in TInstance instance, object? obj, [CallerArgumentExpression("obj")] string? argName = null)
        => MustBe(instance, () => obj is not null, () => new NullValueValidationException(argName!));

    public static Result<TInstance> MustBeNotNull<TInstance>(TInstance instance, Func<string> getExceptionMessage)
        => MustBe(instance, () => instance is not null, () => new NullValueValidationException(getExceptionMessage(), null));

    public static Result<string> MustBeNotNull(string? obj)
        => MustBe(obj!, () => !obj.IsNullOrEmpty(), () => new NullValueValidationException());

    public static Result<T> MustBeNotNull<T>(in T instance, string? obj, [CallerArgumentExpression("obj")] string? argName = null)
        => MustBe(instance, () => !obj.IsNullOrEmpty(), () => new NullValueValidationException(argName!));

    public static bool TryMustBeArgumentNotNull<TValue>([AllowNull][NotNullWhen(true)] TValue? obj, out Result<TValue?> result, [CallerArgumentExpression("obj")] string? argName = null)
        => MustBeArgumentNotNull(obj, argName).TryParse(out result);
}