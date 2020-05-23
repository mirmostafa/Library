#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;

namespace Library40.Remoting
{
	public static class ServerUtils
	{
		public static void RegisterClassHttp<TRemoteObject>(int port, string objectUrl) where TRemoteObject : MarshalByRefObject
		{
			RegisterClassHttp(port, typeof (TRemoteObject), objectUrl);
		}

		public static void RegisterClassHttp(int port, Type remoteObjectType, string objectUrl)
		{
			RegisterClassHttp(port, remoteObjectType.AssemblyQualifiedName, objectUrl);
		}

		public static void RegisterClassHttp(int port, string typeName, string objectUrl)
		{
			RegisterClassHttp(port, false, typeName, objectUrl);
		}

		public static void RegisterClassHttp(int port, bool ensureSecurity, string typeName, string objectUrl)
		{
			RegisterClassHttp(port, ensureSecurity, typeName, objectUrl, WellKnownObjectMode.Singleton);
		}

		public static void RegisterClassHttp(int port, bool ensureSecurity, string typeName, string objectUrl, WellKnownObjectMode mode)
		{
			var channel = new HttpChannel(port);

			ChannelServices.RegisterChannel(channel, ensureSecurity);
			RemotingConfiguration.RegisterWellKnownServiceType(Type.GetType(typeName), objectUrl, mode);
		}
	}
}