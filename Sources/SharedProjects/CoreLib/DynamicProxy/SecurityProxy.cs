using System;
using System.Reflection;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.Internals;

namespace Mohammad.DynamicProxy
{
    /// <summary>
    ///     Test proxy invocation handler which is used to check a methods security before invoking the method
    /// </summary>
    public class SecurityProxy : IProxyInvocationHandler
    {
        private readonly object _Obj;

        /// <summary>
        ///     Class constructor
        /// </summary>
        /// <param name="obj"> Instance of object to be proxied </param>
        private SecurityProxy(object obj) { this._Obj = obj; }

        public event EventHandler<ItemActingEventArgs<MethodCallingEventItem>> MethodCalling;
        protected virtual void OnMethodCalling(ItemActingEventArgs<MethodCallingEventItem> e) { this.MethodCalling.Raise(this, e); }

        /// <summary>
        ///     Factory method to create a new proxy instance.
        /// </summary>
        /// <param name="obj"> Instance of object to be proxied </param>
        /// <param name="securityProxy"> </param>
        public static T NewInstance<T>(T obj, out SecurityProxy securityProxy)
        {
            securityProxy = new SecurityProxy(obj);
            return (T) ProxyFactory.Instance.Create(securityProxy, obj.GetType());
        }

        /// <summary>
        ///     Factory method to create a new proxy instance.
        /// </summary>
        /// <param name="obj"> Instance of object to be proxied </param>
        public static T NewInstance<T>(T obj) { return (T) ProxyFactory.Instance.Create(new SecurityProxy(obj), obj.GetType()); }

        /// <summary>
        ///     IProxyInvocationHandler method that gets called from within the proxy instance.
        /// </summary>
        /// <param name="proxy"> Instance of proxy </param>
        /// <param name="method"> Method instance </param>
        /// <param name="parameters"> </param>
        /// <exception cref="Exception"></exception>
        object IProxyInvocationHandler.Invoke(object proxy, MethodInfo method, object[] parameters)
        {
            object retVal = null;
            var e = new ItemActingEventArgs<MethodCallingEventItem>(new MethodCallingEventItem(proxy, method, parameters));
            this.OnMethodCalling(e);
            if (e.Handled)
                retVal = method.Invoke(this._Obj, parameters);

            return retVal;
        }
    }
}