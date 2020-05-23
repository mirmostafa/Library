using System.Collections.Generic;
using System.Net.NetworkInformation;
using Mohammad.Helpers;

namespace Mohammad.Net
{
    public class Nic
    {
        private long _BytesReceivedLast;
        private long _BytesSentLast;

        public int BytesSent
        {
            get
            {
                var interfaceStats = this.NetworkInterface.GetIPv4Statistics();
                var result = interfaceStats.BytesSent - this._BytesSentLast;
                this._BytesSentLast = interfaceStats.BytesSent;
                return result.ToInt() / 1024;
            }
        }

        public int DownloadSpeed
        {
            get
            {
                var interfaceStats = this.NetworkInterface.GetIPv4Statistics();
                var result = interfaceStats.BytesSent - this._BytesReceivedLast;
                this._BytesReceivedLast = interfaceStats.BytesReceived;
                return result.ToInt();
            }
        }

        public NetworkInterface NetworkInterface { get; }
        public Nic(NetworkInterface networkInterface) { this.NetworkInterface = networkInterface; }
        public static IEnumerable<NetworkInterface> GetAllNetworkInterfaceCards() => NetworkInterface.GetAllNetworkInterfaces();
    }
}