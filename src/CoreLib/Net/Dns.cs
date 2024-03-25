using System.Net.Sockets;

using SysDns = System.Net.Dns;

namespace Library.Net;

/// <summary>
/// A static class for getting host IP address and host name.
/// </summary>
/// <returns>Returns the IP address and host name of the current machine.</returns>
public static class Dns
{
    /// <summary>
    /// Gets the IP address of the host machine.
    /// </summary>
    /// <returns>The IP address of the host machine.</returns>
    public static IpAddress GetHostIpAddress() =>
        IpAddress.Parse(SysDns.GetHostAddresses(GetHostName()).First(a => a.AddressFamily == AddressFamily.InterNetwork).ToString());

    /// <summary>
    /// Gets the host name of the local computer.
    /// </summary>
    /// <returns>A string that contains the DNS host name of the local computer.</returns>
    /// <exception cref="System.Net.Sockets.SocketException:">
    /// An error is encountered when resolving the local host name.
    /// </exception>
    public static string GetHostName() =>
        SysDns.GetHostName();
}