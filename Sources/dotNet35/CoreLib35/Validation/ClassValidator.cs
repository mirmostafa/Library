#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Library35.ExceptionHandlingPattern;
using Library35.Helpers;
using Library35.Validation.Exceptions;

namespace Library35.Validation
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
				ValidationException exception;
				if (!this.ParseValidationByAttributes(obj, property, out exception))
					throw exception;
			}
		}

		public IEnumerable<ValidationException> SummarizeValations(object obj)
		{
			ValidationException exception;
			foreach (var property in obj.GetType().GetProperties())
				if (!this.ParseValidationByAttributes(obj, property, out exception))
					yield return exception;
		}

		public void ValidateAny(object obj, params string[] propNames)
		{
			foreach (var property in
				obj.GetType().GetProperties().Where(prop => prop.Name.IsInRange(propNames)))
			{
				ValidationException exception;
				if (!this.ParseValidationByAttributes(obj, property, out exception))
					throw exception;
			}
		}

		public void ValidateExcept(object obj, params string[] propNames)
		{
			foreach (var property in
				obj.GetType().GetProperties().Where(prop => !prop.Name.IsInRange(propNames)))
			{
				ValidationException exception;
				if (!this.ParseValidationByAttributes(obj, property, out exception))
					throw exception;
			}
		}

		public void ValidateByPredicator(object obj, Predicate<object> predicator, params string[] propNames)
		{
			foreach (var property in
				obj.GetType().GetProperties().Where(prop => prop.Name.IsInRange(propNames)))
				base.ValidateByPredicator(property.GetValue(obj, null), predicator, new ValidationException());
		}

		public void ValidateByPredicator(object obj, Predicate<object> predicator, Exception ex, params string[] propNames)
		{
			foreach (var property in
				obj.GetType().GetProperties().Where(prop => prop.Name.IsInRange(propNames)))
				base.ValidateByPredicator(property.GetValue(obj, null), predicator, ex);
		}
	}
}