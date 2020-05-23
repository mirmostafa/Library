#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using Library35.Helpers;

namespace Library35.Net
{
	public sealed class IpAddress : IComparable<IpAddress>, IEquatable<IpAddress>
	{
		private readonly int[] _Parts;

		private IpAddress(string ip)
		{
			if (ip == null)
				throw new ArgumentNullException("ip");
			var builder = new StringBuilder();
			builder.Append(ip);
			Validate(builder.ToString());
			this._Parts = Split(builder.ToString());
		}

		#region IComparable<IpAddress> Members
		public int CompareTo(IpAddress other)
		{
			Func<int[], string> merge = parts => string.Format("{0}.{1}.{2}.{3}", parts[0].ToString("000"), parts[1].ToString("000"), parts[2].ToString("000"), parts[3].ToString("000"));
			return merge(this._Parts).Replace(".", "").ToLong().CompareTo(merge(other._Parts).Replace(".", "").ToLong());
		}
		#endregion

		#region IEquatable<IpAddress> Members
		/// <summary>
		///     Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
		/// </returns>
		/// <param name="other"> An object to compare with this object. </param>
		public bool Equals(IpAddress other)
		{
			if (other == null)
				throw new ArgumentNullException("other");
			return this.GetFormatted().Equals(other.GetFormatted());
		}
		#endregion

		public static IpAddress Parse(string ip)
		{
			return new IpAddress(ip);
		}

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

		public static int[] Split(string ip)
		{
			Validate(ip);
			return ip.Split('.').ForEach(s => s.ToInt()).ToArray();
		}

		private static void Validate(string ip)
		{
			if (ip == null)
				throw new ArgumentNullException("ip");
			string[] parts;
			if ((parts = ip.Split('.')).Count() != 4)
				throw new InvalidCastException("Paramater cannot be cast to IpAddress");
			if (parts.Any(part => !part.IsInteger()))
				throw new InvalidCastException("Paramater cannot be cast to IpAddress");
			if (parts.Any(part => !part.ToInt().IsBetween(0, 255)))
				throw new InvalidCastException("Paramater cannot be cast to IpAddress");
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
			if (start == null)
				throw new ArgumentNullException("start");
			if (end == null)
				throw new ArgumentNullException("end");
			var startIp = new IpAddress(start);
			var endIp = new IpAddress(end);
			yield return startIp;
			while (startIp.CompareTo(endIp) == -1)
			{
				startIp = startIp.GetNext();
				yield return startIp;
			}
		}

		public static int[] Split(IpAddress ipAddress)
		{
			return ipAddress._Parts;
		}

		internal IpAddress GetNext()
		{
			var result = new IpAddress(this.ToString());
			result._Parts[3]++;
			if (result._Parts[3] == 256)
			{
				result._Parts[2]++;
				result._Parts[3] = 0;
			}
			if (result._Parts[2] == 256)
			{
				result._Parts[1]++;
				result._Parts[2] = 0;
			}
			if (result._Parts[1] == 256)
			{
				result._Parts[0]++;
				result._Parts[1] = 0;
			}
			if (result._Parts[0] == 256)
				result._Parts[0] = result._Parts[1] = result._Parts[2] = result._Parts[3] = 0;
			return result;
		}

		public bool Equals(string ip)
		{
			return this.ToString().Equals(ip);
		}

		/// <summary>
		///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current
		///     <see cref="T:System.Object" />.
		/// </summary>
		/// <returns>
		///     true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" /> ;
		///     otherwise, false.
		/// </returns>
		/// <param name="obj">
		///     The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" /> .
		/// </param>
		/// <exception cref="T:System.NullReferenceException">
		///     The
		///     <paramref name="obj" />
		///     parameter is null.
		/// </exception>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != typeof (IpAddress))
				return false;
			return this.Equals((IpAddress)obj);
		}

		/// <summary>
		///     Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		///     A hash code for the current <see cref="T:System.Object" /> .
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return (this._Parts != null ? this._Parts.GetHashCode() : 0);
		}

		public static bool operator ==(IpAddress left, IpAddress right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(IpAddress left, IpAddress right)
		{
			return !Equals(left, right);
		}

		/// <summary>
		///     Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
		/// </summary>
		/// <returns>
		///     A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" /> .
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}.{3}", this._Parts[0], this._Parts[1], this._Parts[2], this._Parts[3]);
		}

		public PingReply Ping(TimeSpan timeout)
		{
			return Ping(this.ToString(), timeout);
		}

		public static PingReply Ping(string hostNameOrAddress, TimeSpan timeout)
		{
			if (hostNameOrAddress == null)
				throw new ArgumentNullException("hostNameOrAddress");
			using (var ping = new Ping())
				return ping.Send(hostNameOrAddress, timeout.TotalMilliseconds.ToInt());
		}

		public static PingReply Ping(string hostNameOrAddress)
		{
			if (hostNameOrAddress == null)
				throw new ArgumentNullException("hostNameOrAddress");
			using (var ping = new Ping())
				return ping.Send(hostNameOrAddress);
		}

		public PingReply Ping()
		{
			return Ping(this.ToString());
		}

		public static implicit operator string(IpAddress ipAddress)
		{
			return ipAddress != null ? ipAddress.ToString() : string.Empty;
		}

		public string GetFormatted()
		{
			return string.Format("{0}.{1}.{2}.{3}", this._Parts[0].ToString("000"), this._Parts[1].ToString("000"), this._Parts[2].ToString("000"), this._Parts[3].ToString("000"));
		}

		public static string GetFormated(string ip)
		{
			Validate(ip);
			var parts = Split(ip);
			return string.Format("{0}.{1}.{2}.{3}", parts[0].ToString("000"), parts[1].ToString("000"), parts[2].ToString("000"), parts[3].ToString("000"));
		}

		public static string GetFormated(IpAddress ipAddress)
		{
			return ipAddress.GetFormatted();
		}

		public static string ResolveIp(string ip)
		{
			Validate(ip);
			return ip != null ? System.Net.Dns.GetHostEntry(ip).HostName : string.Empty;
		}

		public string Resolve()
		{
			return ResolveIp(this.ToString());
		}

		public bool IsInRange(IEnumerable<IpAddress> ipAddresses)
		{
			return ipAddresses.Where(ip => ip.Equals(this)).Any();
		}

		public static IpAddress GetCurrentIp()
		{
			return Dns.GetHostIpAddress();
		}

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
	}
}