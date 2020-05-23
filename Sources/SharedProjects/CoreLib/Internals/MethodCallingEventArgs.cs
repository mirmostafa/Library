using System.Reflection;

namespace Mohammad.Internals
{
    public class MethodCallingEventItem
    {
        public object Proxy { get; private set; }
        public MethodInfo Method { get; private set; }
        public object[] Parameters { get; private set; }

        public MethodCallingEventItem(object proxy, MethodInfo method, object[] parameters)
        {
            this.Proxy = proxy;
            this.Method = method;
            this.Parameters = parameters;
        }
    }
}