namespace Library.Net;

/// <summary>
/// A static class for getting host IP address and host name.
/// </summary>
/// <returns>
/// Returns the IP address and host name of the current machine.
/// </returns>
public static class Dns
{
    /// <summary>
    /// Gets the IP address of the host machine.
    /// </summary>
    /// <returns>The IP address of the host machine.</returns>
    public static IpAddress? GetHostIpAddress()
        => GetIpAddress(System.Net.Dns.GetHostName());

    public static string GetHostName()
        => System.Net.Dns.GetHostName();

    public static IpAddress? GetIpAddress(string name)
    {
        var entries = System.Net.Dns.GetHostEntry(name).AddressList;
        return entries.Any() ? IpAddress.Parse(entries.Last().ToString()) : null;
    }
}