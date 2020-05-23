#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Library35.Globalization.DataTypes;
using Library35.Globalization.Helpers;
using Library35.Helpers;

namespace Library35.Globalization
{
	// Programmer: Mohammad
	// Description: PersianDateTime
	// Date: 1385/10/6 10:33:14 AM
	[Serializable]
	public struct PersianDateTime : ICloneable, IComparable<PersianDateTime>, IEquatable<PersianDateTime>, IConvertible, ISerializable
	{
		internal static PersianCalendar PersianCalendar = new PersianCalendar();
		internal PersianDateTimeData Data;

		/// <summary>
		///     Initializes a new instance of the <see cref="PersianDateTime" /> struct.
		/// </summary>
		/// <param name="dateTime"> A DateTime instance. </param>
		public PersianDateTime(DateTime dateTime)
		{
			this.Data = new PersianDateTimeData();
			this.Data.Init();
			this.Data.Year = PersianCalendar.GetYear(dateTime);
			this.Data.Month = PersianCalendar.GetMonth(dateTime);
			this.Data.Day = PersianCalendar.GetDayOfMonth(dateTime);
			this.Data.Hour = PersianCalendar.GetHour(dateTime);
			this.Data.Minute = PersianCalendar.GetMinute(dateTime);
			this.Data.Second = PersianCalendar.GetSecond(dateTime);
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="PersianDateTime" /> struct.
		/// </summary>
		/// <param name="year"> The year. </param>
		/// <param name="month"> The month. </param>
		/// <param name="day"> The day. </param>
		/// <param name="hour"> The hour. </param>
		/// <param name="minute"> The minute. </param>
		/// <param name="second"> The second. </param>
		public PersianDateTime(int year, int month, int day, int hour, int minute, int second)
		{
			this.Data = new PersianDateTimeData();
			this.Data.Init();
			this.Data.Year = year;
			this.Data.Month = month;
			this.Data.Day = day;
			this.Data.Hour = hour;
			this.Data.Minute = minute;
			this.Data.Second = second;
		}

		/// <summary>
		///     Gets the year.
		/// </summary>
		/// <value> The year. </value>
		public int Year
		{
			get { return this.Data.Year; }
		}

		/// <summary>
		///     Gets the month.
		/// </summary>
		/// <value> The month. </value>
		public int Month
		{
			get { return this.Data.Month; }
		}

		/// <summary>
		///     Gets the day.
		/// </summary>
		/// <value> The day. </value>
		public int Day
		{
			get { return this.Data.Day; }
		}

		/// <summary>
		///     Gets the hour.
		/// </summary>
		/// <value> The hour. </value>
		public int Hour
		{
			get { return this.Data.Hour; }
		}

		/// <summary>
		///     Gets the minute.
		/// </summary>
		/// <value> The minute. </value>
		public int Minute
		{
			get { return this.Data.Minute; }
		}

		/// <summary>
		///     Gets the second.
		/// </summary>
		/// <value> The second. </value>
		public int Second
		{
			get { return this.Data.Second; }
		}

		public static PersianDateTime Now
		{
			get { return DateTime.Now.ToPersianDateTime(); }
		}

		public long Ticks
		{
			get { return ((DateTime)this).Ticks; }
		}

		/// <summary>
		///     Gets the month names.
		/// </summary>
		/// <value> The month names. </value>
		public static IEnumerable<string> MonthNames
		{
			get { return EnumHelper.GetItems<PersianMonth>().GetDescriptions(); }
		}

		/// <summary>
		///     Gets the day of week.
		/// </summary>
		/// <value> The day of week. </value>
		public PersianDayOfWeek DayOfWeek
		{
			get { return (PersianDayOfWeek)PersianCalendar.GetDayOfWeek(this).ToInt(); }
		}

		/// <summary>
		///     Gets the day of week title.
		/// </summary>
		/// <value> The day of week title. </value>
		public string DayOfWeekTitle
		{
			get { return this.DayOfWeek.GetItemDescription(); }
		}

		#region ICloneable Members
		/// <summary>
		///     Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns> A new object that is a copy of this instance. </returns>
		public object Clone()
		{
			return new PersianDateTime(this.Year, this.Month, this.Day, this.Hour, this.Minute, this.Second);
		}
		#endregion

		#region IComparable<PersianDateTime> Members
		public int CompareTo(PersianDateTime other)
		{
			return DateTime.Compare(this, other);
		}
		#endregion

		#region IConvertible Members
		/// <summary>
		///     Converts the value of this instance to an equivalent <see cref="T:System.DateTime" /> using the specified
		///     culture-specific formatting information.
		/// </summary>
		/// <param name="provider">
		///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting
		///     information.
		/// </param>
		/// <returns>
		///     A <see cref="T:System.DateTime" /> instance equivalent to the value of this instance.
		/// </returns>
		public DateTime ToDateTime(IFormatProvider provider)
		{
			return this;
		}

		TypeCode IConvertible.GetTypeCode()
		{
			throw new NotSupportedException();
		}

		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			throw RaiseInvalidTypeCastException();
		}

