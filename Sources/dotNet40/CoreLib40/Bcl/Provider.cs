#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;

namespace Library40.Bcl
{
	public static class Provider
	{
		#region Delegates
		public delegate TInterface CreateInstance<out TInterface>();
		#endregion

		private static readonly Dictionary<Type, object> _Providers = new Dictionary<Type, object>();

		public static TInterface Get<TInterface>()
		{
			var result = _Providers[typeof (TInterface)];
			return result is CreateInstance<TInterface> ? (result as CreateInstance<TInterface>)() : (TInterface)result;
		}

		public static void Set<TInterface>(TInterface provider)
		{
			_Providers[typeof (TInterface)] = provider;
		}

		public static void Set<TInterface>(CreateInstance<TInterface> providerCreator)
		{
			_Providers[typeof (TInterface)] = providerCreator;
		}
	}
}