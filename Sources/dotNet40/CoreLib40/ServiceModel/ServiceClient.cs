#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Library40.Internals;

namespace Library40.ServiceModel
{
	/// <summary>
	///     A utility class to get a service from a host
	/// </summary>
	/// <typeparam name="TContract">The service contract, usually an interface</typeparam>
	public partial class ServiceClient<TContract> : ServiceBase
	{
		public ServiceClient([NotNull] string uri)
			: this(uri, DefaultBinding)
		{
		}

		public ServiceClient([NotNull] string uri, object callbackObject = null)
			: this(uri, DefaultBinding, callbackObject)
		{
		}

		public ServiceClient([NotNull] string uri, [NotNull] Binding binding, object callbackObject = null)
		{
			if (uri == null)
				throw new ArgumentNullException("uri");
			if (binding == null)
				throw new ArgumentNullException("binding");
			this.Binding = binding;
			this.Endpoint = new EndpointAddress(uri);
			this.ChannelFactory = callbackObject == null
				? new ChannelFactory<TContract>(this.Binding, this.Endpoint)
				: new DuplexChannelFactory<TContract>(callbackObject, this.Binding, this.Endpoint);
		}

		public EndpointAddress Endpoint { get; private set; }

		public ChannelFactory<TContract> ChannelFactory { get; private set; }

		public Binding Binding { get; private set; }

		public TContract GetChannel()
		{
			return this.ChannelFactory.CreateChannel();
		}
	}
}