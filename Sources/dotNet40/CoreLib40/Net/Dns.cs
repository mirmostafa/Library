#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Linq;

namespace Library40.Net
{
	public class Dns
	{
		public static IpAddress GetHostIpAddress()
		{
			return IpAddress.Parse(System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.Last().ToString());
		}

		public static string GetHostName()
		{
			return System.Net.Dns.GetHostName();
		}

		public static IpAddress GetIpAddress(string name)
		{
			var entries = System.Net.Dns.GetHostEntry(name).AddressList;
			return entries.Any() ? IpAddress.Parse(entries.Last().ToString()) : null;
		}
	}
}