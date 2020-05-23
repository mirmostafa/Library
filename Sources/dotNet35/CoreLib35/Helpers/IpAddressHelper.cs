#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Library35.Net;

namespace Library35.Helpers
{
	public static class IpAddressHelper
	{
		private static readonly IPAddress _Empty = IPAddress.Parse("0.0.0.0");
		private static readonly IPAddress _IntranetMask1 = IPAddress.Parse("10.255.255.255");
		private static readonly IPAddress _IntranetMask2 = IPAddress.Parse("172.16.0.0");
		private static readonly IPAddress _IntranetMask3 = IPAddress.Parse("172.31.255.255");
		private static readonly IPAddress _IntranetMask4 = IPAddress.Parse("192.168.255.255");

		public static bool HasConflict(this IEnumerable<IpAddress> ipAddresses1, IEnumerable<IpAddress> ipAddresses)
		{
			return ipAddresses.Any(ip => ip.IsInRange(ipAddresses1));
		}

		private static void CheckIpVersion(IPAddress ipAddress, IPAddress mask, out byte[] addressBytes, out byte[] maskBytes)
		{
			if (mask == null)
				throw new ArgumentException();

			addressBytes = ipAddress.GetAddressBytes();
			maskBytes = mask.GetAddressBytes();

			if (addressBytes.Length != maskBytes.Length)
				throw new ArgumentException("The address and mask don't use the same IP standard");
		}

		public static IPAddress And(this IPAddress ipAddress, IPAddress mask)
		{
			byte[] addressBytes;
			byte[] maskBytes;
			CheckIpVersion(ipAddress, mask, out addressBytes, out maskBytes);

			var resultBytes = new byte[addressBytes.Length];
			for (var i = 0; i < addressBytes.Length; ++i)
				resultBytes[i] = (byte)(addressBytes[i] & maskBytes[i]);

			return new IPAddress(resultBytes);
		}

		/// <summary>
		///     Retuns true if the ip address is one of the following
		///     IANA-reserved private IPv4 network ranges (from http://en.wikipedia.org/wiki/IP_address)
		///     Start 	      End
		///     10.0.0.0 	    10.255.255.255
		///     172.16.0.0 	  172.31.255.255
		///     192.168.0.0   192.168.255.255
		/// </summary>
		/// <returns> </returns>
		public static bool IsOnIntranet(this IPAddress ipAddress)
		{
			if (_Empty.Equals(ipAddress))
				return false;
			var onIntranet = IPAddress.IsLoopback(ipAddress);
			onIntranet = onIntranet || ipAddress.Equals(ipAddress.And(_IntranetMask1)); //10.255.255.255
			onIntranet = onIntranet || ipAddress.Equals(ipAddress.And(_IntranetMask4)); ////192.168.255.255

			onIntranet = onIntranet || (_IntranetMask2.Equals(ipAddress.And(_IntranetMask2)) && ipAddress.Equals(ipAddress.And(_IntranetMask3)));

			return onIntranet;
		}
	}
}