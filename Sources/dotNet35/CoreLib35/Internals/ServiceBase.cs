#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.ServiceModel;

namespace Library35.Internals
{
	public abstract class ServiceBase
	{
		/// <summary>
		///     A simple default binding = NetTcpBinding
		/// </summary>
		public static NetTcpBinding DefaultBinding
		{
			get
			{
				return new NetTcpBinding
				       {
					       Security =
					       {
						       Mode = SecurityMode.None
					       }
				       };
			}
		}

		/// <summary>
		///     Gets the basic HTTP binding.
		/// </summary>
		/// <value> The basic HTTP binding. </value>
		public static BasicHttpBinding BasicHttpBinding
		{
			get
			{
				return new BasicHttpBinding
				       {
					       Security =
					       {
						       Mode = BasicHttpSecurityMode.None
					       }
				       };
			}
		}

		/// <summary>
		///     Gets the ws HTTP binding.
		/// </summary>
		/// <value> The ws HTTP binding. </value>
		public static WSHttpBinding WsHttpBinding
		{
			get
			{
				return new WSHttpBinding
				       {
					       Security =
					       {
						       Mode = SecurityMode.None
					       }
				       };
			}
		}
	}
}