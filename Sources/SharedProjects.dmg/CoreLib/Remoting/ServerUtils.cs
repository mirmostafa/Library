using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;

namespace Mohammad.Remoting
{
    public static class ServerUtils
    {
        public static void RegisterClassHttp<TRemoteObject>(int port, string objectUrl) where TRemoteObject : MarshalByRefObject
        {
            RegisterClassHttp(port, typeof(TRemoteObject), objectUrl);
        }

        public static void RegisterClassHttp(int port, Type remoteObjectType, string objectUrl)
        {
            RegisterClassHttp(port, remoteObjectType.AssemblyQualifiedName, objectUrl);
        }

        public static void RegisterClassHttp(int port, string typeName, string objectUrl) { RegisterClassHttp(port, false, typeName, objectUrl); }

        public static void RegisterClassHttp(int port, bool ensureSecurity, string typeName, string objectUrl)
        {
            RegisterClassHttp(port, ensureSecurity, typeName, objectUrl, WellKnownObjectMode.Singleton);
        }

        public static void RegisterClassHttp(int port, bool ensureSecurity, string typeName, string objectUrl, WellKnownObjectMode mode)
        {
            if (typeName == null)
                throw new ArgumentNullException(nameof(typeName));
            var channel = new HttpChannel(port);

            ChannelServices.RegisterChannel(channel, ensureSecurity);
            RemotingConfiguration.RegisterWellKnownServiceType(Type.GetType(typeName), objectUrl, mode);
        }
    }
}