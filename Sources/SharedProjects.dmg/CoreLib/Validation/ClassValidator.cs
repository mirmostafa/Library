using System;
using System.Collections.Generic;
using System.Linq;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.Helpers;
using Mohammad.Validation.Exceptions;

namespace Mohammad.Validation
{
    public class ClassValidator : Validator
    {
        public ClassValidator(ExceptionHandling<Exception> exceptionHandling)
            : base(exceptionHandling) { }

        public void Validate(object obj)
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                if (!this.TryValideByAttributes(obj, property, out IValidationException exception))
                    throw exception as Exception;
            }
        }

        public IEnumerable<IValidationException> SummarizeValidations(object obj)
        {
            foreach (var property in obj.GetType().GetProperties())
                if (!this.TryValideByAttributes(obj, property, out IValidationException exception))
                    yield return exception;
        }

        public void ValidateAny(object obj, params string[] propNames)
        {
            foreach (var property in
                obj.GetType().GetProperties().Where(prop => prop.Name.IsInRange(propNames)))
            {
                if (!this.TryValideByAttributes(obj, property, out IValidationException exception))
                    throw exception as Exception;
            }
        }

        public void ValidateExcept(object obj, params string[] propNames)
        {
            foreach (var property in
                obj.GetType().GetProperties().Where(prop => !prop.Name.IsInRange(propNames)))
            {
                if (!this.TryValideByAttributes(obj, property, out IValidationException exception))
                    throw exception as Exception;
            }
        }
    }
}