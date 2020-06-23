


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mohammad.Helpers
{
    /// <summary>
    ///     A utility to do some common tasks about properties
    /// </summary>
    public static class PropertyHelper
    {
        public static TType Get<TType>(ref TType variable)
            where TType : class, new() => Get(ref variable, () => new TType());

        public static TType Get<TType>(ref TType variable, Func<TType> creator)
            where TType : class => variable ??= creator();

        public static IEnumerable<TType> Get<TType>(IEnumerable<TType> backingField, Func<IEnumerable<TType>> gatherItems, Action<IList<TType>> initiated)
        {
            if (gatherItems == null)
            {
                throw new ArgumentNullException(nameof(gatherItems));
            }

            if (backingField == null)
            {
                var result = new List<TType>();
                foreach (var item in gatherItems())
                {
                    result.Add(item);
                    yield return item;
                }

                initiated?.Invoke(result);
            }
            else
            {
                foreach (var items in backingField)
                {
                    yield return items;
                }
            }
        }

        public static T[] GetArray<T>(ref T[] variable, int count) => variable ??= new T[count];

        public static TReturnType GetValue<TTargetType, TReturnType>(TTargetType target, string propertyName)
        {
            var properties = typeof(TTargetType).GetProperties();
            foreach (var property in properties.Where(property => string.Compare(property.Name, propertyName, StringComparison.Ordinal) == 0))
            {
                return (TReturnType)property.GetValue(target, null);
            }

            return default;
        }

        public static bool SetValue<TTargetType>(TTargetType target, string propertyName, object propertyValue) => SetValue(target, propertyName, propertyValue, false);

        public static bool SetValue<TTargetType>(TTargetType target, string propertyName, object propertyValue, bool searchAll) =>
            (searchAll
                ? typeof(TTargetType).GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                : typeof(TTargetType).GetProperties()).Where(property => string.Compare(property.Name, propertyName, StringComparison.Ordinal) == 0)
            .Select(property => !CodeHelper.HasException(() => property.SetValue(target, propertyValue, null)))
            .FirstOrDefault();

        public static void Deconstruct(this PropertyInfo p, out bool isStatic, out bool isReadOnly, out bool isIndexed, out Type propertyType)
        {
            var getter = p.GetMethod;

            // Is the property read-only?
            isReadOnly = !p.CanWrite;

            // Is the property instance or static?
            isStatic = getter.IsStatic;

            // Is the property indexed?
            isIndexed = p.GetIndexParameters().Length > 0;

            // Get the property type.
            propertyType = p.PropertyType;
        }

        public static void Deconstruct(this PropertyInfo p, out bool hasGetAndSet, out bool sameAccess, out string access, out string getAccess, out string setAccess)
        {
            hasGetAndSet = sameAccess = false;
            string getAccessTemp = null;
            string setAccessTemp = null;

            MethodInfo getter = null;
            if (p.CanRead)
                getter = p.GetMethod;

            MethodInfo setter = null;
            if (p.CanWrite)
                setter = p.SetMethod;

            if (setter != null && getter != null)
                hasGetAndSet = true;

            if (getter != null)
            {
                if (getter.IsPublic)
                    getAccessTemp = "public";
                else if (getter.IsPrivate)
                    getAccessTemp = "private";
                else if (getter.IsAssembly)
                    getAccessTemp = "internal";
                else if (getter.IsFamily)
                    getAccessTemp = "protected";
                else if (getter.IsFamilyOrAssembly)
                    getAccessTemp = "protected internal";
            }

            if (setter != null)
            {
                if (setter.IsPublic)
                    setAccessTemp = "public";
                else if (setter.IsPrivate)
                    setAccessTemp = "private";
                else if (setter.IsAssembly)
                    setAccessTemp = "internal";
                else if (setter.IsFamily)
                    setAccessTemp = "protected";
                else if (setter.IsFamilyOrAssembly)
                    setAccessTemp = "protected internal";
            }

            // Are the accessibility of the getter and setter the same?
            if (setAccessTemp == getAccessTemp)
            {
                sameAccess = true;
                access = getAccessTemp;
                getAccess = setAccess = String.Empty;
            }
            else
            {
                access = null;
                getAccess = getAccessTemp;
                setAccess = setAccessTemp;
            }
        }

        internal static TType Get<TType>(ref TType variable, TType defaultValue) where TType : class => variable ??= defaultValue;
    }
}