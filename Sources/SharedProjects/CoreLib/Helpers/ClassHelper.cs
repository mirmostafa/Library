using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Mohammad.Helpers
{
    /// <summary>
    ///     A utility to do some common tasks about classes
    /// </summary>
    public static class ClassHelper
    {
        public static TAttribute GetDeclaringTypeAttribute<TAttribute>(this MethodBase method, TAttribute defaultValue = null) where TAttribute : class

        {
            var attributes = method?.DeclaringType?.GetCustomAttributes(typeof(TAttribute), false);
            var result = (from Attribute attribute in attributes
                          where attribute is TAttribute
                          select attribute as TAttribute).FirstOrDefault() ?? defaultValue;
            return result;
        }

        public static string GetClassDescription(this MethodBase method, bool getClassNameIfNotFound)
        {
            var attributes = method.DeclaringType.GetCustomAttributes(typeof(DescriptionAttribute), false);
            var result = string.Empty;
            foreach (var attribute in attributes.Where(attribute => attribute is DescriptionAttribute))
            {
                result = (attribute as DescriptionAttribute).Description;
                break;
            }
            if (string.IsNullOrEmpty(result) && getClassNameIfNotFound)
                result = GetClassName(method);
            return result;
        }

        public static string GetClassName(object classInstance, string defaultValue)
        {
            var result = classInstance == null ? defaultValue : (classInstance is string ? classInstance.ToString() : classInstance.GetType().ToString());
            if (result.LastIndexOf(".", StringComparison.Ordinal) > 0)
                result = result.Substring(result.LastIndexOf(".", StringComparison.Ordinal) + 1);
            return result;
        }

        public static string GetClassName(this MethodBase method) { return method?.DeclaringType?.Name; }
        public static string GetDescription(object classInstance) { return GetDescription(classInstance, ""); }

        public static string GetDescription(object classInstance, string defaultValue)
        {
            var result = ObjectHelper.GetDescription(classInstance);
            if (string.IsNullOrEmpty(result))
                result = defaultValue;
            return result;
        }
    }
}