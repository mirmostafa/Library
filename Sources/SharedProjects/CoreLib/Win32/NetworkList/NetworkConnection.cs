using System;
using Mohammad.Win32.Interop.NetworkList;

namespace Mohammad.Win32.NetworkList
{
    /// <summary>
    ///     Represents a connection to a network.
    /// </summary>
    /// <remarks>
    ///     A collection containing instances of this class is obtained by calling
    ///     the Microsoft.WindowsAPICodePack.Net.Network.Connections property.
    /// </remarks>
    public class NetworkConnection
    {
        private readonly INetworkConnection networkConnection;

        /// <summary>
        ///     Retrieves an object that represents the network
        ///     associated with this connection.
        /// </summary>
        /// <returns>
        ///     A <see cref="Network" /> object.
        /// </returns>
        public Network Network { get { return new Network(this.networkConnection.GetNetwork()); } }

        /// <summary>
        ///     Gets the adapter identifier for this connection.
        /// </summary>
        /// <value>
        ///     A <see cref="System.Guid" /> object.
        /// </value>
        public Guid AdapterId { get { return this.networkConnection.GetAdapterId(); } }

        /// <summary>
        ///     Gets the unique identifier for this connection.
        /// </summary>
        /// <value>
        ///     A <see cref="System.Guid" /> object.
        /// </value>
        public Guid ConnectionId { get { return this.networkConnection.GetConnectionId(); } }

        /// <summary>
        ///     Gets a value that indicates the connectivity of this connection.
        /// </summary>
        /// <value>
        ///     A <see cref="Connectivity" /> value.
        /// </value>
        public Connectivity Connectivity { get { return this.networkConnection.GetConnectivity(); } }

        /// <summary>
        ///     Gets a value that indicates whether the network associated
        ///     with this connection is
        ///     an Active Directory network and whether the machine
        ///     has been authenticated by Active Directory.
        /// </summary>
        /// <value>
        ///     A <see cref="DomainType" /> value.
        /// </value>
        public DomainType DomainType { get { return this.networkConnection.GetDomainType(); } }

        /// <summary>
        ///     Gets a value that indicates whether this
        ///     connection has Internet access.
        /// </summary>
        /// <value>
        ///     A <see cref="System.Boolean" /> value.
        /// </value>
        public bool IsConnectedToInternet { get { return this.networkConnection.IsConnectedToInternet; } }

        /// <summary>
        ///     Gets a value that indicates whether this connection has
        ///     network connectivity.
        /// </summary>
        /// <value>
        ///     A <see cref="System.Boolean" /> value.
        /// </value>
        public bool IsConnected { get { return this.networkConnection.IsConnected; } }

        internal NetworkConnection(INetworkConnection networkConnection) { this.networkConnection = networkConnection; }
    }
}