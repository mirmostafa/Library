using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mohammad.Win.Actions
{
    /// <summary>
    /// </summary>
    public class ActionTargetDescriptionInfo
    {
        private readonly Dictionary<string, PropertyInfo> properties;

        /// <summary>
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        /// <param name="targetType"></param>
        public ActionTargetDescriptionInfo(Type targetType)
        {
            this.properties = new Dictionary<string, PropertyInfo>();
            this.TargetType = targetType;

            foreach (var property in targetType.GetProperties())
                this.properties.Add(property.Name, property);
        }

        internal void SetValue(string propertyName, object target, object value)
        {
            if (this.properties.ContainsKey(propertyName))
                this.properties[propertyName].SetValue(target, value, null);
        }

        internal object GetValue(string propertyName, object source)
        {
            if (this.properties.ContainsKey(propertyName))
                return this.properties[propertyName].GetValue(source, null);

            return null;
        }
    }
}