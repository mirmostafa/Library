using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using Mohammad.Net;
using Mohammad.ServiceModel;
using Mohammad.Threading.Tasks;

// ReSharper disable AssignNullToNotNullAttribute

namespace Mohammad.Helpers
{
    public static class WcfHelper
    {
        /// <summary>
        ///     A simple default binding = NetTcpBinding
        /// </summary>
        public static NetTcpBinding DefaultBinding
            =>
                new NetTcpBinding
                {
                    Security = {Mode = SecurityMode.None},
                    OpenTimeout = TimeSpan.FromMinutes(2),
                    CloseTimeout = TimeSpan.FromMinutes(2),
                    ReceiveTimeout = TimeSpan.FromMinutes(15),
                    SendTimeout = TimeSpan.FromMinutes(15),
                    TransactionFlow = false,
                    TransferMode = TransferMode.Buffered,
                    TransactionProtocol = TransactionProtocol.OleTransactions,
                    HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                    ListenBacklog = 100,
                    MaxBufferPoolSize = 2147483647,
                    MaxBufferSize = 2147483647,
                    MaxConnections = 1000,
                    MaxReceivedMessageSize = 2147483647
                };

        /// <summary>
        ///     Gets the basic HTTP binding.
        /// </summary>
        /// <value>The basic HTTP binding.</value>
        public static BasicHttpBinding BasicHttpBinding => new BasicHttpBinding {Security = {Mode = BasicHttpSecurityMode.None}};

        /// <summary>
        ///     Gets the ws HTTP binding.
        /// </summary>
        /// <value>The ws HTTP binding.</value>
        public static WSHttpBinding WsHttpBinding => new WSHttpBinding {Security = {Mode = SecurityMode.None}};

        public static string CreateUri<TContract>(string host = null, string urlSufix = null)
        {
            if (host.IsNullOrEmpty())
                host = Dns.GetLocalIp();
            if (!urlSufix.IsNullOrEmpty())
                return new UriBuilder {Scheme = Uri.UriSchemeNetTcp, Host = host, Fragment = urlSufix}.ToString();
            var customAttributes = typeof(TContract).GetCustomAttributes(typeof(ServiceConfigAttribute), false);
            if (customAttributes.Length == 0)
                throw new Exception("ServiceConfigAttribute not found.");
            var serviceUrl = customAttributes[0].As<ServiceConfigAttribute>().ServiceUrl;
            return !host.IsNullOrEmpty() ? new UriBuilder(serviceUrl) {Host = host}.ToString() : new UriBuilder(serviceUrl).ToString();
        }

        public static TContract GetServiceChannel<TContract>(string host = null, string urlSufix = null)
            => ServiceClient<TContract>.GetChannel(CreateUri<TContract>(host, urlSufix));

        public static async Task<TContract> GetServiceChannelAsync<TContract>(string host = null, string urlSufix = null)
            => await Async.Run(() => GetServiceChannel<TContract>(host, urlSufix));

        public static ServiceHost<TContract, TService> GetNetTcpServiceHost<TContract, TService>(string uri = null, TService singletonServiceInstance = null)
            where TService : class, TContract
        {
            var result = new ServiceHost<TContract, TService>(CreateUri<TContract>("0.0.0.0"), singletonServiceInstance: singletonServiceInstance);
            foreach (var serviceThrottlingBehavior in result.Description.Behaviors.OfType<ServiceThrottlingBehavior>())
            {
                serviceThrottlingBehavior.MaxConcurrentSessions = 200;
                serviceThrottlingBehavior.MaxConcurrentInstances = 100000;
                serviceThrottlingBehavior.MaxConcurrentCalls = 100;
            }
            return result;
        }
    }
}