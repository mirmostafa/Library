#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.ServiceModel.Channels;
using Library40.Internals;

namespace Library40.ServiceModel
{
	public partial class ServiceClient<TContract>
	{
		public static TContract GetChannel([NotNull] string uri)
		{
			return GetChannel(uri, DefaultBinding);
		}

		public static TContract GetChannel([NotNull] string uri, [NotNull] Binding binding)
		{
			return new ServiceClient<TContract>(uri, binding).GetChannel();
		}

		public static TContract GetChannel([NotNull] string uri, [NotNull] object callbackObject)
		{
			return GetChannel(uri, DefaultBinding, callbackObject);
		}

		public static TContract GetChannel([NotNull] string uri, [NotNull] Binding binding, [NotNull] object callbackObject)
		{
			return new ServiceClient<TContract>(uri, binding, callbackObject).GetChannel();
		}
	}
}