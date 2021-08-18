﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Library.Exceptions.Validations;

namespace Library.Helpers;
[DebuggerStepThrough]
public static class Check
{
    public static void ArgumentBiggerThan(in int arg, in int min, in string argName)
    {
        IfArgumentNotNull(argName, nameof(argName));
        if (arg <= min)
        {
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                "The argument {0}(={1}) cannot be lass than {2}",
                argName,
                arg,
                min));
        }
    }

    [return: NotNull]
    public static string ArgumentNotNull([NotNull] this string? obj, string? argName = null)
        => NotValid(obj, x => x.IsNullOrEmpty(), () => new ArgumentNullException(argName))!;

    [return: NotNull]
    public static T ArgumentNotNull<T>([NotNull] this T? obj, string? argName = null)
        => NotValid(obj, x => x is null, () => new ArgumentNullException(argName))!;

    public static T ArgumentOutOfRange<T>(this T obj, Predicate<T> validate, string? argName = null)
        => NotValid(obj, validate, () => new ArgumentOutOfRangeException(argName));

    public static void IfArgumentIsNotValid<T>(this T? obj, Predicate<T?> validator, string? argName)
        => _ = NotValid(obj, validator.ArgumentNotNull(nameof(validator)), () => new ArgumentNullException(argName))!;

    public static void IfArgumentNotNull([NotNull] this string? obj, string argName)
        => _ = NotValid(obj, x => x is null, () => new ArgumentNullException(argName))!;

    public static void IfArgumentNotNull<T>([NotNull] this string? obj, string? argName = null)
        => _ = NotValid(obj, StringHelper.IsNullOrEmpty, () => new ArgumentNullException(argName))!;

    public static void IfArgumentNotNull<T>([NotNull] this T? obj, string argName)
        => _ = NotValid(obj, x => x is null, () => new ArgumentNullException(argName))!;

    public static void IfNotNull<T>([NotNull] this T obj, Func<Exception> getException)
        => _ = NotValid(obj, x => x is null, getException)!;

    public static void IfNotNull<T>([NotNull] this T obj, string name)
        => _ = NotValid(obj, x => x is null, () => new NullValueValidationException(name))!;

    public static void IfNotValid(object? obj, Predicate<object> validate, Func<Exception> getException)
        => NotValid(obj, validate, getException);

    [return: NotNull]
    public static string NotNull([NotNull] this string? obj, string name)
        => NotValid(obj, x => x.IsNullOrEmpty(), () => new NullValueValidationException(name))!;

    [return: NotNull]
    public static T NotNull<T>([NotNull] this T obj, Func<Exception> getException)
        => NotValid(obj, x => x is null, getException)!;

    [return: NotNull]
    public static T NotNull<T>([NotNull] this T obj, string name)
        => NotValid(obj, x => x is null, () => new NullValueValidationException(name))!;

    public static T NotNull<T>(this T @this, [NotNull] object? obj, string name)
        => NotValid(@this, _ => obj is null, () => new NullValueValidationException(name))!;

    public static T NotNull<T>(this T @this, string? obj, string name)
        => NotValid(@this, _ => obj.IsNullOrEmpty(), () => new NullValueValidationException(name))!;

    public static void IfNotNull<T>(this T @this, string? obj, string name)
        => _ = NotValid(@this, _ => obj.IsNullOrEmpty(), () => new NullValueValidationException(name))!;

    public static T NotValid<T, TException>(this T obj, Predicate<T> validate)
        where TException : Exception, new()
        => NotValid(obj, validate, static () => new TException());

    public static T NotValid<T>(this T obj, Predicate<T> validate, Func<Exception> getException)
        => !(validate?.Invoke(obj) ?? false) ? obj : (getException is null ? obj : throw getException());

    public static T NotValid<T>(this T obj, Predicate<T> validate, Func<T, Exception> getException)
        => !(validate?.Invoke(obj) ?? false) ? obj : (getException is null ? obj : throw getException(obj));

    public static T NotValid<T>(this T obj, Predicate<T> validate, string? message = null)
        => !(validate?.Invoke(obj) ?? false) ? obj : throw new ValidationException(message ?? string.Empty);
}