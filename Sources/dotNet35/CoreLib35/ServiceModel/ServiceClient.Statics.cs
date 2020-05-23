#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.ServiceModel.Channels;

namespace Library35.ServiceModel
{
	public partial class ServiceClient<TContract>
	{
		public static TContract GetChannel(string uri)
		{
			return GetChannel(uri, DefaultBinding);
		}

		public static TContract GetChannel(string uri, Binding binding)
		{
			return new ServiceClient<TContract>(uri, binding).GetChannel();
		}

		public static TContract GetChannel(string uri, object callbackObject)
		{
			return GetChannel(uri, DefaultBinding, callbackObject);
		}

		public static TContract GetChannel(string uri, Binding binding, object callbackObject)
		{
			return new ServiceClient<TContract>(uri, binding, callbackObject).GetChannel();
		}
	}
}