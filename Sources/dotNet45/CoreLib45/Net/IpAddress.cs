// Created on     2018/07/23

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Mohammad.Helpers;

namespace Mohammad.Net
{
    public sealed partial class IpAddress : IComparable<IpAddress>, IEquatable<IpAddress>, IEquatable<IPAddress>
    {
        private readonly int[] _Parts;

        private IpAddress(string ip)
        {
            if (ip == null)
            {
                throw new ArgumentNullException(nameof(ip));
            }

            var builder = new StringBuilder();
            builder.Append(ip);
            Validate(builder.ToString());
            this._Parts = Split(builder.ToString());
        }

        private IpAddress(IpAddress ip)
            : this(ip.ToString())
        {
        }

        public int CompareTo(IpAddress other)
        {
            Func<int[], string> merge = parts => $"{parts[0]:000}.{parts[1]:000}.{parts[2]:000}.{parts[3]:000}";
            return merge(this._Parts).Replace(".", "").ToLong().CompareTo(merge(other._Parts).Replace(".", "").ToLong());
        }

        public bool Equals(IpAddress other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return this.GetFormatted().Equals(other.GetFormatted());
        }

        public bool Equals(IPAddress other) => this == Parse(other?.ToString());

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
            if (start == null)
            {
                throw new ArgumentNullException(nameof(start));
            }

            if (end == null)
            {
                throw new ArgumentNullException(nameof(end));
            }

            while (start.CompareTo(end) == -1)
            {
                yield return start;
                start = start.GetNext();
            }
        }

        public static IEnumerable<IpAddress> GetRange(string start, string end) => GetRange(new IpAddress(start), new IpAddress(end));

        public static string Resolve(string ip)
        {
            Validate(ip);
            LibrarySupervisor.Logger.Debug($"Resolving {ip}");
            return Dns.GetNameByIp(ip);
        }

        public static IpAddress GetFirstInSubmask(IpAddress ip)
        {
            var result = Parse(ip);
            result._Parts[3] = 0;
            return result;
        }

        public static IpAddress GetLastInSubmask(IpAddress ip)
        {
            var result = Parse(ip);
            result._Parts[3] = 255;
            return result;
        }

        private static int[] Split(string ip)
        {
            Validate(ip);
            return ip.Split('.').Select(s => s.ToInt()).ToArray();
        }

        private static void Validate(string ip)
        {
            if (ip == null)
            {
                throw new ArgumentNullException(nameof(ip));
            }

            string[] parts;
            if ((parts = ip.Split('.')).Length != 4 || parts.Any(part => !part.IsInteger()) || parts.Any(part => !part.ToInt().IsBetween(0, 255)))
            {
                throw new InvalidCastException("Parameter cannot be cast to IpAddress");
            }
        }

        public IpAddress GetNext()
        {
            var result = new IpAddress(this.ToString());
            result.MoveNext();
            return result;
        }

        public void MoveNext()
        {
            this._Parts[3]++;
            if (this._Parts[3] == 256)
            {
                this._Parts[2]++;
                this._Parts[3] = 0;
            }

            if (this._Parts[2] == 256)
            {
                this._Parts[1]++;
                this._Parts[2] = 0;
            }

            if (this._Parts[1] == 256)
            {
                this._Parts[0]++;
                this._Parts[1] = 0;
            }

            if (this._Parts[0] == 256)
            {
                this._Parts[0] = this._Parts[1] = this._Parts[2] = this._Parts[3] = 0;
            }
        }

        public override int GetHashCode() => this._Parts?.GetHashCode() ?? 0;

        public override string ToString() => $"{this._Parts[0]}.{this._Parts[1]}.{this._Parts[2]}.{this._Parts[3]}";

        public PingReply Ping(TimeSpan? timeout = null)
        {
            var hostNameOrAddress = this.ToString();
            if (hostNameOrAddress == null)
            {
                throw new ArgumentNullException(nameof(hostNameOrAddress));
            }

            LibrarySupervisor.Logger.Debug($"Pinging {hostNameOrAddress}");
            using (var ping = new Ping())
            {
                return timeout.HasValue ? ping.Send(hostNameOrAddress, timeout.Value.TotalMilliseconds.ToInt()) : ping.Send(hostNameOrAddress);
            }
        }

        public bool CanPing() => this.Ping().Status == IPStatus.Success;

        public string GetFormatted() => $"{this._Parts[0]:000}.{this._Parts[1]:000}.{this._Parts[2]:000}.{this._Parts[3]:000}";

        public string Resolve() => Resolve(this.ToString());

        public bool IsInRange(IEnumerable<IpAddress> ipAddresses) => ipAddresses.Any(ip => ip.Equals(this));

        public bool TryResolve(out string computerName)
        {
            computerName = string.Empty;
            try
            {
                computerName = this.Resolve();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ResolveAndPingResult ResolveAndPing()
        {
            var system = CodeHelper.CatchFunc(this.Resolve, "");
            return system.IsNullOrEmpty()
                ? new ResolveAndPingResult(this, IPStatus.DestinationUnreachable, string.Empty)
                : new ResolveAndPingResult(this, this.Ping(TimeSpan.FromSeconds(5)).Status, system);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var ip = obj as IpAddress;
            return ip != null && this.Equals(ip);
        }
    }

    /*====================================================================================*/

    partial class IpAddress
    {
        public static bool operator ==(IpAddress left, IpAddress right) => Equals(left, right);
        public static bool operator !=(IpAddress left, IpAddress right) => !Equals(left, right);

        public static IpAddress operator +(IpAddress left, int num)
        {
            var result = new IpAddress(left);
            EnumerableHelper.For(num, result.MoveNext);
            return result;
        }

        public static explicit operator string(IpAddress ipAddress) => ipAddress?.ToString() ?? string.Empty;
        public static implicit operator IPAddress(IpAddress ip) => IPAddress.Parse(ip.ToString());
        public static implicit operator IpAddress(IPAddress ip) => Parse(ip.ToString());
        public static IpAddress Parse(string ip) => new IpAddress(ip);
        public static IpAddress Parse(IPAddress ip) => new IpAddress(ip?.ToString());

        public static bool TryParse(string ip, out IpAddress result)
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
    }
}