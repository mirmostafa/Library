using System.Net.NetworkInformation;

namespace Library.Net;

public class Nic
{
    private long _bytesReceivedLast;
    private long _bytesSentLast;

    public Nic(NetworkInterface networkInterface)
        => this.NetworkInterface = networkInterface;

    public NetworkInterface NetworkInterface { get; private set; }

    public static IEnumerable<NetworkInterface> GetAllNetworkInterfaceCards()
        => NetworkInterface.GetAllNetworkInterfaces();

    public int GetBytesSent()
    {
        var interfaceStats = this.NetworkInterface.GetIPv4Statistics();
        var result = interfaceStats.BytesSent - this._bytesSentLast;
        this._bytesSentLast = interfaceStats.BytesSent;
        return result.ToInt() / 1024;
    }

    public int GetDownloadSpeed()
    {
        var interfaceStats = this.NetworkInterface.GetIPv4Statistics();
        var result = interfaceStats.BytesSent - this._bytesReceivedLast;
        this._bytesReceivedLast = interfaceStats.BytesReceived;
        return result.ToInt();
    }
}