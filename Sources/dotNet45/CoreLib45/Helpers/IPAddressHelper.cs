using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Mohammad.Net;

namespace Mohammad.Helpers
{
    // ReSharper disable once InconsistentNaming
    public static class IPAddressHelper
    {
        private static readonly IPAddress _Empty = IPAddress.Parse("0.0.0.0");
        private static readonly IPAddress _IntranetMask1 = IPAddress.Parse("10.255.255.255");
        private static readonly IPAddress _IntranetMask2 = IPAddress.Parse("172.16.0.0");
        private static readonly IPAddress _IntranetMask3 = IPAddress.Parse("172.31.255.255");
        private static readonly IPAddress _IntranetMask4 = IPAddress.Parse("192.168.255.255");

        public static IPAddress And(this IPAddress ipAddress, IPAddress mask)
        {
            //CheckIpVersion(ipAddress, mask, out var addressBytes, out var maskBytes);
            var (addressBytes, maskBytes) = CheckIpVersion(ipAddress, mask);

            var resultBytes = new byte[addressBytes.Length];
            for (var i = 0; i < addressBytes.Length; ++i)
            {
                resultBytes[i] = (byte)(addressBytes[i] & maskBytes[i]);
            }

            return new IPAddress(resultBytes);
        }

        public static bool HasConflict(this IEnumerable<IpAddress> ipAddresses1, IEnumerable<IpAddress> ipAddresses) =>
            ipAddresses.Any(ip => ip.IsInRange(ipAddresses1));

        /// <summary>
        ///     Returns true if the IP address is one of the following
        ///     IANA-reserved private IPv4 network ranges (from http://en.wikipedia.org/wiki/IP_address)
        ///     Start 	      End
        ///     10.0.0.0 	  10.255.255.255
        ///     172.16.0.0 	  172.31.255.255
        ///     192.168.0.0   192.168.255.255
        /// </summary>
        /// <returns></returns>
        public static bool IsOnIntranet(this IPAddress ipAddress)
        {
            if (_Empty.Equals(ipAddress))
            {
                return false;
            }

            var onIntranet = IPAddress.IsLoopback(ipAddress);
            onIntranet = onIntranet || ipAddress.Equals(ipAddress.And(_IntranetMask1));
            onIntranet = onIntranet || ipAddress.Equals(ipAddress.And(_IntranetMask4));

            onIntranet = onIntranet || _IntranetMask2.Equals(ipAddress.And(_IntranetMask2)) && ipAddress.Equals(ipAddress.And(_IntranetMask3));

            return onIntranet;
        }

        private static (byte[] addressBytes, byte[] maskBytes) CheckIpVersion(IPAddress ipAddress, IPAddress mask)
        {
            if (mask == null)
            {
                throw new ArgumentException();
            }

            var addressBytes = ipAddress.GetAddressBytes();
            var maskBytes = mask.GetAddressBytes();

            if (addressBytes.Length != maskBytes.Length)
            {
                throw new ArgumentException("The address and mask don't use the same IP standard");
            }

            return (addressBytes, maskBytes);
        }
    }
}