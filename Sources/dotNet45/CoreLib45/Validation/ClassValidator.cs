#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.Helpers;
using Mohammad.Validation.Attributes;
using Mohammad.Validation.Exceptions;

namespace Mohammad.Validation
{
    public class ClassValidator : Validator
    {
        public ClassValidator(ExceptionHandling<Exception> exceptionHandling)
            : base(exceptionHandling)
        {
        }

        public IEnumerable<ValidationException> SummarizeValidations(object obj, Func<string, string> translate)
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                (var isOk, var exception) = this.TryValidByAttributes(obj, property, translate(property.Name));
                if (!isOk)
                    yield return exception;
            }
        }

        protected (bool IsOk, ValidationException Exception) TryValidByAttributes(object obj, PropertyInfo property, string name)
        {
            try
            {
                this.ValidByAttributes(obj, property, p => name);
                return (true, null);
            }
            catch (ValidationException e)
            {
                return (false, e);
            }
        }

        public void Validate(object obj, Func<string, string> translate)
        {
            foreach (var property in obj.GetType().GetProperties())
                this.ValidByAttributes(obj, property, translate);
        }

        public void ValidateByPropNames(object obj, Func<string, string> translate, params string[] propNames)
        {
            foreach (var property in obj.GetType().GetProperties().Where(prop => prop.Name.IsInRange(propNames)))
                this.ValidByAttributes(obj, property, translate);
        }

        public void ValidateExcept(object obj, Func<string, string> translate, params string[] propNames)
        {
            foreach (var property in obj.GetType().GetProperties().Where(prop => !prop.Name.IsInRange(propNames)))
                this.ValidByAttributes(obj, property, translate);
        }

        protected void ValidByAttributes(object obj, PropertyInfo property, Func<string, string> translate)
        {
            var value = property.GetValue(obj, null);
            if (property.GetCustomAttributes(typeof(NotNullAttribute), true).Length > 0)
            {
                var notNull = (NotNullAttribute)property.GetCustomAttributes(typeof(NotNullAttribute), true)[0];
                this.AssertNotNull(value, translate(property.Name) ?? notNull.FriendlyName ?? property.Name);
            }

            if (property.GetCustomAttributes(typeof(IsBetweenNumberAttribute), true).Length > 0)
            {
                var d = long.Parse(value.ToString());
                var isBetween = (IsBetweenNumberAttribute)property.GetCustomAttributes(typeof(IsBetweenNumberAttribute), true)[0];
                this.AssertIsBetween(d, isBetween.MinValue, isBetween.MaxValue, isBetween.FriendlyName ?? property.Name);
            }

            if (property.GetCustomAttributes(typeof(IsBetweenDateAttribute), true).Length > 0)
            {
                var d = DateTime.Parse(value.ToString());
                var isBetween = (IsBetweenDateAttribute)property.GetCustomAttributes(typeof(IsBetweenNumberAttribute), true)[0];
                this.AssertIsBetween(d, isBetween.MinValue, isBetween.MaxValue, isBetween.FriendlyName ?? property.Name);
            }
        }
    }
}