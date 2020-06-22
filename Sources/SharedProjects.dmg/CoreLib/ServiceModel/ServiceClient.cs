using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Mohammad.Helpers;

namespace Mohammad.ServiceModel
{
    /// <summary>
    ///     A utility class to get a service from a host
    /// </summary>
    /// <typeparam name="TContract"> The service contract, usually an interface </typeparam>
    public partial class ServiceClient<TContract>
    {
        public EndpointAddress Endpoint { get; }
        public ChannelFactory<TContract> ChannelFactory { get; }
        public Binding Binding { get; }

        public ServiceClient(string uri)
            : this(uri, WcfHelper.DefaultBinding) {}

        public ServiceClient(string uri, object callbackObject = null, TimeSpan? sendTimeout = null)
            : this(uri, WcfHelper.DefaultBinding, callbackObject, sendTimeout) {}

        public ServiceClient(string uri, Binding binding, object callbackObject = null, TimeSpan? sendTimeout = null)
        {
            if (uri == null)
            {
                var serviceConfigs = typeof(TContract).GetCustomAttributes(typeof(ServiceConfigAttribute), true);
                if (serviceConfigs.Length > 0)
                    uri = serviceConfigs[0].As<ServiceConfigAttribute>().ServiceUrl;
                if (uri == null)
                    throw new ArgumentNullException(nameof(uri));
            }
            if (binding == null)
                throw new ArgumentNullException(nameof(binding));
            this.Binding = binding;
            if (sendTimeout.HasValue)
                this.Binding.SendTimeout = sendTimeout.Value;
            this.Endpoint = new EndpointAddress(uri);
            this.ChannelFactory = callbackObject == null
                ? new ChannelFactory<TContract>(this.Binding, this.Endpoint)
                : new DuplexChannelFactory<TContract>(callbackObject, this.Binding, this.Endpoint);
        }

        public TContract GetChannel() => this.ChannelFactory.CreateChannel();
        public async Task<TContract> GetChannelAsync() => await Task.Run(() => this.GetChannel());
    }
}