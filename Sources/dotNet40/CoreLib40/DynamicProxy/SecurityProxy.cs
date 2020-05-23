#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Reflection;
using Library40.EventsArgs;
using Library40.Helpers;
using Library40.Internals;

namespace Library40.DynamicProxy
{
	/// <summary>
	///     Test proxy invocation handler which is used to check a methods security before invoking the method
	/// </summary>
	public class SecurityProxy : IProxyInvocationHandler
	{
		private readonly Object _Obj;

		/// <summary>
		///     Class constructor
		/// </summary>
		/// <param name="obj"> Instance of object to be proxied </param>
		private SecurityProxy(Object obj)
		{
			this._Obj = obj;
		}

		#region IProxyInvocationHandler Members
		/// <summary>
		///     IProxyInvocationHandler method that gets called from within the proxy instance.
		/// </summary>
		/// <param name="proxy"> Instance of proxy </param>
		/// <param name="method"> Method instance </param>
		/// <param name="parameters"> </param>
		/// <exception cref="Exception"></exception>
		Object IProxyInvocationHandler.Invoke(Object proxy, MethodInfo method, Object[] parameters)
		{
			Object retVal = null;
			var e = new ItemActingEventArgs<MethodCallingEventItem>(new MethodCallingEventItem(proxy, method, parameters));
			this.OnMethodCalling(e);
			if (e.Handled)
				retVal = method.Invoke(this._Obj, parameters);

			return retVal;
		}
		#endregion

		public event EventHandler<ItemActingEventArgs<MethodCallingEventItem>> MethodCalling;

		protected virtual void OnMethodCalling(ItemActingEventArgs<MethodCallingEventItem> e)
		{
			this.MethodCalling.Raise(this, e);
		}

		/// <summary>
		///     Factory method to create a new proxy instance.
		/// </summary>
		/// <param name="obj"> Instance of object to be proxied </param>
		/// <param name="securityProxy"> </param>
		public static T NewInstance<T>(T obj, out SecurityProxy securityProxy)
		{
			securityProxy = new SecurityProxy(obj);
			return (T)ProxyFactory.Instance.Create(securityProxy, obj.GetType());
		}

		/// <summary>
		///     Factory method to create a new proxy instance.
		/// </summary>
		/// <param name="obj"> Instance of object to be proxied </param>
		public static T NewInstance<T>(T obj)
		{
			return (T)ProxyFactory.Instance.Create(new SecurityProxy(obj), obj.GetType());
		}
	}
}