using System.Net.NetworkInformation;

namespace Library.Net;

public sealed class Nic(NetworkInterface networkInterface)
{
    private long _bytesReceivedLast;
    private long _bytesSentLast;

    public NetworkInterface NetworkInterface { get; private set; } = networkInterface;

    public static IEnumerable<NetworkInterface> GetAllNetworkInterfaceCards()
        => NetworkInterface.GetAllNetworkInterfaces();

    public int GetBytesSent()
    {
        var interfaceStats = this.NetworkInterface.GetIPv4Statistics();
        var result = interfaceStats.BytesSent - this._bytesSentLast;
        this._bytesSentLast = interfaceStats.BytesSent;
        return result.Cast().ToInt() / 1024;
    }

    public int GetDownloadSpeed()
    {
        var interfaceStats = this.NetworkInterface.GetIPv4Statistics();
        var result = interfaceStats.BytesSent - this._bytesReceivedLast;
        this._bytesReceivedLast = interfaceStats.BytesReceived;
        return result.Cast().ToInt();
    }
}