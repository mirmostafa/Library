#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Reflection;

namespace Library40.Internals
{
	public class MethodCallingEventItem
	{
		public MethodCallingEventItem(Object proxy, MethodInfo method, Object[] parameters)
		{
			this.Proxy = proxy;
			this.Method = method;
			this.Parameters = parameters;
		}

		public object Proxy { get; private set; }
		public MethodInfo Method { get; private set; }
		public object[] Parameters { get; private set; }
	}
}