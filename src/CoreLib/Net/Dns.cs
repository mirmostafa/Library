namespace Library.Net;
public static class Dns
{
    public static IpAddress GetHostIpAddress() =>
        IpAddress.Parse(System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.Last().ToString());

    public static string GetHostName() =>
        System.Net.Dns.GetHostName();

    public static IpAddress? GetIpAddress(string name)
    {
        var entries = System.Net.Dns.GetHostEntry(name).AddressList;
        return entries.Any() ? IpAddress.Parse(entries.Last().ToString()) : null;
    }
}