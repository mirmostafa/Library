#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Library35.Windows.Actions
{
	/// <summary>
	/// </summary>
	public class ActionTargetDescriptionInfo
	{
		private readonly Dictionary<string, PropertyInfo> properties;

		private readonly Type targetType;

		/// <summary>
		///     Creates a new instance
		/// </summary>
		/// <param name="targetType"></param>
		public ActionTargetDescriptionInfo(Type targetType)
		{
			this.properties = new Dictionary<string, PropertyInfo>();
			this.targetType = targetType;

			foreach (var property in targetType.GetProperties())
				this.properties.Add(property.Name, property);
		}

		/// <summary>
		/// </summary>
		public Type TargetType
		{
			get { return this.targetType; }
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