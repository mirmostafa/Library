using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mohammad.Exceptions;
using Mohammad.Helpers;
using Mohammad.Internals;
using Mohammad.Validation.Exceptions;

namespace Mohammad.Validation
{
    public static class ValidationHelper
    {
        public static void Validate<TException>([NotNull] Func<bool> isValid, string message, Action onIsValid,
            Func<TException, bool> onIsNotValid, object owner)
            where TException : ExceptionBase
        {
            if (isValid == null)
                throw new ArgumentNullException(nameof(isValid));
            try
            {
                if (!isValid())
                    ExceptionBase.WrapThrow<TException>(message, owner);
                onIsValid?.Invoke();
            }
            catch (TException ex)
            {
                if (onIsNotValid != null && onIsNotValid(ex))
                    return;
                throw;
            }
        }

        public static void Validate<TException>([NotNull] Func<bool> isValid, string message, Action onIsValid,
            Func<TException, bool> onIsNotValid)
            where TException : ExceptionBase
        {
            Validate(isValid, message, onIsValid, onIsNotValid, null);
        }

        public static void Validate<TException>([NotNull] Func<bool> isValid, string message, Func<TException, bool> onIsNotValid)
            where TException : ExceptionBase
        {
            Validate(isValid, message, null, onIsNotValid, null);
        }

        public static void Validate<TException>([NotNull] Func<bool> isValid, Func<TException, bool> onIsNotValid)
            where TException : ExceptionBase
        {
            Validate(isValid, null, null, onIsNotValid, null);
        }

        public static void Validate<TException>([NotNull] Func<bool> isValid, string message, Action onIsValid,
            Action<TException> onIsNotValid, object owner)
            where TException : ExceptionBase
        {
            Validate<TException>(isValid,
                message,
                onIsValid,
                ex =>
                {
                    onIsNotValid?.Invoke(ex);
                    return false;
                },
                owner);
        }

        public static void Validate([NotNull] Func<bool> isValid, string message, Action onIsValid,
            Action<ValidationException> onIsNotValid, object owner)
        {
            Validate<ValidationException>(isValid,
                message,
                onIsValid,
                ex =>
                {
                    onIsNotValid?.Invoke(ex);
                    return false;
                },
                owner);
        }

        public static void Validate([NotNull] Func<bool> isValid, string message, Action<ValidationException> onIsNotValid, object owner)
        {
            Validate<ValidationException>(isValid,
                message,
                null,
                ex =>
                {
                    onIsNotValid?.Invoke(ex);
                    return false;
                },
                owner);
        }

        public static void Validate([NotNull] Func<bool> isValid, string message, Action onIsValid, Func<Exception, bool> onIsNotValid,
            object owner)
        {
            Validate<ValidationException>(isValid, message, onIsValid, onIsNotValid, owner);
        }

        public static void Validate([NotNull] Func<bool> isValid, string message, Action onIsValid, Func<Exception, bool> onIsNotValid)
        {
            Validate<ValidationException>(isValid, message, onIsValid, onIsNotValid, null);
        }

        public static void Validate([NotNull] Func<bool> isValid, Action onIsValid, Func<Exception, bool> onIsNotValid)
        {
            Validate<ValidationException>(isValid, null, onIsValid, onIsNotValid, null);
        }

        public static void Validate([NotNull] Func<bool> isValid, Func<Exception, bool> onIsNotValid)
        {
            Validate<ValidationException>(isValid, null, null, onIsNotValid, null);
        }

        public static void Validate([NotNull] Func<bool> isValid, string message)
        {
            Validate(isValid, message, null, (Action<ValidationException>) null, null);
        }

        public static void ArgumentNotNull(string argument, string argumentName, Action onIsValid = null,
            Func<Exception, bool> onIsNotValid = null, object owner = null)
        {
            Validate<NotNullOrZeroValidationException>(() => argument != null,
                $"'{argumentName}' cannot be null",
                onIsValid,
                onIsNotValid,
                owner);
        }

        public static void ArgumentNotNullOrEmpty(string argument, string argumentName, Action onIsValid = null,
            Func<Exception, bool> onIsNotValid = null, object owner = null)
        {
            Validate<NotNullOrZeroValidationException>(() => !argument.IsNullOrEmpty(),
                $"'{argumentName}' cannot be null or empty",
                onIsValid,
                onIsNotValid,
                owner);
        }

        public static void ArgumentNotNumer(string argument, string argumentName, Action onIsValid = null,
            Func<Exception, bool> onIsNotValid = null, object owner = null)
        {
            Validate<NotNumberValidationException>(argument.IsNumber, $"'{argumentName}' must be a number", onIsValid, onIsNotValid, owner);
        }

        public static void ArgumentNotZero(IEnumerable enumerable, string argumentName, Action onIsInvalid = null,
            Func<Exception, bool> onIsNotValid = null, object owner = null)
        {
            Validate<NotNullOrZeroValidationException>(() => enumerable != null && enumerable.Any(),
                $"'{argumentName}' cannot be null or empty",
                onIsInvalid,
                onIsNotValid,
                owner);
        }

        /// <summary>
        ///     Finds the invalid items according to the specific item.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="isValid">Predicator.</param>
        /// <returns>
        ///     The invalid items according to the specific item.
        /// </returns>
        public static IEnumerable<string> FindInvalids(this IEnumerable<string> items, Func<object, bool> isValid) => items
            .Where(i => !isValid(i)).Select(i => i);

        /// <summary>
        ///     Finds null or empty strings.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The null or empty strings.</returns>
        public static IEnumerable<string> FindNullsOrEmpties(this IEnumerable<string> items) => items.Where(i => i.IsNullOrEmpty())
            .Select(i => i);

        /// <summary>
        ///     Finds the invalid items according to the specific item.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="isValid">Predicator.</param>
        /// <returns>
        ///     The invalid items according to the specific item.
        /// </returns>
        public static IEnumerable<(object data, string name)> FindInvalids(this IEnumerable<(object data, string name)> items,
            Func<object, bool> isValid) => items.Where(i => !isValid(i.data)).Select(i => i);

        /// <summary>
        ///     Finds null or empty strings.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The null or empty strings.</returns>
        public static IEnumerable<(string data, string name)> FindNullsOrEmpties(this IEnumerable<(string data, string name)> items) =>
            items.Where(i => i.data.IsNullOrEmpty()).Select(i => i);
    }
}