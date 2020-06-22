using System;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Mohammad.Helpers;

namespace Mohammad.ServiceModel
{
    public partial class ServiceClient<TContract>
    {
        public static TContract GetChannelByServiceConfig()
        {
            string uri = null;
            var serviceConfigs = typeof(TContract).GetCustomAttributes(typeof(ServiceConfigAttribute), true);
            if (serviceConfigs.Length > 0)
                uri = serviceConfigs[0].As<ServiceConfigAttribute>().ServiceUrl;

            return GetChannel(uri);
        }

        public static async Task<TContract> GetChannelByServiceConfigAsync() { return await Task.Run(() => GetChannelByServiceConfig()); }
        public static TContract GetChannel(string uri, TimeSpan? sendTimeout = null) { return GetChannel(uri, WcfHelper.DefaultBinding, sendTimeout); }

        public static async Task<TContract> GetChannelAsync(string uri, TimeSpan? sendTimeout = null)
        {
            return await Task.Run(() => GetChannel(uri, WcfHelper.DefaultBinding, sendTimeout));
        }

        public static TContract GetChannel(string uri, Binding binding, TimeSpan? sendTimeout = null)
        {
            return new ServiceClient<TContract>(uri, binding, sendTimeout: sendTimeout).GetChannel();
        }

        public static TContract GetChannel(string uri, object callbackObject, TimeSpan? sendTimeout = null)
        {
            return GetChannel(uri, WcfHelper.DefaultBinding, callbackObject, sendTimeout);
        }

        public static TContract GetChannel(string uri, Binding binding, object callbackObject, TimeSpan? sendTimeout = null)
        {
            return new ServiceClient<TContract>(uri, binding, callbackObject, sendTimeout).GetChannel();
        }
    }
}