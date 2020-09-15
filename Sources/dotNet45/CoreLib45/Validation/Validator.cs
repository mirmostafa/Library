using System;
using System.Collections.Generic;
using System.Linq;
using Mohammad.DesignPatterns.Creational;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.Helpers;
using Mohammad.Validation.Exceptions;

namespace Mohammad.Validation
{
    public class Validator : Singleton<Validator>, IExceptionHandlerContainer<Exception>
    {
        public Validator(ExceptionHandling<Exception> exceptionHandling) => this.ExceptionHandling = exceptionHandling;

        public Validator()
        {
        }

        public ExceptionHandling<Exception> ExceptionHandling { get; set; }

        public void AssertEqual<TType>(TType obj1, string obj1Name, TType obj2, string obj2Name)
            where TType : IEquatable<TType>
        {
            if (!obj1.Equals(obj2))
            {
                throw new NotEqualValidationException(obj1Name, obj2Name);
            }
        }

        public void AssertIsBetween(long obj, long minValue, long maxValue, string name)
        {
            if (!obj.IsBetween(minValue, maxValue))
            {
                throw new OutOfRanageValidationException(name, minValue, maxValue);
            }
        }

        public void AssertIsBetween(DateTime obj, DateTime minValue, DateTime maxValue, string name)
        {
            if (obj < minValue || obj > maxValue)
            {
                throw new OutOfRanageValidationException(name, minValue, maxValue);
            }
        }

        public void AssertIsNotLessThen(long obj, long minValue, string name)
        {
            if (!obj.IsLessThan(minValue))
            {
                throw new NotLessThanValueValidationException(name, minValue);
            }
        }

        public void AssertNotDefault<T>(T obj, string name)
        {
            if (obj?.Equals(default(T)) ?? true)
            {
                throw new NotNullOrZeroValidationException(name);
            }
        }

        public void AssertNotNull(object obj, string name)
        {
            if (string.IsNullOrEmpty(obj?.ToString()))
            {
                throw new NotNullOrZeroValidationException(name);
            }
        }

        public void AssertNotNull(IEnumerable<object> obj, string name)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                throw new NotNullOrZeroValidationException(name);
            }

            if (!obj.Any())
            {
                throw new NotNullOrZeroValidationException(name);
            }
        }
    }
}