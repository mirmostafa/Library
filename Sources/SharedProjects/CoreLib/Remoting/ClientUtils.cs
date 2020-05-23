using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;

namespace Mohammad.Remoting
{
    public static class ClientUtils
    {
        public static TContract GetClassHttp<TContract>(string computername, int portNo, string remotingName)
            => GetClassHttp<TContract>(computername, portNo, remotingName, false);

        public static TContract GetClassHttp<TContract>(string computername, int portNo, string remotingName, bool ensureSecurity)
            => GetClassHttp<TContract>($"http://{computername}:{portNo}/{remotingName}", portNo, ensureSecurity);

        public static TContract GetClassHttp<TContract>(string url, int portNo, bool ensureSecurity)
        {
            var channel = new HttpChannel();
            ChannelServices.RegisterChannel(channel, ensureSecurity);
            return (TContract) Activator.GetObject(typeof(TContract), url);
        }
    }
}