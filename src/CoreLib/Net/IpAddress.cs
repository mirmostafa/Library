using System.Collections.Immutable;
using System.Net.NetworkInformation;

using Library.DesignPatterns.Markers;
using Library.Interfaces;
using Library.Results;
using Library.Validations;

namespace Library.Net;

[Immutable]
[Serializable]
public sealed class IpAddress :
    IComparable<IpAddress>, IEquatable<IpAddress>, IParsable<IpAddress>,
    IAdditionOperators<IpAddress, IpAddress, IpAddress>,
    IMinMaxValue<IpAddress>
{
    public static readonly IpAddress MaxValue = Parse("255.255.255.255");
    public static readonly IpAddress MinValue = Parse("0.0.0.0");

    private readonly ImmutableArray<int> _segments;

    public IpAddress([DisallowNull] in string ip)
        => this._segments = Validate(ip).ThrowOnFail().GetValue()!.Split('.').Select(s => s.Cast().ToInt()).ToImmutableArray();

    static IpAddress IMinMaxValue<IpAddress>.MaxValue => MaxValue;
    static IpAddress IMinMaxValue<IpAddress>.MinValue => MinValue;
    public int this[in int index] => this._segments[index];

    public static IpAddress GetCurrentIp()
        => Dns.GetHostIpAddress();

    public static IpAddress GetLocalHost()
        => Parse("127.0.0.1");

    public static IEnumerable<IpAddress> GetRange(IpAddress start, IpAddress end)
    {
        var x = start;
        while (x < end)
        {
            yield return x;
            x = x.Add(1);
        }
    }

    public static IEnumerable<IpAddress> GetRange(string start, string end)
        => GetRange(new IpAddress(start), new IpAddress(end));

    [return: NotNullIfNotNull(nameof(ipAddress))]
    public static implicit operator string?(in IpAddress? ipAddress)
        => ipAddress?.ToString();

    public static bool IsValid(in string ip)
        => Validate(ip).IsSucceed;

    public static bool operator !=(in IpAddress left, IpAddress right)
        => !Equals(left, right);

    public static IpAddress operator +(IpAddress left, IpAddress right)
        => left.Add(right);

    public static bool operator <(in IpAddress left, IpAddress right)
        => left is null ? right is not null : left.CompareTo(right) < 0;

    public static bool operator <=(in IpAddress left, IpAddress right)
        => left is null || left.CompareTo(right) <= 0;

    public static bool operator ==(in IpAddress left, IpAddress right)
        => Equals(left, right);

    public static bool operator >(in IpAddress left, IpAddress right)
        => left is not null && left.CompareTo(right) > 0;

    public static bool operator >=(in IpAddress left, IpAddress right)
        => left is null ? right is null : left.CompareTo(right) >= 0;

    public static IpAddress Parse(in string ip)
        => new(ip);

    public static IpAddress Parse(string s, IFormatProvider? provider)
        => Parse(s);

    public static PingReply Ping(in string hostNameOrAddress, TimeSpan timeout)
    {
        Check.IfArgumentNotNull(hostNameOrAddress);
        LibLogger.Debug($"Ping {hostNameOrAddress}");
        using var ping = new Ping();
        return ping.Send(hostNameOrAddress, timeout.TotalMilliseconds.Cast().ToInt());
    }

    public static PingReply Ping(in string hostNameOrAddress)
    {
        Check.IfArgumentNotNull(hostNameOrAddress);
        LibLogger.Debug($"Ping {hostNameOrAddress}");
        using var ping = new Ping();
        return ping.Send(hostNameOrAddress);
    }

    public static bool TryParse([DisallowNull] in string ip, [MaybeNullWhen(false)] out IpAddress? result)
    {
        if (ip.IsNullOrEmpty())
        {
            result = default;
            return false;
        }
        try
        {
            result = Parse(ip);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    public static TryMethodResult<IpAddress?> TryParse([DisallowNull] in string ip)
    {
        try
        {
            return TryMethodResult<IpAddress>.CreateSuccess(new IpAddress(ip))!;
        }
        catch (Exception ex)
        {
            return TryMethodResult<IpAddress>.CreateFailure(status: ex);
        }
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out IpAddress result)
    {
        if (s.IsNullOrEmpty())
        {
            result = default;
            return false;
        }
        try
        {
            result = Parse(s, provider);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    public static Result<string?> Validate(in string ip)
        => ip.ArgumentNotNull().Split('.').Compact().Check()
             .RuleFor(x => x.Length == 4, () => "Parameter cannot be cast to IpAddress")
             .RuleFor(x => x.All(seg => seg.IsInteger()), () => "Parameter cannot be cast to IpAddress")
             .RuleFor(x => x.All(seg => seg.Cast().ToInt().IsBetween(0, 255)), () => "Parameter cannot be cast to IpAddress")
             .Build().WithValue(ip);

    public IpAddress Add(int i)
        => new(Merge(AddNumberToSegmentArray(this._segments, i)));

    public IpAddress Add(in IpAddress ip)
        => new(Merge(AddIpAddressSegmentArray(this._segments, ip._segments)));

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer
    /// that indicates whether the current instance precedes, follows, or occurs in the same
    /// position in the sort order as the other object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value
    /// has these meanings:
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term>
    /// <description>Meaning</description>
    /// </listheader>
    /// <item>
    /// <term>Less than zero</term>
    /// <description>This instance precedes <paramref name="other"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Zero</term>
    /// <description>
    /// This instance occurs in the same position in the sort order as <paramref name="other"/>.
    /// </description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description>This instance follows <paramref name="other"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    public int CompareTo(IpAddress? other)
        => other is null ? -1 : Merge(this._segments).Replace(".", "").Cast().ToLong().CompareTo(Merge(other._segments).Replace(".", "").Cast().ToLong());

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <returns>
    /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
    /// </returns>
    /// <param name="other">An object to compare with this object.</param>
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

    public bool Equals(in string ip)
        => this.ToString().Equals(ip, StringComparison.Ordinal);

    /// <summary>
    /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
    /// </summary>
    /// <returns>
    /// true if the specified <see cref="T:System.Object"/> is equal to the current <see
    /// cref="T:System.Object"/>; otherwise, false.
    /// </returns>
    /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
    /// <exception cref="T:System.NullReferenceException">
    /// The <paramref name="obj"/> parameter is null.
    /// </exception>
    /// <filterpriority>2</filterpriority>
    public override bool Equals(object? obj)
        => obj is IpAddress ip && this.Equals(ip);

    public string Format()
        => Merge(this._segments);

    /// <summary>
    /// Serves as a hash function for a particular type.
    /// </summary>
    /// <returns>A hash code for the current <see cref="T:System.Object"/>.</returns>
    /// <filterpriority>2</filterpriority>
    public override int GetHashCode()
        => this._segments.GetHashCode();

    public bool IsInRange(in IpAddress min, in IpAddress max)
        => this.CompareTo(min.ArgumentNotNull()) < 0 || this.CompareTo(max.ArgumentNotNull()) > 0;

    public PingReply Ping(in TimeSpan timeout)
        => Ping(this.ToString(), timeout);

    public PingReply Ping()
        => Ping(this.ToString());

    [return: NotNull]
    public string Resolve()
        => System.Net.Dns.GetHostEntry(this).HostName;

    /// <summary>
    /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
    /// </summary>
    /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
    /// <filterpriority>2</filterpriority>
    [return: NotNull]
    public override string ToString()
        => Merge(this._segments);

    /// <summary>
    /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
    /// </summary>
    /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
    /// <filterpriority>2</filterpriority>
    [return: NotNull]
    public string ToString(string format)
        => Merge(this._segments, format);

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

    private static ImmutableArray<int> AddIpAddressSegmentArray(ImmutableArray<int> segment1, ImmutableArray<int> segment2)
    {
        var result = new int[4];
        var carry = 0;

        for (var i = 3; i >= 0; i--)
        {
            var sum = segment1[i] + segment2[i] + carry;
            result[i] = sum % 256;
            carry = sum / 256;
        }

        return result.ToImmutableArray();
    }

    private static ImmutableArray<int> AddNumberToSegmentArray(ImmutableArray<int> segment, int number)
    {
        var result = segment.ToArray();
        result[0] += number / 256 / 256 / 256 % 256;
        result[1] += number / 256 / 256 % 256;
        result[2] += number / 256 % 256;
        result[3] += number % 256;

        for (var i = 3; i > 0; i--)
        {
            result[i - 1] += segment[i] / 256;
            result[i] %= 256;
        }

        return result.ToImmutableArray();
    }

    private static string Merge(in ImmutableArray<int> segments, string format = "0")
        => $"{segments[0].ToString(format)}.{segments[1].ToString(format)}.{segments[2].ToString(format)}.{segments[3].ToString(format)}";
}