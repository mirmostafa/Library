#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

#region
using System;
using System.ComponentModel;
using System.Reflection;
#endregion

namespace Library35.Helpers
{
	/// <summary>
	///     A utility to do some common tasks about classes
	/// </summary>
	public static class ClassHelper
	{
		#region Methods

		#region GetAttribute
		public static TAttribute GetAttribute<TAttribute>(this MethodBase method, TAttribute defaultValue) where TAttribute : Attribute
		{
			var attributes = method.DeclaringType.GetCustomAttributes(typeof (TAttribute), false);
			var result = default(TAttribute);
			foreach (Attribute attribute in attributes)
				if ((attribute != null) && (attribute is TAttribute))
				{
					result = attribute as TAttribute;
					break;
				}
			if (result == default(TAttribute))
				result = defaultValue;
			return result;
		}
		#endregion

		#region GetClassDescription
		public static string GetClassDescription(this MethodBase method, bool getClassNameIfNotFound)
		{
			var attributes = method.DeclaringType.GetCustomAttributes(typeof (DescriptionAttribute), false);
			var result = string.Empty;
			foreach (Attribute attribute in attributes)
				if ((attribute != null) && (attribute is DescriptionAttribute))
				{
					result = (attribute as DescriptionAttribute).Description;
					break;
				}
			if (string.IsNullOrEmpty(result) && getClassNameIfNotFound)
				result = GetClassName(method);
			return result;
		}
		#endregion

		#region GetClassName
		public static string GetClassName(object classInstance, string defaultValue)
		{
			var result = (classInstance == null) ? defaultValue : ((classInstance is string) ? classInstance.ToString() : classInstance.GetType().ToString());
			if (result.LastIndexOf(".") > 0)
				result = result.Substring(result.LastIndexOf(".") + 1);
			return result;
		}
		#endregion

		#region GetClassName
		public static string GetClassName(this MethodBase method)
		{
			return method.DeclaringType.Name;
		}
		#endregion

		#region GetDescription
		public static string GetDescription(object classInstance)
		{
			return GetDescription(classInstance, "");
		}
		#endregion

		#region GetDescription
		public static string GetDescription(object classInstance, string defaultValue)
		{
			var result = ObjectHelper.GetDescription(classInstance);
			if (string.IsNullOrEmpty(result))
				result = defaultValue;
			return result;
		}
		#endregion

		#endregion
	}
}