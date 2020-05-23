#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Library40.ExceptionHandlingPattern;
using Library40.Exceptions;
using Library40.Validation.Attributes;
using Library40.Validation.Exceptions;

namespace Library40.Validation
{
	public class Validator : IExceptionHandlerContainer<Exception>
	{
		public Validator(ExceptionHandling<Exception> exceptionHandling)
		{
			this.ExceptionHandling = exceptionHandling;
		}

		public Validator()
		{
		}

		#region IExceptionHandlerContainer<Exception> Members
		public ExceptionHandling<Exception> ExceptionHandling { get; set; }
		#endregion

		/// <summary>
		///     Returns true if object is valid. otherwise fills "exception" and returns false.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="property"></param>
		/// <param name="exceptionBase"></param>
		/// <returns></returns>
		protected bool ParseValidationByAttributes(object obj, PropertyInfo property, out ValidationExceptionBase exceptionBase)
		{
			exceptionBase = null;
			if (property.GetCustomAttributes(typeof (NotNullAttribute), true).Length > 0)
				try
				{
					var notNull = (NotNullAttribute)property.GetCustomAttributes(typeof (NotNullAttribute), true)[0];
					AssertNotNull(property.GetValue(obj, null), notNull.FriendlyName ?? property.Name);
				}
				catch (ValidationExceptionBase ex)
				{
					exceptionBase = ex;
					return false;
				}
			if (property.GetCustomAttributes(typeof (IsBetweenAttribute), true).Length > 0)
				try
				{
					var isBetween = (IsBetweenAttribute)property.GetCustomAttributes(typeof (IsBetweenAttribute), true)[0];
					this.AssertIsBetween(property.GetValue(obj, null), isBetween.MinValue, isBetween.MaxValue, isBetween.FriendlyName ?? property.Name);
				}
				catch (ValidationExceptionBase ex)
				{
					exceptionBase = ex;
					return false;
				}
			return true;
		}

		public void AssertNotNull(object obj, string name)
		{
			if (obj == null || string.IsNullOrEmpty(obj.ToString()))
				throw new NotNullOrZeroValidationExceptionBase(name);
		}

		public void AssertNotNull(IEnumerable<Object> obj, string name)
		{
			if (obj == null || string.IsNullOrEmpty(obj.ToString()))
				throw new NotNullOrZeroValidationExceptionBase(name);
			if (obj.Count() == 0)
				throw new NotNullOrZeroValidationExceptionBase(name);
		}

		public void AssertNotDefault<T>(T obj, string name)
		{
			if (obj.Equals(default(T)))
				throw new NotNullOrZeroValidationExceptionBase(name);
		}

		public void AssertIsBetween(object obj, long minValue, long maxValue, string name)
		{
			long value;
			if (obj == null || !long.TryParse(obj.ToString(), out value)) // || !value.IsBetween(minValue, maxValue))
				throw new NotBetweenMaxAndMinValuesValidationExceptionBase(name, minValue, maxValue);
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
				throw new NotLessThanValueValidationExceptionBase(name, minValue);
		}

		public void AssertEqual<TType>(TType obj1, string obj1Name, TType obj2, string obj2Name) where TType : IEquatable<TType>
		{
			if (!obj1.Equals(obj2))
				throw new NotEqualValidationExceptionBase(obj1Name, obj2Name);
		}

		protected virtual void Throw<TValidationException>(Func<TValidationException> ctor) where TValidationException : ValidationExceptionBase
		{
			if (this.ExceptionHandling != null)
				this.ExceptionHandling.HandleException(ctor());
			else
				ExceptionBase.Throw(ctor);
		}
	}
}