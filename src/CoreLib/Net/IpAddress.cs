﻿using System.Net.NetworkInformation;

using Library.DesignPatterns.Markers;
using Library.Validations;

namespace Library.Net;

[Immutable]
public sealed class IpAddress : IComparable<IpAddress>, IEquatable<IpAddress>
{
    private readonly int[] _parts;

    private IpAddress([DisallowNull] in string ip)
    {
        Validate(ip);
        this._parts = Split(ip);
    }

    public int this[in int index] => this._parts[index];

    public static string Format(in string ip)
    {
        Validate(ip);
        return Merge(Split(ip));
    }

    public static IpAddress? GetCurrentIp()
            => Dns.GetHostIpAddress();

    public static IEnumerable<IpAddress> GetRange(IpAddress start, IpAddress end)
    {
        var current = start;
        while (current.CompareTo(end) == -1)
        {
            current = current.GetNext();
            yield return current;
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

    public static implicit operator string?(in IpAddress? ipAddress)
        => ipAddress?.ToString();

    public static bool IsValid(in string ip)
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

    public static bool operator !=(in IpAddress left, IpAddress right)
        => !Equals(left, right);

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

    public static PingReply Ping(in string hostNameOrAddress, TimeSpan timeout)
    {
        Check.IfArgumentNotNull(hostNameOrAddress);

        using var ping = new Ping();
        return ping.Send(hostNameOrAddress, timeout.TotalMilliseconds.ToInt());
    }

    public static PingReply Ping(in string hostNameOrAddress)
    {
        Check.IfArgumentNotNull(hostNameOrAddress);

        using var ping = new Ping();
        return ping.Send(hostNameOrAddress);
    }

    public static string ResolveIp(in string ip)
    {
        Validate(ip);
        return ip != null ? System.Net.Dns.GetHostEntry(ip).HostName : string.Empty;
    }

    public static int[] Split(in IpAddress ipAddress)
        => ipAddress._parts;

    public static bool TryParse(in string ip, out IpAddress? result)
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

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list>
    /// </returns>
    public int CompareTo(IpAddress? other)
        => other is null
            ? -1
            : Merge(this._parts).Replace(".", "").ToLong().CompareTo(Merge(other._parts).Replace(".", "").ToLong());

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

    public bool Equals(in string ip)
        => this.ToString().Equals(ip);

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
    public override bool Equals(object? obj)
        => obj is IpAddress ip && this.Equals(ip);

    public string Format()
        => Merge(this._parts);

    /// <summary>
    ///     Serves as a hash function for a particular type.
    /// </summary>
    /// <returns>
    ///     A hash code for the current <see cref="T:System.Object" />.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public override int GetHashCode()
        => this._parts.GetHashCode();

    public IpAddress GetNext()
    {
        var result = FromIp(this);
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

    public bool IsInRange(in IEnumerable<IpAddress> ipAddresses)
            => ipAddresses.Any(ip => ip.Equals(this));

    public PingReply Ping(in TimeSpan timeout)
        => Ping(this.ToString(), timeout);

    public PingReply Ping()
        => Ping(this.ToString());

    public string Resolve()
        => ResolveIp(this.ToString());

    /// <summary>
    ///     Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
    /// </summary>
    /// <returns>
    ///     A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    [return: NotNull]
    public override string ToString()
        => Merge(this._parts);

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

    private static IpAddress FromIp(in IpAddress ip)
                                => new(ip.ToString());

    private static string Merge(in int[] parts)
        => $"{parts[0]:000}.{parts[1]:000}.{parts[2]:000}.{parts[3]:000}";

    private static int[] Split(in string ip)
        => ip.Split('.').Select(s => s.ToInt()).ToArray();

    private static void Validate(in string ip)
    {
        var parts = ip.ArgumentNotNull().Split('.');
        Check.If(parts.Length != 4, () => new InvalidCastException("Parameter cannot be cast to IpAddress"));
        Check.If(parts.Any(part => !part.IsInteger()), () => new InvalidCastException("Parameter cannot be cast to IpAddress"));
        Check.If(parts.Any(part => !part.ToInt().IsBetween(0, 255)), () => new InvalidCastException("Parameter cannot be cast to IpAddress"));
    }
}