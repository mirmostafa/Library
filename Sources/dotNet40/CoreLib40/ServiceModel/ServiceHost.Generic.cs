#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.ServiceModel.Channels;

namespace Library40.ServiceModel
{
	/// <summary>
	///     A utility class to create a host for a service
	/// </summary>
	/// <typeparam name="TContract">The service contract, usually an interface</typeparam>
	/// <typeparam name="TService">The service, which implements the contract</typeparam>
	public class ServiceHost<TContract, TService> : ServiceHost
		where TService : class
	{
		public ServiceHost(string uri, Binding binding = null, object singletonServiceInstance = null)
			: base(typeof (TContract), typeof (TService), uri, binding, singletonServiceInstance)
		{
		}
	}
}