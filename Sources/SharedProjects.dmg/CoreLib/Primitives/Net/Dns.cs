using System.Collections.Generic;
using System.Linq;
using System.Net;
using Mohammad.Dynamic;
using static Mohammad.Helpers.CodeHelper;

namespace Mohammad.Net
{
    public static class Dns
    {
        private static dynamic _Cache = new Expando();
        private static readonly Dictionary<string, string> _DnsTable = new Dictionary<string, string>();
        public static IEnumerable<KeyValuePair<string, string>> DntTable => Lock(() => _DnsTable, _DnsTable).AsEnumerable();

        public static IEnumerable<IpAddress> GetIpAddress(string name)
        {
            lock (_DnsTable)
            {
                if (_DnsTable.ContainsValue(name.ToLower()))
                    return _DnsTable.Where(p => p.Value.Equals(name)).Select(p => IpAddress.Parse(p.Key));
                foreach (var ipAddress in System.Net.Dns.GetHostEntry(name).AddressList.ExcludeV6())
                    _DnsTable.Add(ipAddress.ToString(), name);
                return _DnsTable.Where(p => p.Value.Equals(name)).Select(p => IpAddress.Parse(p.Key));
            }
        }

        private static IEnumerable<IPAddress> ExcludeV6(this IEnumerable<IPAddress> addresses)
            => addresses.Where(ip => !ip.IsIPv4MappedToIPv6 && !ip.IsIPv6LinkLocal && !ip.IsIPv6Multicast && !ip.IsIPv6SiteLocal && !ip.IsIPv6Teredo);

        public static IEnumerable<IpAddress> GetLocalV4Ips() => InnerGetAllIps().ExcludeV6().Where(ip => ip.ToString() != "::1").Select(IpAddress.Parse);

        public static IpAddress GetIpAddressAddress() => IpAddress.Parse(GetLocalIp());
        public static string GetLocalName() => _Cache.LocalName ?? (_Cache.LocalName = System.Net.Dns.GetHostName());
        public static string GetLocalIp() => GetLocalIpAddress()?.ToString();
        public static IpAddress GetLocalIpAddress() => _Cache.LocalIp ?? (_Cache.LocalIp = GetLocalV4Ips().First());
        public static IEnumerable<IpAddress> GetAllLocalIps() => InnerGetAllIps().Select(IpAddress.Parse);

        private static IEnumerable<IPAddress> InnerGetAllIps()
            => _Cache.InnerGetAllIps ?? (_Cache.InnerGetAllIps = System.Net.Dns.GetHostAddresses(GetLocalName()).Distinct());

        public static string GetNameByIp(IpAddress ip)
        {
            lock (_DnsTable)
            {
                if (!_DnsTable.ContainsKey(ip.ToString()))
                    _DnsTable.Add(ip.ToString(), CatchFunc(() => System.Net.Dns.GetHostEntry(ip))?.HostName);
                return _DnsTable[ip.ToString()];
            }
        }

        public static string GetNameByIp(string ip)
        {
            lock (_DnsTable)
            {
                if (!_DnsTable.ContainsKey(ip))
                    _DnsTable.Add(ip, CatchFunc(() => System.Net.Dns.GetHostEntry(ip))?.HostName);
                return _DnsTable[ip];
            }
        }

        public static void ResetCache() { _Cache = new Expando(); }
    }
}