#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Library35.PlugIn
{
	public static class Composer
	{
		public static IEnumerable<TResult> Compose<TResult>(Assembly asm, string name, Func<Type, TResult> instanceCreator = null)
		{
			foreach (var type in asm.GetTypes())
			{
				if (typeof (TResult).Name != "Object" && !type.Name.Equals(typeof (TResult)))
					continue;
				var exportAttrobute = type.GetCustomAttributes(typeof (ExportAttribute), true).Cast<Attribute>().FirstOrDefault() as ExportAttribute;
				if (exportAttrobute == null)
					continue;
				if (string.IsNullOrEmpty(name))
					yield return (TResult)type.GetConstructor(new Type[]
					                                          {
					                                          }).Invoke(new object[]
					                                                    {
					                                                    });
				else if (exportAttrobute.Name == name || type.Name == name)
					yield return (TResult)type.GetConstructor(new Type[]
					                                          {
					                                          }).Invoke(new object[]
					                                                    {
					                                                    });
			}
		}

		public static IEnumerable Compose(string dllFileSpec, string name = null, Func<Type, object> instanceCreator = null)
		{
			return Compose(Assembly.LoadFrom(dllFileSpec), name, instanceCreator);
		}
	}
}