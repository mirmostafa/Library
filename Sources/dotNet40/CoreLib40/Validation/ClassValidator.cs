#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Library40.ExceptionHandlingPattern;
using Library40.Helpers;
using Library40.Validation.Exceptions;

namespace Library40.Validation
{
	public class ClassValidator : Validator
	{
		public ClassValidator(ExceptionHandling<Exception> exceptionHandling)
			: base(exceptionHandling)
		{
		}

		public void Validate(object obj)
		{
			foreach (var property in obj.GetType().GetProperties())
			{
				ValidationExceptionBase exceptionBase;
				if (!this.ParseValidationByAttributes(obj, property, out exceptionBase))
					throw exceptionBase;
			}
		}

		public IEnumerable<ValidationExceptionBase> SummarizeValations(object obj)
		{
			ValidationExceptionBase exceptionBase;
			foreach (var property in obj.GetType().GetProperties())
				if (!this.ParseValidationByAttributes(obj, property, out exceptionBase))
					yield return exceptionBase;
		}

		public void ValidateAny(object obj, params string[] propNames)
		{
			foreach (var property in
				obj.GetType().GetProperties().Where(prop => prop.Name.IsInRange(propNames)))
			{
				ValidationExceptionBase exceptionBase;
				if (!this.ParseValidationByAttributes(obj, property, out exceptionBase))
					throw exceptionBase;
			}
		}

		public void ValidateExcept(object obj, params string[] propNames)
		{
			foreach (var property in
				obj.GetType().GetProperties().Where(prop => !prop.Name.IsInRange(propNames)))
			{
				ValidationExceptionBase exceptionBase;
				if (!this.ParseValidationByAttributes(obj, property, out exceptionBase))
					throw exceptionBase;
			}
		}

		public void ValidateByPredicator(object obj, Predicate<object> predicator, params string[] propNames)
		{
			foreach (var property in
				obj.GetType().GetProperties().Where(prop => prop.Name.IsInRange(propNames)))
				base.ValidateByPredicator(property.GetValue(obj, null), predicator, new ValidationExceptionBase());
		}

		public void ValidateByPredicator(object obj, Predicate<object> predicator, Exception ex, params string[] propNames)
		{
			foreach (var property in
				obj.GetType().GetProperties().Where(prop => prop.Name.IsInRange(propNames)))
				base.ValidateByPredicator(property.GetValue(obj, null), predicator, ex);
		}
	}
}