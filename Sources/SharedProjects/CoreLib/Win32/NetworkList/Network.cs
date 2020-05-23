using System;
using Mohammad.Win32.Interop.NetworkList;

namespace Mohammad.Win32.NetworkList
{
    /// <summary>
    ///     Represents a network on the local machine.
    ///     It can also represent a collection of network
    ///     connections with a similar network signature.
    /// </summary>
    /// <remarks>
    ///     Instances of this class are obtained by calling
    ///     methods on the <see cref="NetworkListManager" /> class.
    /// </remarks>
    public class Network
    {
        private readonly INetwork network;

        /// <summary>
        ///     Gets or sets the category of a network. The
        ///     categories are trusted, untrusted, or
        ///     authenticated.
        /// </summary>
        /// <value>
        ///     A <see cref="NetworkCategory" /> value.
        /// </value>
        public NetworkCategory Category { get { return this.network.GetCategory(); } set { this.network.SetCategory(value); } }

        /// <summary>
        ///     Gets the local date and time when the network
        ///     was connected.
        /// </summary>
        /// <value>
        ///     A <see cref="System.DateTime" /> object.
        /// </value>
        public DateTime ConnectedTime
        {
            get
            {
                uint low, high, dummy1, dummy2;
                this.network.GetTimeCreatedAndConnected(out dummy1, out dummy2, out low, out high);
                long time = high;
                // Shift the day info into the high order bits.
                time <<= 32;
                time |= low;
                return DateTime.FromFileTimeUtc(time);
            }
        }

        /// <summary>
        ///     Gets the network connections for the network.
        /// </summary>
        /// <value>
        ///     A <see cref="NetworkConnectionCollection" /> object.
        /// </value>
        public NetworkConnectionCollection Connections { get { return new NetworkConnectionCollection(this.network.GetNetworkConnections()); } }

        /// <summary>
        ///     Gets the connectivity state of the network.
        /// </summary>
        /// <value>
        ///     A <see cref="Connectivity" /> value.
        /// </value>
        /// <remarks>
        ///     Connectivity provides information on whether
        ///     the network is connected, and the protocols
        ///     in use for network traffic.
        /// </remarks>
        public Connectivity Connectivity { get { return this.network.GetConnectivity(); } }

        /// <summary>
        ///     Gets the local date and time when the
        ///     network was created.
        /// </summary>
        /// <value>
        ///     A <see cref="System.DateTime" /> object.
        /// </value>
        public DateTime CreatedTime
        {
            get
            {
                uint low, high, dummy1, dummy2;
                this.network.GetTimeCreatedAndConnected(out low, out high, out dummy1, out dummy2);
                long time = high;
                //Shift the value into the high order bits.
                time <<= 32;
                time |= low;
                return DateTime.FromFileTimeUtc(time);
            }
        }

        /// <summary>
        ///     Gets or sets a description for the network.
        /// </summary>
        /// <value>
        ///     A <see cref="System.String" /> value.
        /// </value>
        public string Description { get { return this.network.GetDescription(); } set { this.network.SetDescription(value); } }

        /// <summary>
        ///     Gets the domain type of the network.
        /// </summary>
        /// <value>
        ///     A <see cref="DomainType" /> value.
        /// </value>
        /// <remarks>
        ///     The domain
        ///     indictates whether the network is an Active
        ///     Directory Network, and whether the machine
        ///     has been authenticated by Active Directory.
        /// </remarks>
        public DomainType DomainType { get { return this.network.GetDomainType(); } }

        /// <summary>
        ///     Gets a value that indicates whether there is
        ///     network connectivity.
        /// </summary>
        /// <value>
        ///     A <see cref="System.Boolean" /> value.
        /// </value>
        public bool IsConnected { get { return this.network.IsConnected; } }

        /// <summary>
        ///     Gets a value that indicates whether there is
        ///     Internet connectivity.
        /// </summary>
        /// <value>
        ///     A <see cref="System.Boolean" /> value.
        /// </value>
        public bool IsConnectedToInternet { get { return this.network.IsConnectedToInternet; } }

        /// <summary>
        ///     Gets or sets the name of the network.
        /// </summary>
        /// <value>
        ///     A <see cref="System.String" /> value.
        /// </value>
        public string Name { get { return this.network.GetName(); } set { this.network.SetName(value); } }

        /// <summary>
        ///     Gets a unique identifier for the network.
        /// </summary>
        /// <value>
        ///     A <see cref="System.Guid" /> value.
        /// </value>
        public Guid NetworkId { get { return this.network.GetNetworkId(); } }

        internal Network(INetwork network) { this.network = network; }
    }
}