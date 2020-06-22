using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mohammad.DesignPatterns.Creational;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.Validation.Attributes;
using Mohammad.Validation.Exceptions;

namespace Mohammad.Validation
{
    public class Validator : Singleton<Validator>,IExceptionHandlerContainer<Exception>
    {
        static Validator CreateInstance() => new Validator();

        public Validator(ExceptionHandling<Exception> exceptionHandling) { this.ExceptionHandling = exceptionHandling; }
        public Validator() { }

        /// <summary>
        ///     Returns true if object is valid. otherwise fills "exception" and returns false.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected bool TryValideByAttributes(object obj, PropertyInfo property, out IValidationException exception)
        {
            exception = null;
            if (property.GetCustomAttributes(typeof(NotNullAttribute), true).Length > 0)
                try
                {
                    var notNull = (NotNullAttribute) property.GetCustomAttributes(typeof(NotNullAttribute), true)[0];
                    this.AssertNotNull(property.GetValue(obj, null), notNull.FriendlyName ?? property.Name);
                }
                catch (NotNullOrZeroValidationException ex)
                {
                    exception = ex;
                    return false;
                }
            if (property.GetCustomAttributes(typeof(IsBetweenAttribute), true).Length <= 0)
                return true;
            try
            {
                var isBetween = (IsBetweenAttribute) property.GetCustomAttributes(typeof(IsBetweenAttribute), true)[0];
                this.AssertIsBetween(property.GetValue(obj, null), isBetween.MinValue, isBetween.MaxValue, isBetween.FriendlyName ?? property.Name);
            }
            catch (NotBetweenMaxAndMinValuesValidationException ex)
            {
                exception = ex;
                return false;
            }
            return true;
        }

        public void AssertNotNull(object obj, string name)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                throw new NotNullOrZeroValidationException(name);
        }

        public void AssertNotNull(IEnumerable<object> obj, string name)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                throw new NotNullOrZeroValidationException(name);
            if (!obj.Any())
                throw new NotNullOrZeroValidationException(name);
        }

        public void AssertNotDefault<T>(T obj, string name)
        {
            if (obj.Equals(default(T)))
                throw new NotNullOrZeroValidationException(name);
        }

        public void AssertIsBetween(object obj, long minValue, long maxValue, string name)
        {
            long value;
            if (obj == null || !long.TryParse(obj.ToString(), out value)) // || !value.IsBetween(minValue, maxValue))
                throw new NotBetweenMaxAndMinValuesValidationException(name, minValue, maxValue);
        }

        public void ValidateByPredicator(object obj, Predicate<object> predicator, Exception ex)
        {
            if (!predicator(obj))
                throw ex;
        }

        public void AssertIsNotLessThen(object obj, long minValue, string name)
        {
            long value;
            if (obj == null || !long.TryParse(obj.ToString(), out value)) // || !value.IsLessThan(minValue))
                throw new NotLessThanValueValidationException(name, minValue);
        }

        public void AssertEqual<TType>(TType obj1, string obj1Name, TType obj2, string obj2Name) where TType : IEquatable<TType>
        {
            if (!obj1.Equals(obj2))
                throw new NotEqualValidationException(obj1Name, obj2Name);
        }

        public ExceptionHandling<Exception> ExceptionHandling { get; set; }
    }
}