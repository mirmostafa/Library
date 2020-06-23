// Created on     2018/07/23


using System.Net.NetworkInformation;

namespace Mohammad.Net
{
    public class ResolveAndPingResult
    {
        public IpAddress Ip { get; }
        public IPStatus PingStatus { get; }
        public string MachineName { get; }

        internal ResolveAndPingResult(IpAddress ip, IPStatus pingStatus, string machineName)
        {
            this.Ip = ip;
            this.PingStatus = pingStatus;
            this.MachineName = machineName;
        }
    }
}