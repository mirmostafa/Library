using System.Net.Sockets;

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
    public static IpAddress GetHostIpAddress()
        => IpAddress.Parse(System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).First(a => a.AddressFamily == AddressFamily.InterNetwork).ToString());

    public static string GetHostName()
        => System.Net.Dns.GetHostName();
}