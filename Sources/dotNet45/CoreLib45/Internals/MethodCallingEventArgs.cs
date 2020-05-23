#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Reflection;

namespace Mohammad.Internals
{
    public class MethodCallingEventItem
    {
        public object Proxy { get; }
        public MethodInfo Method { get; }
        public object[] Parameters { get; }

        public MethodCallingEventItem(object proxy, MethodInfo method, object[] parameters)
        {
            this.Proxy = proxy;
            this.Method = method;
            this.Parameters = parameters;
        }
    }
}