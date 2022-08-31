using System.Net.NetworkInformation;
using Library.DesignPatterns.Markers;
using Library.Validations;

namespace Library.Net;

[Immutable]
public sealed class IpAddress : IComparable<IpAddress>, IEquatable<IpAddress>
{
    private readonly int[] _parts;

    private IpAddress([DisallowNull] string ip) =>
        this._parts = Split(ip);

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list>
    /// </returns>
    public int CompareTo(IpAddress? other)
    {
        return other is null
            ? -1
            : merge(this._parts).Replace(".", "").ToLong().CompareTo(merge(other._parts).Replace(".", "").ToLong());

        static string merge(int[] parts) => string.Format("{0}.{1}.{2}.{3}", parts[0].ToString("000"), parts[1].ToString("000"), parts[2].ToString("000"), parts[3].ToString("000"));
    }

    /// <summary>
    ///     Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <returns>
    ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
    /// </returns>
    /// <param name="other">
    ///     An object to compare with this object.
    /// </param>
    public bool Equals(IpAddress? other)
    {
        if (other is null)
        {
            return false;
        }
        for (var i = 0; i < 4; i++)
        {
            if (other[i] != this[i])
            {
                return false;
            }
        }
        return true;
    }

    public int this[in int index] => this._parts[index];

    public static IpAddress Parse(string ip) => new(ip);

    public static bool TryParse(string ip, out IpAddress? result)
    {
        try
        {
            result = new IpAddress(ip);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static int[] Split(string ip)
    {
        Validate(ip);
        return ip.Split('.').Select(s => s.ToInt()).ToArray();
    }

    private static void Validate(string ip)
    {
        Check.IfArgumentNotNull(ip);

        string[] parts;
        Check.If((parts = ip.Split('.')).Length != 4, () => new InvalidCastException("Paramater cannot be cast to IpAddress"));
        Check.If(parts.Any(part => !part.IsInteger()), () => new InvalidCastException("Paramater cannot be cast to IpAddress"));
        Check.If(parts.Any(part => !part.ToInt().IsBetween(0, 255)), () => new InvalidCastException("Paramater cannot be cast to IpAddress"));
    }

    public static bool IsValid(string ip)
    {
        try
        {
            Validate(ip);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static IEnumerable<IpAddress> GetRange(IpAddress start, IpAddress end)
    {
        while (start.CompareTo(end) == -1)
        {
            start = start.GetNext();
            yield return start;
        }
    }

    public static IEnumerable<IpAddress> GetRange(string start, string end)
    {
        Check.IfArgumentNotNull(start);
        Check.IfArgumentNotNull(end);

        var enumerate = new IpAddress(start);
        var endIp = new IpAddress(end);
        do
        {
            yield return enumerate;
            enumerate = enumerate.GetNext();
        } while (enumerate.CompareTo(endIp) == -1);
    }

    public static int[] Split(IpAddress ipAddress)
        => ipAddress._parts;

    internal IpAddress GetNext()
    {
        var result = new IpAddress(this.ToString());
        result._parts[3]++;
        if (result._parts[3] == 256)
        {
            result._parts[2]++;
            result._parts[3] = 0;
        }
        if (result._parts[2] == 256)
        {
            result._parts[1]++;
            result._parts[2] = 0;
        }
        if (result._parts[1] == 256)
        {
            result._parts[0]++;
            result._parts[1] = 0;
        }
        if (result._parts[0] == 256)
        {
            result._parts[0] = result._parts[1] = result._parts[2] = result._parts[3] = 0;
        }

        return result;
    }

    public bool Equals(string ip) => this.ToString().Equals(ip);

    /// <summary>
    ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current
    ///     <see cref="T:System.Object" />.
    /// </summary>
    /// <returns>
    ///     true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />;
    ///     otherwise, false.
    /// </returns>
    /// <param name="obj">
    ///     The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.
    /// </param>
    /// <exception cref="T:System.NullReferenceException">
    ///     The <paramref name="obj" /> parameter is null.
    /// </exception>
    /// <filterpriority>2</filterpriority>
    public override bool Equals(object? obj) =>
        obj is IpAddress ip && this.Equals(ip);

    /// <summary>
    ///     Serves as a hash function for a particular type.
    /// </summary>
    /// <returns>
    ///     A hash code for the current <see cref="T:System.Object" />.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public override int GetHashCode() => this._parts.GetHashCode();

    public static bool operator ==(IpAddress left, IpAddress right) => Equals(left, right);

    public static bool operator !=(IpAddress left, IpAddress right) => !Equals(left, right);

    /// <summary>
    ///     Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
    /// </summary>
    /// <returns>
    ///     A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public override string ToString() => string.Format("{0}.{1}.{2}.{3}", this._parts[0], this._parts[1], this._parts[2], this._parts[3]);

    public PingReply Ping(TimeSpan timeout) => Ping(this.ToString(), timeout);

    public static PingReply Ping(string hostNameOrAddress, TimeSpan timeout)
    {
        Check.IfArgumentNotNull(hostNameOrAddress);

        using var ping = new Ping();
        return ping.Send(hostNameOrAddress, timeout.TotalMilliseconds.ToInt());
    }

    public static PingReply Ping(string hostNameOrAddress)
    {
        Check.IfArgumentNotNull(hostNameOrAddress);

        using var ping = new Ping();
        return ping.Send(hostNameOrAddress);
    }

    public PingReply Ping() => Ping(this.ToString());

    public static implicit operator string(IpAddress? ipAddress) => ipAddress is not null ? ipAddress.ToString() : string.Empty;

    public string GetFormatted() => string.Format("{0}.{1}.{2}.{3}", this._parts[0].ToString("000"), this._parts[1].ToString("000"), this._parts[2].ToString("000"), this._parts[3].ToString("000"));

    public static string GetFormated(string ip)
    {
        Validate(ip);
        var parts = Split(ip);
        return string.Format("{0}.{1}.{2}.{3}", parts[0].ToString("000"), parts[1].ToString("000"), parts[2].ToString("000"), parts[3].ToString("000"));
    }

    public static string GetFormated(IpAddress ipAddress) => ipAddress.GetFormatted();

    public static string ResolveIp(string ip)
    {
        Validate(ip);
        return ip != null ? System.Net.Dns.GetHostEntry(ip).HostName : string.Empty;
    }

    public string Resolve() => ResolveIp(this.ToString());

    public bool IsInRange(IEnumerable<IpAddress> ipAddresses) => ipAddresses.Where(ip => ip.Equals(this)).Any();

    public static IpAddress GetCurrentIp() => Dns.GetHostIpAddress();

    public bool TryResolve(out string? computerName)
    {
        try
        {
            computerName = this.Resolve();
            return true;
        }
        catch
        {
            computerName = null;
            return false;
        }
    }

    public static bool operator <(IpAddress left, IpAddress right) => left is null ? right is not null : left.CompareTo(right) < 0;

    public static bool operator <=(IpAddress left, IpAddress right) => left is null || left.CompareTo(right) <= 0;

    public static bool operator >(IpAddress left, IpAddress right) => left is not null && left.CompareTo(right) > 0;

    public static bool operator >=(IpAddress left, IpAddress right) => left is null ? right is null : left.CompareTo(right) >= 0;
}