#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using Library35.Helpers;
using Library35.Internals;

namespace Library35.ServiceModel
{
	/// <summary>
	///     A utility class to create a host for a service
	/// </summary>
	public class ServiceHost : ServiceBase, IDisposable
	{
		private static readonly object _EventDisposed = new object();
		private readonly List<ServiceEndpoint> _Endpoints = new List<ServiceEndpoint>();
		private readonly System.ServiceModel.ServiceHost _Host;
		private EventHandlerList _Events;

		/// <summary>
		///     Initializes a new instance of the <see cref="ServiceHost&lt;TContract, TService&gt;" /> class.
		/// </summary>
		/// <param name="contractType">Type of the contract.</param>
		/// <param name="serviceType">Type of the service.</param>
		/// <param name="uri">The service's Uri</param>
		/// <param name="binding">The binding protocol</param>
		/// <param name="singletonServiceInstance">The instance of the hosted service.</param>
		public ServiceHost(Type contractType, Type serviceType, string uri = null, Binding binding = null, object singletonServiceInstance = null)
		{
			this.ServiceType = serviceType;
			this.ContractType = contractType;
			this._Host = singletonServiceInstance == null
				? new System.ServiceModel.ServiceHost(serviceType, new Uri(uri))
				: new System.ServiceModel.ServiceHost(singletonServiceInstance, new Uri(uri));
			if (uri.IsNullOrEmpty())
				return;
			if (binding == null)
				binding = DefaultBinding;
			this.AddServiceEndpoint(binding, uri);
		}

		/// <summary>
		/// </summary>
		public Type ServiceType { get; private set; }

		/// <summary>
		/// </summary>
		public Type ContractType { get; private set; }

		/// <summary>
		///     Get the uri, which the service is being up.
		/// </summary>
		public IEnumerable<string> Addresses
		{
			get { return this._Endpoints.Select(ep => ep.Address.ToString()); }
		}

		/// <summary>
		///     Gets the list of event handlers that are attached to this component.
		/// </summary>
		protected EventHandlerList Events
		{
			get { return this._Events ?? (this._Events = new EventHandlerList()); }
		}

		/// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		///     Adds a event handler to listen to the Disposed event on the component.
		/// </summary>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler Disposed
		{
			add { this.Events.AddHandler(_EventDisposed, value); }
			remove { this.Events.RemoveHandler(_EventDisposed, value); }
		}

		/// <summary>
		///     Adds the service endpoint.
		/// </summary>
		/// <param name="binding"> The binding. </param>
		/// <param name="uri"> The URI. </param>
		public void AddServiceEndpoint(Binding binding, string uri)
		{
			this._Endpoints.Add(this._Host.AddServiceEndpoint(this.ContractType, binding, uri));
			if ((!(binding is WSHttpBinding)))
				return;
			var wsEnabledbehavior = new ServiceMetadataBehavior
			                        {
				                        HttpGetEnabled = true,
				                        HttpGetUrl = new Uri(uri)
			                        };
			this._Host.Description.Behaviors.Add(wsEnabledbehavior);
		}

		/// <summary>
		///     Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing">
		///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
				lock (this)
				{
					if (this._Host != null)
					{
						this.Close();
						((IDisposable)this._Host).Dispose();
					}
					if (this._Events != null)
					{
						var handler = (EventHandler)this._Events[_EventDisposed];
						if (handler != null)
							handler(this, EventArgs.Empty);
					}
				}
		}

		/// <summary>
		///     Causes a communication object to transition from the created state to the opened state
		/// </summary>
		public void Open()
		{
			this._Host.Open();
		}

		/// <summary>
		///     Causes a communication object to transition from the current state to the closed state
		/// </summary>
		public void Close()
		{
			this._Host.Close();
		}
	}
}