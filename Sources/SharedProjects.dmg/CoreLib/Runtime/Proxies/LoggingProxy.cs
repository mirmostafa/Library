using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Mohammad.Data.BusinessTools.EventsArgs;
using Mohammad.EventsArgs;
using Mohammad.Helpers;

namespace Mohammad.Runtime.Proxies
{
    public class LoggingProxy<T> : RealProxy
        where T : MarshalByRefObject
    {
        private readonly T _Instance;

        protected LoggingProxy(T instance)
            : base(typeof(T)) => this._Instance = instance;

        public static T Create(T instance, Action<InvokeEventArgs> onInvoked = null, Action<InvokeEventArgs> onInvoking = null,
            Action<T, ItemActedEventArgs<Exception>> onExceptionOccurred = null)
        {
            var proxy = new LoggingProxy<T>(instance);
            if (onInvoked != null)
                proxy.Invoked += (s, e) => onInvoked(e.Item);
            if (onInvoking != null)
                proxy.Invoking += (s, e) => onInvoking(e.Item);
            if (onExceptionOccurred != null)
                proxy.ExceptionOccurred += (s, e) => onExceptionOccurred(e.As<T>(), e);
            return (T)proxy.GetTransparentProxy();
        }

        public override IMessage Invoke(IMessage msg)
        {
            try
            {
                var methodCall = (IMethodCallMessage)msg;
                var method = (MethodInfo)methodCall.MethodBase;

                var args = new EntityActing<InvokeEventArgs>(InvokeEventArgs.Create(this._Instance, methodCall));
                this.OnInvoking(args);
                if (args.Handled)
                    return null;
                var result = method.Invoke(this._Instance, methodCall.InArgs);
                this.OnInvoked(new EntityActed<InvokeEventArgs>(InvokeEventArgs.Create(this._Instance, methodCall)));
                return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
            }
            catch (Exception e)
            {
                this.OnExceptionOccurred(new ItemActedEventArgs<Exception>(e));
                if (e is TargetInvocationException && e.InnerException != null)
                    return new ReturnMessage(e.InnerException, (IMethodCallMessage)msg);

                return new ReturnMessage(e, (IMethodCallMessage)msg);
            }
        }

        public event EventHandler<ItemActedEventArgs<InvokeEventArgs>> Invoked;
        public event EventHandler<ItemActedEventArgs<Exception>> ExceptionOccurred;
        public event EventHandler<ItemActingEventArgs<InvokeEventArgs>> Invoking;

        protected virtual void OnInvoked(ItemActedEventArgs<InvokeEventArgs> e) { this.Invoked?.Invoke(this._Instance, e); }
        protected virtual void OnExceptionOccurred(ItemActedEventArgs<Exception> e) { this.ExceptionOccurred?.Invoke(this._Instance, e); }
        protected virtual void OnInvoking(ItemActingEventArgs<InvokeEventArgs> e) { this.Invoking?.Invoke(this._Instance, e); }

        public class InvokeEventArgs : EventArgs
        {
            public MethodInfo Method { get; }
            public string MethodName { get; }
            public IEnumerable<(string Name, object Value)> Arguments { get; }
            public T Instance { get; }

            public InvokeEventArgs(T instance, MethodInfo method, string methodName, IEnumerable<(string Name, object Value)> arguments)
            {
                this.Instance = instance;
                this.Method = method;
                this.Arguments = arguments;
                this.MethodName = methodName;
            }

            public static InvokeEventArgs Create(T instance, IMethodCallMessage msg) => new InvokeEventArgs(instance,
                (MethodInfo)msg.MethodBase,
                msg.MethodName,
                EnumerableHelper.For(msg.ArgCount, index => (msg.GetArgName(index), msg.GetArg(index))));
        }
    }
}