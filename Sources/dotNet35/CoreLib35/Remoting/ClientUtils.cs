#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;

namespace Library35.Remoting
{
	public static class ClientUtils
	{
		public static TContract GetClassHttp<TContract>(string computername, int portNo, string remotingName)
		{
			return GetClassHttp<TContract>(computername, portNo, remotingName, false);
		}

		public static TContract GetClassHttp<TContract>(string computername, int portNo, string remotingName, bool ensureSecurity)
		{
			return GetClassHttp<TContract>(string.Format("http://{0}:{1}/{2}", computername, portNo, remotingName), portNo, ensureSecurity);
		}

		public static TContract GetClassHttp<TContract>(string url, int portNo, bool ensureSecurity)
		{
			var channel = new HttpChannel();
			ChannelServices.RegisterChannel(channel, ensureSecurity);
			return (TContract)Activator.GetObject(typeof (TContract), url);
		}
	}
}