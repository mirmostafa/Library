using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Mohammad.Globalization.DataTypes;
using Mohammad.Helpers;

namespace Mohammad.Globalization
{
    // Programmer: Mohammad
    // Description: PersianDateTime
    // Date: 1385/10/6 10:33:14 AM
    // Last update: 1399/04/02: Immutalization
    [Serializable]
    //[DebuggerDisplay("Value: {" + nameof(Value) + "}, Text: {" + nameof(Text) + "}", Name = "{" + nameof(Text) + "}")]
    public readonly struct PersianDateTime : ICloneable, IComparable<PersianDateTime>, IEquatable<PersianDateTime>, IConvertible, ISerializable
    {
        internal static PersianCalendar PersianCalendar = new PersianCalendar();
        internal readonly PersianDateTimeData Data;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PersianDateTime" /> struct.
        /// </summary>
        /// <param name="dateTime">A DateTime instance.</param>
        public PersianDateTime(in DateTime dateTime)
        {
            this.Data = new PersianDateTimeData();
            this.Data.Init();
            this.Data.Year = PersianCalendar.GetYear(dateTime);
            this.Data.Month = PersianCalendar.GetMonth(dateTime);
            this.Data.Day = PersianCalendar.GetDayOfMonth(dateTime);
            this.Data.Hour = PersianCalendar.GetHour(dateTime);
            this.Data.Minute = PersianCalendar.GetMinute(dateTime);
            this.Data.Second = PersianCalendar.GetSecond(dateTime);
            this.Data.Millisecond = PersianCalendar.GetMilliseconds(dateTime);
            this.IsInitiated = true;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PersianDateTime" /> struct.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minute">The minute.</param>
        /// <param name="second">The second.</param>
        /// <param name="millisecond"></param>
        public PersianDateTime(in int year, in int month, in int day, in int hour, in int minute, in int second, in double millisecond)
        {
            this.Data = new PersianDateTimeData();
            this.Data.Init();
            this.Data.Year = year;
            this.Data.Month = month;
            this.Data.Day = day;
            this.Data.Hour = hour;
            this.Data.Minute = minute;
            this.Data.Second = second;
            this.Data.Millisecond = millisecond;
            this.IsInitiated = true;
        }

        private PersianDateTime(in PersianDateTimeData data) => (this.Data, this.IsInitiated) = (data, true);

        private PersianDateTime(in SerializationInfo info, in StreamingContext context)
            => (this.Data, this.IsInitiated) = ((PersianDateTimeData)info.GetValue("Data", typeof(PersianDateTimeData)), true);

        public static string DateSeparator { get; set; } = "/";
        public static string TimeSeparator { get; set; } = ":";
        public static string ToStringFormat { get; set; } = "yyyy/MM/dd HH:mm:ss";
        public bool IsInitiated { get; }

        /// <summary>
        ///     Gets the year.
        /// </summary>
        /// <value>The year.</value>
        public int Year => this.Data.Year;

        /// <summary>
        ///     Gets the month.
        /// </summary>
        /// <value>The month.</value>
        public int Month => this.Data.Month;

        /// <summary>
        ///     Gets the day.
        /// </summary>
        /// <value>The day.</value>
        public int Day => this.Data.Day;

        /// <summary>
        ///     Gets the hour.
        /// </summary>
        /// <value>The hour.</value>
        public int Hour => this.Data.Hour;

        /// <summary>
        ///     Gets the minute.
        /// </summary>
        /// <value>The minute.</value>
        public int Minute => this.Data.Minute;

        /// <summary>
        ///     Gets the second.
        /// </summary>
        /// <value>The second.</value>
        public int Second => this.Data.Second;

        public double Millisecond => this.Data.Millisecond;

        /// <summary>
        ///     Gets a <see cref="T:Mohammad.Globalization.PersianDateTime" /> object that is set to the current date and time
        ///     on this computer, expressed as the local time.
        /// </summary>
        /// <returns>An object whose value is the current local date and time.</returns>
        public static PersianDateTime Now => new PersianDateTime(DateTime.Now);

        /// <summary>
        ///     Returns the tick count for this DateTime. The returned value is the number of 100-nanosecond intervals that have
        ///     elapsed since 1/1/0001 12:00am.
        /// </summary>
        public long Ticks => ((DateTime)this).Ticks;

        /// <summary>
        ///     Gets the month names.
        /// </summary>
        /// <value>The month names.</value>
        public static IEnumerable<string> MonthNames => EnumHelper.GetDescriptions(EnumHelper.GetItems<PersianMonth>());

        /// <summary>
        ///     Gets the days of week names.
        /// </summary>
        /// <value>The month names.</value>
        public static IEnumerable<string> DaysOfWeek => EnumHelper.GetDescriptions(EnumHelper.GetItems<PersianDayOfWeek>());

        /// <summary>
        ///     Gets the day of week.
        /// </summary>
        /// <value>The day of week.</value>
        public PersianDayOfWeek DayOfWeek => (PersianDayOfWeek)PersianCalendar.GetDayOfWeek(this).ToInt();

        /// <summary>
        ///     Gets the day of week title.
        /// </summary>
        /// <value>The day of week title.</value>
        public string DayOfWeekTitle => EnumHelper.GetItemDescription(this.DayOfWeek);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        // ReSharper disable once UnusedMember.Local
        private string DebuggerDisplay
        {
            get
            {
                var self = this;
                return self.ToString();
            }
        }

        /// <summary>
        ///     Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        ///     A new object that is a copy of this instance.
        /// </returns>
        public object Clone() => new PersianDateTime(this.Year, this.Month, this.Day, this.Hour, this.Minute, this.Second, this.Millisecond);

        public int CompareTo(PersianDateTime other) => DateTime.Compare(this, other);

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
        public DateTime ToDateTime(IFormatProvider? provider) => this;

        TypeCode IConvertible.GetTypeCode() => throw new NotSupportedException();

        bool IConvertible.ToBoolean(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

        byte IConvertible.ToByte(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

        char IConvertible.ToChar(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

        decimal IConvertible.ToDecimal(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

        double IConvertible.ToDouble(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

        short IConvertible.ToInt16(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

        int IConvertible.ToInt32(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

        long IConvertible.ToInt64(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

        sbyte IConvertible.ToSByte(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

        float IConvertible.ToSingle(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

        string IConvertible.ToString(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

        object IConvertible.ToType(Type conversionType, IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

        ushort IConvertible.ToUInt16(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

        uint IConvertible.ToUInt32(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

        ulong IConvertible.ToUInt64(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

        public bool Equals(PersianDateTime other) => this.CompareTo(other) == 0;

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => info.AddValue("Data", this.Data);

        public static implicit operator string(in PersianDateTime persianDateTime) => persianDateTime.ToString();
        public static implicit operator PersianDateTime(in string persianDateTimeString) => ParsePersian(persianDateTimeString);
        public static implicit operator PersianDateTime(in DateTime dateTime) => dateTime.ToPersianDateTime();

        public static implicit operator DateTime(in PersianDateTime persianDateTime) => PersianCalendar.ToDateTime(persianDateTime.Year,
            persianDateTime.Month,
            persianDateTime.Day,
            persianDateTime.Hour,
            persianDateTime.Minute,
            persianDateTime.Second,
            0);

        public static PersianDateTime operator +(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2)
        {
            DateTime dateTime1 = persianDateTime1;
            DateTime dateTime2 = persianDateTime2;
            return dateTime1.Add(new TimeSpan(dateTime2.Ticks)).ToPersianDateTime();
        }

        public static PersianDateTime operator -(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2)
        {
            DateTime dateTime1 = persianDateTime1;
            DateTime dateTime2 = persianDateTime2;
            return dateTime1.Subtract(new TimeSpan(dateTime2.Ticks)).ToPersianDateTime();
        }

        public static bool operator ==(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2) => persianDateTime1.Equals(persianDateTime2);

        public static bool operator !=(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2) => !persianDateTime1.Equals(persianDateTime2);

        public static bool operator <(in PersianDateTime left, in PersianDateTime right) => left.CompareTo(right) < 0;

        public static bool operator <=(in PersianDateTime left, in PersianDateTime right) => left.CompareTo(right) <= 0;

        public static bool operator >(in PersianDateTime left, in PersianDateTime right) => left.CompareTo(right) > 0;

        public static bool operator >=(in PersianDateTime left, in PersianDateTime right) => left.CompareTo(right) >= 0;

        public static int Compare(in string persianDateTime1, in string persianDateTime2)
        {
            if (!TryParsePersian(persianDateTime1, out var p1))
            {
                throw new InvalidCastException("cannot cast persianDateTime1 to PersianDateTime");
            }

            if (!TryParsePersian(persianDateTime2, out var p2))
            {
                throw new InvalidCastException("cannot cast persianDateTime2 to PersianDateTime");
            }

            return p1.CompareTo(p2);
        }

        public static PersianDateTime Add(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2) => persianDateTime1 + persianDateTime2;
        public static PersianDateTime Subtract(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2) => persianDateTime1 - persianDateTime2;
        public static PersianDateTime ParseDateTime(in DateTime dateTime) => new PersianDateTime(dateTime);

        public static PersianDateTime ParsePersian(in string dateTimeString)
        {
            ArgHelper.AssertNotNull(dateTimeString, nameof(dateTimeString));
            var data = new PersianDateTimeData
            {
                HasDate = dateTimeString.IndexOf(DateSeparator, StringComparison.Ordinal) > 0,
                HasTime = dateTimeString.IndexOf(":", StringComparison.Ordinal) > 0
            };
            var indexOfSpace = dateTimeString.IndexOf(" ", StringComparison.Ordinal);
            if (data.HasDate)
            {
                var datePart = data.HasTime ? dateTimeString.Substring(0, indexOfSpace) : dateTimeString;
                if (!int.TryParse(datePart.Substring(0, datePart.IndexOf(DateSeparator, StringComparison.Ordinal)), out data.Year))
                {
                    throw new ArgumentException("not valid date", nameof(dateTimeString));
                }

                datePart = datePart.Remove(0, datePart.IndexOf(DateSeparator, StringComparison.Ordinal) + 1);
                if (!int.TryParse(datePart.Substring(0, datePart.IndexOf(DateSeparator, StringComparison.Ordinal)), out data.Month))
                {
                    throw new ArgumentException("not valid date", nameof(dateTimeString));
                }

                datePart = datePart.Remove(0, datePart.IndexOf(DateSeparator, StringComparison.Ordinal) + 1);
                if (!int.TryParse(datePart, out data.Day))
                {
                    throw new ArgumentException("not valid date", nameof(dateTimeString));
                }
            }

            if (data.HasTime)
            {
                var timePart = data.HasDate ? dateTimeString.Substring(indexOfSpace) : dateTimeString;
                if (!int.TryParse(timePart.Substring(0, timePart.IndexOf(TimeSeparator, StringComparison.Ordinal)), out data.Hour))
                {
                    throw new ArgumentException("not valid date", nameof(dateTimeString));
                }

                timePart = timePart.Remove(0, timePart.IndexOf(TimeSeparator, StringComparison.Ordinal) + 1);
                if (!int.TryParse(timePart, out data.Minute))
                {
                    throw new ArgumentException("not valid date", nameof(dateTimeString));
                }
            }

            if (data.ToString()?.Equals("00:00:00 0000/00/00") != false)
            {
                throw new ArgumentException("not valid date", nameof(dateTimeString));
            }

            return new PersianDateTime(data);
        }

        public static PersianDateTime ParseEnglish(in string dateTimeString)
        {
            ArgHelper.AssertNotNull(dateTimeString, nameof(dateTimeString));
            return DateTime.Parse(dateTimeString, CultureInfo.CurrentCulture).ToPersianDateTime();
        }

        public static string ToPersian(in DateTime dateTime) => new PersianDateTime(dateTime).ToString()!;

        /// <summary>
        ///     Tries to parse a string to PersianDateTime.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool TryParsePersian(in string str, out PersianDateTime result)
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

        public static (PersianDateTime Result, bool Succeed) TryParsePersian(in string str)
        {
            var succeed = TryParsePersian(str, out var result);
            return (result, succeed);
        }

        public static bool IsPersianDateTime(in string s) => TryParsePersian(s, out _);

        /// <summary>
        ///     Raises the invalid type cast exception.
        /// </summary>
        /// <returns></returns>
        private static InvalidCastException RaiseInvalidTypeCastException()
        {
            var targetType = CodeHelper.GetCallerMethodName().Substring(2);
            return new InvalidCastException($"Unable to cast PersianDateTime to {targetType}");
        }

        public void Deconstruct(out int year, out int month, out int day, out int hour, out int minute, out int second, out double millisecond)
        {
            year = this.Year;
            month = this.Month;
            day = this.Day;
            hour = this.Hour;
            minute = this.Minute;
            second = this.Second;
            millisecond = this.Millisecond;
        }

        public void Deconstruct(out int year, out int month, out int day)
        {
            year = this.Year;
            month = this.Month;
            day = this.Day;
        }

        public void Deconstruct(out int hour, out int minute, out int second, out double millisecond)
        {
            hour = this.Hour;
            minute = this.Minute;
            second = this.Second;
            millisecond = this.Millisecond;
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public string? ToString(string format)
        {
            if (!this.IsInitiated)
            {
                return base.ToString();
            }

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
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string? ToString() => this.ToString(ToStringFormat);

        /// <summary>
        ///     Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">
        ///     The <see cref="object" /> to compare with this instance.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (!(obj is PersianDateTime))
            {
                return false;
            }

            var target = (PersianDateTime)obj;
            return this.CompareTo(target) == 0;
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode() => this.Data.GetHashCode();

        public string ToDateString(string? separator = null)
        {
            if (separator == null)
            {
                separator = DateSeparator;
            }

            return string.Concat(this.Year.ToString("0000", CultureInfo.CurrentCulture),
                separator,
                this.Month.ToString("00", CultureInfo.CurrentCulture),
                separator,
                this.Day.ToString("00", CultureInfo.CurrentCulture));
        }

        public string ToTimeString(string? separator = null)
        {
            if (separator == null)
            {
                separator = TimeSeparator;
            }

            return string.Concat(this.Hour.ToString("00", CultureInfo.CurrentCulture),
                separator,
                this.Minute.ToString("00", CultureInfo.CurrentCulture),
                separator,
                this.Second.ToString("00", CultureInfo.CurrentCulture));
        }

        /// <summary>
        ///     Converts to the DateTime.
        /// </summary>
        /// <returns></returns>
        public DateTime ToDateTime() => this;

        /// <summary>
        ///     Adds the specified PersianDate time.
        /// </summary>
        /// <param name="persianDateTime">The PersianDate time.</param>
        /// <returns></returns>
        public PersianDateTime Add(in PersianDateTime persianDateTime) => Add(this, persianDateTime);
    }
}