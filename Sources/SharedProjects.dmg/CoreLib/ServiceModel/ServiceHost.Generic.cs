using System.ServiceModel.Channels;

namespace Mohammad.ServiceModel
{
    /// <summary>
    ///     A utility class to create a host for a service
    /// </summary>
    /// <typeparam name="TContract">The service contract, usually an interface</typeparam>
    /// <typeparam name="TService">The service, which implements the contract</typeparam>
    public class ServiceHost<TContract, TService> : ServiceHost
        where TService : class, TContract
    {
        public ServiceHost(string uri = null, Binding binding = null, TService singletonServiceInstance = null)
            : base(typeof(TContract), typeof(TService), uri, binding, singletonServiceInstance) {}

        public static ServiceHost<TContract, TService> CreateInstance(string uri = null, Binding binding = null, TService singletonServiceInstance = null)
        {
            return new ServiceHost<TContract, TService>(uri, binding, singletonServiceInstance);
        }
    }
}