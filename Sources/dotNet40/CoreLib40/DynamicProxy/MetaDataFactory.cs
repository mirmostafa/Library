#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Reflection;

namespace Library40.DynamicProxy
{
	/// <summary>
	///     Factory class used to cache Types instances
	/// </summary>
	public class MetaDataFactory
	{
		private static readonly Hashtable _TypeMap = new Hashtable();

		/// <summary>
		///     Class constructor. Private because this is a static class.
		/// </summary>
		private MetaDataFactory()
		{
		}

		/// <summary>
		///     Method to add a new Type to the cache, using the type's fully qualified name as the key
		/// </summary>
		/// <param name="interfaceType"> Type to cache </param>
		public static void Add(Type interfaceType)
		{
			if (interfaceType != null)
				lock (_TypeMap.SyncRoot)
				{
					if (!_TypeMap.ContainsKey(interfaceType.FullName))
						_TypeMap.Add(interfaceType.FullName, interfaceType);
				}
		}

		/// <summary>
		///     Method to return the method of a given type at a specified index.
		/// </summary>
		/// <param name="name"> Fully qualified name of the method to return </param>
		/// <param name="i"> Index to use to return MethodInfo </param>
		/// <returns> MethodInfo </returns>
		public static MethodInfo GetMethod(string name, int i)
		{
			Type type;
			lock (_TypeMap.SyncRoot)
			{
				type = (Type)_TypeMap[name];
			}

			var methods = type.GetMethods();
			return i < methods.Length ? methods[i] : null;
		}
	}
}