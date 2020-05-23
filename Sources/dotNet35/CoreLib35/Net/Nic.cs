#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.Generic;
using System.Net.NetworkInformation;
using Library35.Helpers;

namespace Library35.Net
{
	public class Nic
	{
		private long _BytesReceivedLast;
		private long _BytesSentLast;

		public Nic(NetworkInterface networkInterface)
		{
			this.NetworkInterface = networkInterface;
		}

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

		public NetworkInterface NetworkInterface { get; private set; }

		public static IEnumerable<NetworkInterface> GetAllNetworkInterfaceCards()
		{
			return NetworkInterface.GetAllNetworkInterfaces();
		}
	}
}