		byte IConvertible.ToByte(IFormatProvider provider)
		{
			throw RaiseInvalidTypeCastException();
		}

		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw RaiseInvalidTypeCastException();
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			throw RaiseInvalidTypeCastException();
		}

		double IConvertible.ToDouble(IFormatProvider provider)
		{
			throw RaiseInvalidTypeCastException();
		}

		short IConvertible.ToInt16(IFormatProvider provider)
		{
			throw RaiseInvalidTypeCastException();
		}

		int IConvertible.ToInt32(IFormatProvider provider)
		{
			throw RaiseInvalidTypeCastException();
		}

		long IConvertible.ToInt64(IFormatProvider provider)
		{
			throw RaiseInvalidTypeCastException();
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			throw RaiseInvalidTypeCastException();
		}

		float IConvertible.ToSingle(IFormatProvider provider)
		{
			throw RaiseInvalidTypeCastException();
		}

		string IConvertible.ToString(IFormatProvider provider)
		{
			throw RaiseInvalidTypeCastException();
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			throw RaiseInvalidTypeCastException();
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			throw RaiseInvalidTypeCastException();
		}

		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			throw RaiseInvalidTypeCastException();
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			throw RaiseInvalidTypeCastException();
		}
		#endregion

		#region IEquatable<PersianDateTime> Members
		public bool Equals(PersianDateTime other)
		{
			return this.CompareTo(other) == 0;
		}
		#endregion

		#region ISerializable Members
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}
		#endregion

		public static int Compare(string persianDateTime1, string persianDateTime2)
		{
			PersianDateTime p1, p2;
			if (!TryParsePersian(persianDateTime1, out p1))
				throw new InvalidCastException("cannot cast persianDateTime1 to PersianDateTime");
			if (!TryParsePersian(persianDateTime2, out p2))
				throw new InvalidCastException("cannot cast persianDateTime2 to PersianDateTime");
			return p1.CompareTo(p2);
		}

		public static implicit operator string(PersianDateTime persianDateTime)
		{
			return string.Format(CultureInfo.CurrentCulture,
				"{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}",
				persianDateTime.Data.Year,
				persianDateTime.Data.Month,
				persianDateTime.Data.Day,
				persianDateTime.Data.Hour,
				persianDateTime.Data.Minute,
				persianDateTime.Data.Second);
		}

		public static implicit operator PersianDateTime(string persianDateTimeString)
		{
			return ParsePersian(persianDateTimeString);
		}

		public static implicit operator PersianDateTime(DateTime dateTime)
		{
			return dateTime.ToPersianDateTime();
		}

		public static implicit operator DateTime(PersianDateTime persiandateTime)
		{
			return PersianCalendar.ToDateTime(persiandateTime.Year, persiandateTime.Month, persiandateTime.Day, persiandateTime.Hour, persiandateTime.Minute, persiandateTime.Second, 0);
		}

		public static PersianDateTime operator +(PersianDateTime persiandateTime1, PersianDateTime persiandateTime2)
		{
			DateTime dateTime1 = persiandateTime1;
			DateTime dateTime2 = persiandateTime2;
			return dateTime1.Add(new TimeSpan(dateTime2.Ticks)).ToPersianDateTime();
		}

		public static PersianDateTime operator -(PersianDateTime persiandateTime1, PersianDateTime persiandateTime2)
		{
			DateTime dateTime1 = persiandateTime1;
			DateTime dateTime2 = persiandateTime2;
			return dateTime1.Subtract(new TimeSpan(dateTime2.Ticks)).ToPersianDateTime();
		}

		public static bool operator ==(PersianDateTime persiandateTime1, PersianDateTime persiandateTime2)
		{
			return persiandateTime1.Equals(persiandateTime2);
		}

		public static bool operator !=(PersianDateTime persiandateTime1, PersianDateTime persiandateTime2)
		{
			return !persiandateTime1.Equals(persiandateTime2);
		}

		public static PersianDateTime Add(PersianDateTime persianDateTime1, PersianDateTime persiandateTime2)
		{
			return persianDateTime1 + persiandateTime2;
		}

		public static PersianDateTime Subtract(PersianDateTime persiandateTime1, PersianDateTime persiandateTime2)
		{
			return persiandateTime1 - persiandateTime2;
		}

		public static PersianDateTime ParsePersian(string dateTimeString)
		{
			ArgHelper.AssertNotNull(dateTimeString, "dateTimeString");
			var result = new PersianDateTime
			             {
				             Data =
				             {
					             HasDate = (dateTimeString.IndexOf("/") > 0),
					             HasTime = (dateTimeString.IndexOf(":") > 0)
				             }
			             };
			if (result.Data.HasDate)
			{
				var datePart = result.Data.HasTime ? dateTimeString.Substring(0, dateTimeString.IndexOf(" ")) : dateTimeString;
				if (!int.TryParse(datePart.Substring(0, datePart.IndexOf("/")), out result.Data.Year))
					throw new ArgumentException("not valid date", "dateTimeString");
				datePart = datePart.Remove(0, datePart.IndexOf("/") + 1);
				if (!int.TryParse(datePart.Substring(0, datePart.IndexOf("/")), out result.Data.Month))
					throw new ArgumentException("not valid date", "dateTimeString");
				datePart = datePart.Remove(0, datePart.IndexOf("/") + 1);
				if (!int.TryParse(datePart, out result.Data.Day))
					throw new ArgumentException("not valid date", "dateTimeString");

				if (result.Data.Year < 1300)
					result.Data.Year += 1300;
			}
			if (result.ToString().Equals("00:00:00 0000/00/00"))
				throw new ArgumentException("not valid date", "dateTimeString");
			return result;
		}

		public static PersianDateTime ParseEnglish(string dateTimeString)
		{
			ArgHelper.AssertNotNull(dateTimeString, "dateTimeString");
			return DateTime.Parse(dateTimeString, CultureInfo.CurrentCulture).ToPersianDateTime();
		}

		/// <summary>
		///     Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <param name="format"> The format. </param>
		/// <returns>
		///     A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public string ToString(string format)
		{
			ArgHelper.AssertNotNull(format, "format");
			format = format.Trim().Replace("yyyy", this.Year.ToString("0000", CultureInfo.CurrentCulture));
			format = format.Trim().Replace("MMMM", MonthNames.ToArray()[this.Month - 1]);
			format = format.Trim().Replace("MM", this.Month.ToString("00", CultureInfo.CurrentCulture));
			format = format.Trim().Replace("dd", this.Day.ToString("00", CultureInfo.CurrentCulture));
			format = format.Trim().Replace("HH", this.Hour.ToString("00", CultureInfo.CurrentCulture));
			format = format.Trim().Replace("mm", this.Minute.ToString("00", CultureInfo.CurrentCulture));
			format = format.Trim().Replace("ss", this.Second.ToString("00", CultureInfo.CurrentCulture));
			return format;
		}

		/// <summary>
		///     Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		///     A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return this.ToString("HH:mm:ss yyyy/MM/dd");
		}

		/// <summary>
		///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
		/// </summary>
		/// <param name="obj">
		///     The <see cref="System.Object" /> to compare with this instance.
		/// </param>
		/// <returns>
		///     <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c> .
		/// </returns>
		public override bool Equals(object obj)
		{
			if (!(obj is PersianDateTime))
				return false;
			var target = (PersianDateTime)obj;
			return this.CompareTo(target) == 0;
		}

		/// <summary>
		///     Returns a hash code for this instance.
		/// </summary>
		/// <returns> A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public string ToDateString()
		{
			return this.ToDateString('/');
		}

		public string ToDateString(char separator)
		{
			return string.Concat(this.Year.ToString("0000", CultureInfo.CurrentCulture),
				separator,
				this.Month.ToString("00", CultureInfo.CurrentCulture),
				separator,
				this.Day.ToString("00", CultureInfo.CurrentCulture));
		}

		/// <summary>
		///     Converts to the DateTime.
		/// </summary>
		/// <returns> </returns>
		public DateTime ToDateTime()
		{
			return this;
		}

		/// <summary>
		///     Raises the invalid type cast exception.
		/// </summary>
		/// <returns> </returns>
		private static InvalidCastException RaiseInvalidTypeCastException()
		{
			var targetType = MethodHelper.CallerMethodName.Substring(2);
			return new InvalidCastException(string.Format("Unable to cast {0} to {1}", "PersianDateTime", targetType));
		}

		/// <summary>
		///     Adds the specified PersianDate time.
		/// </summary>
		/// <param name="persianDateTime"> The PersianDate time. </param>
		/// <returns> </returns>
		public PersianDateTime Add(PersianDateTime persianDateTime)
		{
			return Add(this, persianDateTime);
		}

		/// <summary>
		///     Tries to parse a string to PersianDateTime.
		/// </summary>
		/// <param name="str"> The STR. </param>
		/// <param name="result"> The result. </param>
		/// <returns> </returns>
		public static bool TryParsePersian(string str, out PersianDateTime result)
		{
			result = Now;
			try
			{
				result = ParsePersian(str);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}