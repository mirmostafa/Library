#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Library35.Internals;

namespace Library35.ServiceModel
{
	/// <summary>
	///     A utility class to get a service from a host
	/// </summary>
	/// <typeparam name="TContract"> The service contract, usually an interface </typeparam>
	public partial class ServiceClient<TContract> : ServiceBase
	{
		public ServiceClient(string uri)
			: this(uri, DefaultBinding)
		{
		}

		public ServiceClient(string uri, object callbackObject = null)
			: this(uri, DefaultBinding, callbackObject)
		{
		}

		public ServiceClient(string uri, Binding binding, object callbackObject = null)
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