#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace Mohammad.Helpers
{
    /// <summary>
    ///     A utility to do some common tasks about properties
    /// </summary>
    public static class PropertyHelper
    {
        public static T[] GetArray<T>(ref T[] variable, int count) => variable ?? (variable = new T[count]);

        public static TReturnType GetValue<TTargetType, TReturnType>(TTargetType target, string propertyName)
        {
            var properties = typeof(TTargetType).GetProperties();
            foreach (var property in properties.Where(property => string.Compare(property.Name, propertyName, StringComparison.Ordinal) == 0))
                return (TReturnType) property.GetValue(target, null);

            return default(TReturnType);
        }

        public static TType Get<TType>(ref TType variable) where TType : class, new() => Get(ref variable, () => new TType());
        internal static TType Get<TType>(ref TType variable, TType defaultValue) where TType : class => variable ?? (variable = defaultValue);
        public static TType Get<TType>(ref TType variable, Func<TType> creator) where TType : class => variable ?? (variable = creator());

        public static IEnumerable<TType> Get<TType>(IEnumerable<TType> backingField, Func<IEnumerable<TType>> gatherItems, Action<IList<TType>> initiated)
        {
            if (gatherItems == null)
                throw new ArgumentNullException(nameof(gatherItems));
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
                    yield return items;
            }
        }

        public static bool SetValue<TTargetType>(TTargetType target, string propertyName, object propertyValue)
            => SetValue(target, propertyName, propertyValue, false);

        public static bool SetValue<TTargetType>(TTargetType target, string propertyName, object propertyValue, bool searchAll)
            =>
                (searchAll
                    ? typeof(TTargetType).GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    : typeof(TTargetType).GetProperties()).Where(property => string.Compare(property.Name, propertyName, StringComparison.Ordinal) == 0)
                .Select(property => !CodeHelper.HasException(() => property.SetValue(target, propertyValue, null)))
                .FirstOrDefault();
    }
}