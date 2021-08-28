using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using Library.Globalization.DataTypes;

namespace Library.Globalization;
[Serializable]
[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
// Created: 85/5/4
public readonly struct PersianDateTime : ICloneable, IComparable<PersianDateTime>, IEquatable<PersianDateTime>,
        IConvertible, ISerializable
{
    #region Fields

    internal readonly PersianDateTimeData Data;
    internal static PersianCalendar PersianCalendar = new();

    #endregion

    /// <summary>
    ///     Initializes a new instance of the <see cref="PersianDateTime" /> struct.
    /// </summary>
    /// <param name="dateTime"> A DateTime instance. </param>
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
    /// <param name="year"> The year. </param>
    /// <param name="month"> The month. </param>
    /// <param name="day"> The day. </param>
    /// <param name="hour"> The hour. </param>
    /// <param name="minute"> The minute. </param>
    /// <param name="second"> The second. </param>
    /// <param name="millisecond"> </param>
    public PersianDateTime(in int year,
        in int month,
        in int day,
        in int hour,
        in int minute,
        in int second,
        in double millisecond)
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

    private PersianDateTime(in PersianDateTimeData data)
        => (this.Data, this.IsInitiated) = (data, true);

    private PersianDateTime(in SerializationInfo info, in StreamingContext context)
    {
        Check.IfArgumentNotNull(info, nameof(info));

        this.Data = (PersianDateTimeData)info.GetValue("Data", typeof(PersianDateTimeData))!;
        this.IsInitiated = true;
    }

    public static string DateSeparator { get; set; } = CultureConstants.DATE_SEPARATOR;

    /// <summary>
    ///     Gets the days of week names.
    /// </summary>
    /// <value> The month names. </value>
    public static IEnumerable<string> DaysOfWeek => EnumHelper.GetDescriptions(EnumHelper.GetItems<PersianDayOfWeek>())!;

    /// <summary>
    ///     Gets the month names.
    /// </summary>
    /// <value> The month names. </value>
    public static IEnumerable<string> MonthNames => EnumHelper.GetDescriptions(EnumHelper.GetItems<PersianMonth>())!;

    /// <summary>
    ///     Gets a <see cref="T:HanyCo.Mes20.Infra.Globalization.PersianDateTime" /> object that is
    ///     set to the current date and time on this computer, expressed as the local time.
    /// </summary>
    /// <returns> An object whose value is the current local date and time. </returns>
    public static PersianDateTime Now => new(DateTime.Now);

    /// <summary>
    ///     Gets the persian week days.
    /// </summary>
    /// <value> The persian week days. </value>
    public static IEnumerable<(DayOfWeek Day, string Name)> PersianWeekDays
    {
        get
        {
            yield return (System.DayOfWeek.Saturday, CultureConstants.SHANBE_FA);
            yield return (System.DayOfWeek.Sunday, CultureConstants.YEK_SHANBE_FA);
            yield return (System.DayOfWeek.Monday, CultureConstants.DO_SHANBE_FA);
            yield return (System.DayOfWeek.Tuesday, CultureConstants.SE_SHANBE_FA);
            yield return (System.DayOfWeek.Wednesday, CultureConstants.CHAHAR_SHANBE_FA);
            yield return (System.DayOfWeek.Thursday, CultureConstants.PANJ_SHANBE_FA);
            yield return (System.DayOfWeek.Friday, CultureConstants.JOME_FA);
        }
    }

    /// <summary>
    ///     Gets the persian week days abbreviations.
    /// </summary>
    /// <value> The persian week days abbreviations. </value>
    public static IEnumerable<(DayOfWeek Day, string Name)> PersianWeekDaysAbbrs
    {
        get
        {
            yield return (System.DayOfWeek.Saturday, CultureConstants.SHANBE_ABBR_FA);
            yield return (System.DayOfWeek.Sunday, CultureConstants.YEK_SHANBE_ABBR_FA);
            yield return (System.DayOfWeek.Monday, CultureConstants.DO_SHANBE_ABBR_FA);
            yield return (System.DayOfWeek.Tuesday, CultureConstants.SE_SHANBE_ABBR_FA);
            yield return (System.DayOfWeek.Wednesday, CultureConstants.CHAHAR_SHANBE_ABBR_FA);
            yield return (System.DayOfWeek.Thursday, CultureConstants.PANJ_SHANBE_ABBR_FA);
            yield return (System.DayOfWeek.Friday, CultureConstants.JOME_ABBR_FA);
        }
    }

    public static string ShortDatePattern { get; set; } = CultureConstants.SHORT_DATE_PATTERN;

    public static string TimeSeparator { get; set; } = CultureConstants.TIME_SEPARATOR;

    public static string ToStringFormat { get; set; } = CultureConstants.DEFAULT_DATE_TIME_PATTERN;

    /// <summary>
    ///     Gets the day.
    /// </summary>
    /// <value> The day. </value>
    public int Day => this.Data.Day;

    /// <summary>
    ///     Gets the day of week.
    /// </summary>
    /// <value> The day of week. </value>
    public PersianDayOfWeek DayOfWeek => (PersianDayOfWeek)PersianCalendar.GetDayOfWeek(this).ToInt();

    /// <summary>
    ///     Gets the day of week title.
    /// </summary>
    /// <value> The day of week title. </value>
    public string DayOfWeekTitle => EnumHelper.GetItemDescription(this.DayOfWeek)!;

    /// <summary>
    ///     Gets the hour.
    /// </summary>
    /// <value> The hour. </value>
    public int Hour => this.Data.Hour;

    public bool IsInitiated { get; }

    public double Millisecond => this.Data.Millisecond;

    /// <summary>
    ///     Gets the minute.
    /// </summary>
    /// <value> The minute. </value>
    public int Minute => this.Data.Minute;

    /// <summary>
    ///     Gets the month.
    /// </summary>
    /// <value> The month. </value>
    public int Month => this.Data.Month;

    /// <summary>
    ///     Gets the second.
    /// </summary>
    /// <value> The second. </value>
    public int Second => this.Data.Second;

    /// <summary>
    ///     Returns the tick count for this DateTime. The returned value is the number of
    ///     100-nanosecond intervals that have elapsed since 1/1/0001 12:00am.
    /// </summary>
    public long Ticks => ((DateTime)this).Ticks;

    /// <summary>
    ///     Gets the year.
    /// </summary>
    /// <value> The year. </value>
    public int Year => this.Data.Year;

    internal static IEnumerable<string> EnglishMonthNameAbbrsInPersian
    {
        get
        {
            yield return CultureConstants.FARVARDIN_ABBR_EN;
            yield return CultureConstants.ORDIBEHESHT_ABBR_EN;
            yield return CultureConstants.KHORDAD_ABBR_EN;
            yield return CultureConstants.TIR_ABBR_EN;
            yield return CultureConstants.MORDAD_ABBR_EN;
            yield return CultureConstants.SHAHRIVAR_ABBR_EN;
            yield return CultureConstants.MEHR_ABBR_EN;
            yield return CultureConstants.ABAN_ABBR_EN;
            yield return CultureConstants.AZAR_ABBR_EN;
            yield return CultureConstants.DEY_ABBR_EN;
            yield return CultureConstants.BAHMAN_ABBR_EN;
            yield return CultureConstants.ESFAND_ABBR_EN;
        }
    }

    internal static IEnumerable<string> PersianMonthNameAbbrsInPersian
    {
        get
        {
            yield return CultureConstants.FARVARDIN_ABBR_FA;
            yield return CultureConstants.ORDIBEHESHT_ABBR_FA;
            yield return CultureConstants.KHORDAD_ABBR_FA;
            yield return CultureConstants.TIR_ABBR_FA;
            yield return CultureConstants.MORDAD_ABBR_FA;
            yield return CultureConstants.SHAHRIVAR_ABBR_FA;
            yield return CultureConstants.MEHR_ABBR_FA;
            yield return CultureConstants.ABAN_ABBR_FA;
            yield return CultureConstants.AZAR_ABBR_FA;
            yield return CultureConstants.DEY_ABBR_FA;
            yield return CultureConstants.BAHMAN_ABBR_FA;
            yield return CultureConstants.ESFAND_ABBR_FA;
        }
    }

    internal static IEnumerable<string> PersianMonthNamesInGenitive
    {
        get
        {
            yield return CultureConstants.FARVARDIN_EN;
            yield return CultureConstants.ORDIBEHESHT_EN;
            yield return CultureConstants.KHORDAD_EN;
            yield return CultureConstants.TIR_EN;
            yield return CultureConstants.MORDAD_EN;
            yield return CultureConstants.SHAHRIVAR_EN;
            yield return CultureConstants.MEHR_EN;
            yield return CultureConstants.ABAN_EN;
            yield return CultureConstants.AZAR_EN;
            yield return CultureConstants.DEY_EN;
            yield return CultureConstants.BAHMAN_EN;
            yield return CultureConstants.ESFAND_EN;
        }
    }

    internal static IEnumerable<string> PersianMonthNamesInPersian
    {
        get
        {
            yield return CultureConstants.FARVARDIN_FA;
            yield return CultureConstants.ORDIBEHESHT_FA;
            yield return CultureConstants.KHORDAD_FA;
            yield return CultureConstants.TIR_FA;
            yield return CultureConstants.MORDAD_FA;
            yield return CultureConstants.SHAHRIVAR_FA;
            yield return CultureConstants.MEHR_FA;
            yield return CultureConstants.ABAN_FA;
            yield return CultureConstants.AZAR_FA;
            yield return CultureConstants.DEY_FA;
            yield return CultureConstants.BAHMAN_FA;
            yield return CultureConstants.ESFAND_FA;
        }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string? DebuggerDisplay
    {
        get
        {
            var self = this;
            return self.ToCode();
        }
    }

    /// <summary>
    ///     Adds the specified persian date time1.
    /// </summary>
    /// <param name="persianDateTime1"> The persian date time1. </param>
    /// <param name="persianDateTime2"> The persian date time2. </param>
    /// <returns> </returns>
    public static PersianDateTime Add(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2)
        => persianDateTime1 + persianDateTime2;

    /// <summary>
    ///     Compares the specified persian date time1.
    /// </summary>
    /// <param name="persianDateTime1"> The persian date time1. </param>
    /// <param name="persianDateTime2"> The persian date time2. </param>
    /// <returns> </returns>
    /// <exception cref="InvalidCastException">
    ///     cannot cast persianDateTime1 to PersianDateTime or cannot cast persianDateTime2 to PersianDateTime
    /// </exception>
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

    public static bool IsPersianDateTime(in string s) => TryParsePersian(s, out _);

    /// <summary>
    ///     Parses the date time.
    /// </summary>
    /// <param name="dateTime"> The date time. </param>
    /// <returns> </returns>
    public static PersianDateTime ParseDateTime(in DateTime dateTime) => new(dateTime);

    /// <summary>
    ///     Parses the english string datetime.
    /// </summary>
    /// <param name="dateTimeString"> The date time string. </param>
    /// <returns> </returns>
    public static PersianDateTime ParseEnglish(in string dateTimeString)
    {
        Check.IfArgumentNotNull(dateTimeString, nameof(dateTimeString));
        return DateTime.Parse(dateTimeString, CultureInfo.CurrentCulture).ToPersianDateTime();
    }

    /// <summary>
    ///     Parses the persian.
    /// </summary>
    /// <param name="dateTimeString"> The date time string. </param>
    /// <returns> </returns>
    /// <exception cref="ArgumentException">
    ///     not valid date - dateTimeString or not valid date - dateTimeString or not valid date -
    ///     dateTimeString or not valid date - dateTimeString or not valid date - dateTimeString
    ///     or not valid date - dateTimeString
    /// </exception>
    public static PersianDateTime ParsePersian(in string dateTimeString)
    {
        Check.IfArgumentNotNull(dateTimeString, nameof(dateTimeString));
        var data = new PersianDateTimeData
        {
            HasDate = dateTimeString.IndexOf(DateSeparator, StringComparison.Ordinal) > 0,
            HasTime = dateTimeString.IndexOf(":", StringComparison.Ordinal) > 0
        };
        var indexOfSpace = dateTimeString.IndexOf(" ", StringComparison.Ordinal);
        if (data.HasDate)
        {
            var datePart = data.HasTime ? dateTimeString.Substring(0, indexOfSpace) : dateTimeString;
            if (int.TryParse(datePart.AsSpan(0, datePart.IndexOf(DateSeparator, StringComparison.Ordinal)),
                out data.Year) is false)
            {
                throw new ArgumentException("not valid date", nameof(dateTimeString));
            }

            datePart = datePart.Remove(0, datePart.IndexOf(DateSeparator, StringComparison.Ordinal) + 1);
            if (int.TryParse(datePart.AsSpan(0, datePart.IndexOf(DateSeparator, StringComparison.Ordinal)),
                out data.Month) is false)
            {
                throw new ArgumentException("not valid date", nameof(dateTimeString));
            }

            datePart = datePart.Remove(0, datePart.IndexOf(DateSeparator, StringComparison.Ordinal) + 1);
            if (int.TryParse(datePart, out data.Day) is false)
            {
                throw new ArgumentException("not valid date", nameof(dateTimeString));
            }
        }

        if (data.HasTime)
        {
            var timePart = data.HasDate ? dateTimeString[indexOfSpace..] : dateTimeString;
            if (int.TryParse(timePart.AsSpan(0, timePart.IndexOf(TimeSeparator, StringComparison.Ordinal)),
                out data.Hour) is false)
            {
                throw new ArgumentException("not valid date", nameof(dateTimeString));
            }

            timePart = timePart.Remove(0, timePart.IndexOf(TimeSeparator, StringComparison.Ordinal) + 1);
            if (int.TryParse(timePart, out data.Minute) is false)
            {
                throw new ArgumentException("not valid date", nameof(dateTimeString));
            }
        }

        return data.ToString()?.Equals("00:00:00 0000/00/00", StringComparison.Ordinal) switch
        {
            not false => throw new ArgumentException("not valid date", nameof(dateTimeString)),
            _ => new PersianDateTime(data)
        };
    }

    /// <summary>
    ///     Subtracts the specified persian date time1.
    /// </summary>
    /// <param name="persianDateTime1"> The persian date time1. </param>
    /// <param name="persianDateTime2"> The persian date time2. </param>
    /// <returns> </returns>
    public static PersianDateTime Subtract(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2)
        => persianDateTime1 - persianDateTime2;

    /// <summary>
    ///     Converts to persian.
    /// </summary>
    /// <param name="dateTime"> The date time. </param>
    /// <returns> </returns>
    public static string ToPersian(in DateTime dateTime) => new PersianDateTime(dateTime).ToCode()!;

    public static (PersianDateTime Result, bool Succeed) TryParsePersian(in string str)
    {
        var succeed = TryParsePersian(str, out var result);
        return (result, succeed);
    }

    /// <summary>
    ///     Tries to parse a string to PersianDateTime.
    /// </summary>
    /// <param name="str"> The STR. </param>
    /// <param name="result"> The result. </param>
    /// <returns> </returns>
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

    /// <summary>
    ///     Adds the specified PersianDate time.
    /// </summary>
    /// <param name="persianDateTime"> The PersianDate time. </param>
    /// <returns> </returns>
    public PersianDateTime Add(in PersianDateTime persianDateTime)
        => Add(this, persianDateTime);

    /// <summary>
    ///     Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns> A new object that is a copy of this instance. </returns>
    public object Clone() => new PersianDateTime(this.Year,
        this.Month,
        this.Day,
        this.Hour,
        this.Minute,
        this.Second,
        this.Millisecond);

    /// <summary>
    ///     Compares the current instance with another object of the same type and returns an
    ///     integer that indicates whether the current instance precedes, follows, or occurs in
    ///     the same position in the sort order as the other object.
    /// </summary>
    /// <param name="other"> An object to compare with this instance. </param>
    /// <returns>
    ///     A value that indicates the relative order of the objects being compared. The return
    ///     value has these meanings: Value Meaning Less than zero This instance precedes
    ///     <paramref name="other" /> in the sort order. Zero This instance occurs in the same
    ///     position in the sort order as <paramref name="other" />. Greater than zero This
    ///     instance follows <paramref name="other" /> in the sort order.
    /// </returns>
    public int CompareTo(PersianDateTime other) => DateTime.Compare(this, other);

    /// <summary>
    ///     Deconstructs this instance.
    /// </summary>
    /// <param name="hour"> The hour. </param>
    /// <param name="minute"> The minute. </param>
    /// <param name="second"> The second. </param>
    /// <param name="millisecond"> The millisecond. </param>
    public void Deconstruct(out int hour, out int minute, out int second, out double millisecond)
    {
        hour = this.Hour;
        minute = this.Minute;
        second = this.Second;
        millisecond = this.Millisecond;
    }

    /// <summary>
    ///     Deconstructs this instance.
    /// </summary>
    /// <param name="year"> The year. </param>
    /// <param name="month"> The month. </param>
    /// <param name="day"> The day. </param>
    public void Deconstruct(out int year, out int month, out int day)
    {
        year = this.Year;
        month = this.Month;
        day = this.Day;
    }

    /// <summary>
    ///     Deconstructs this instance.
    /// </summary>
    /// <param name="year"> The year. </param>
    /// <param name="month"> The month. </param>
    /// <param name="day"> The day. </param>
    /// <param name="hour"> The hour. </param>
    /// <param name="minute"> The minute. </param>
    /// <param name="second"> The second. </param>
    /// <param name="millisecond"> The millisecond. </param>
    public void Deconstruct(out int year,
        out int month,
        out int day,
        out int hour,
        out int minute,
        out int second,
        out double millisecond)
    {
        year = this.Year;
        month = this.Month;
        day = this.Day;
        hour = this.Hour;
        minute = this.Minute;
        second = this.Second;
        millisecond = this.Millisecond;
    }

    /// <summary>
    ///     Determines whether the specified <see cref="object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj"> The <see cref="object" /> to compare with this instance. </param>
    /// <returns>
    ///     <c> true </c> if the specified <see cref="object" /> is equal to this instance;
    ///     otherwise, <c> false </c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is not PersianDateTime)
        {
            return false;
        }

        var target = (PersianDateTime)obj;
        return this.CompareTo(target) == 0;
    }

    public bool Equals(PersianDateTime other) => this.CompareTo(other) is 0;

    /// <summary>
    ///     Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    ///     A hash code for this instance, suitable for use in hashing algorithms and data
    ///     structures like a hash table.
    /// </returns>
    public override int GetHashCode() => this.Data.GetHashCode();

    /// <summary>
    ///     Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns> A <see cref="string" /> that represents this instance. </returns>
    public string? ToCode() => this.ToString(ToStringFormat);

    /// <summary>
    ///     Converts to persian date string.
    /// </summary>
    /// <param name="separator"> The separator. </param>
    /// <returns> </returns>
    public string ToDateString(string? separator = null)
    {
        separator ??= DateSeparator;

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
    public DateTime ToDateTime() => this;

    /// <summary>
    ///     Converts the value of this instance to an equivalent <see cref="T:System.DateTime" />
    ///     using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="provider">
    ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
    ///     culture-specific formatting information.
    /// </param>
    /// <returns>
    ///     A <see cref="T:System.DateTime" /> instance equivalent to the value of this instance.
    /// </returns>
    public DateTime ToDateTime(IFormatProvider? provider) => this;

    /// <summary>
    ///     Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <param name="format"> The format. </param>
    /// <returns> A <see cref="string" /> that represents this instance. </returns>
    public string? ToString(string format)
    {
        if (this.IsInitiated is false)
        {
            return this.ToString();
        }

        _ = Check.ArgumentNotNull(format, "format");
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
    ///     Converts to persian time string.
    /// </summary>
    /// <param name="separator"> The separator. </param>
    /// <returns> </returns>
    public string ToTimeString(string? separator = null)
    {
        separator ??= TimeSeparator;
        return string.Concat(this.Hour.ToString("00", CultureInfo.CurrentCulture),
            separator,
            this.Minute.ToString("00", CultureInfo.CurrentCulture),
            separator,
            this.Second.ToString("00", CultureInfo.CurrentCulture));
    }

    /// <summary>
    ///     Raises the invalid type cast exception.
    /// </summary>
    /// <returns> </returns>
    private static InvalidCastException RaiseInvalidTypeCastException()
    {
        var targetType = GetCallerMethodName()![2..];
        return new InvalidCastException($"Unable to cast PersianDateTime to {targetType}");
    }

    private string GetDebuggerDisplay() => this.ToString()!;

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => info.AddValue("Data", this.Data);

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

    object IConvertible.ToType(Type conversionType, IFormatProvider? provider)
        => throw RaiseInvalidTypeCastException();

    ushort IConvertible.ToUInt16(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

    uint IConvertible.ToUInt32(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();

    ulong IConvertible.ToUInt64(IFormatProvider? provider) => throw RaiseInvalidTypeCastException();


    /// <summary>
    ///     Performs an implicit conversion from <see cref="PersianDateTime" /> to <see cref="DateTime" />.
    /// </summary>
    /// <param name="persianDateTime"> The persian date time. </param>
    /// <returns> The result of the conversion. </returns>
    public static implicit operator DateTime(in PersianDateTime persianDateTime) => PersianCalendar.ToDateTime(
        persianDateTime.Year,
        persianDateTime.Month,
        persianDateTime.Day,
        persianDateTime.Hour,
        persianDateTime.Minute,
        persianDateTime.Second,
        0);

    public static implicit operator PersianDateTime(in string persianDateTimeString)
        => ParsePersian(persianDateTimeString);

    /// <summary>
    ///     Performs an implicit conversion from <see cref="DateTime" /> to <see cref="PersianDateTime" />.
    /// </summary>
    /// <param name="dateTime"> The date time. </param>
    /// <returns> The result of the conversion. </returns>
    public static implicit operator PersianDateTime(in DateTime dateTime) => dateTime.ToPersianDateTime();

    /// <summary>
    ///     Performs an implicit conversion from <see cref="PersianDateTime" /> to <see cref="string" />.
    /// </summary>
    /// <param name="persianDateTime"> The persian date time. </param>
    /// <returns> The result of the conversion. </returns>
    public static implicit operator string?(in PersianDateTime persianDateTime) => persianDateTime.ToCode();

    /// <summary>
    ///     Implements the operator -.
    /// </summary>
    /// <param name="persianDateTime1"> The persian date time1. </param>
    /// <param name="persianDateTime2"> The persian date time2. </param>
    /// <returns> The result of the operator. </returns>
    public static PersianDateTime operator -(in PersianDateTime persianDateTime1,
        in PersianDateTime persianDateTime2)
    {
        DateTime dateTime1 = persianDateTime1;
        DateTime dateTime2 = persianDateTime2;
        return dateTime1.Subtract(new TimeSpan(dateTime2.Ticks)).ToPersianDateTime();
    }

    /// <summary>
    ///     Implements the operator !=.
    /// </summary>
    /// <param name="persianDateTime1"> The persian date time1. </param>
    /// <param name="persianDateTime2"> The persian date time2. </param>
    /// <returns> The result of the operator. </returns>
    public static bool operator !=(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2)
        => !persianDateTime1.Equals(persianDateTime2);

    /// <summary>
    ///     Implements the operator +.
    /// </summary>
    /// <param name="persianDateTime1"> The persian date time1. </param>
    /// <param name="persianDateTime2"> The persian date time2. </param>
    /// <returns> The result of the operator. </returns>
    public static PersianDateTime operator +(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2)
    {
        DateTime dateTime1 = persianDateTime1;
        DateTime dateTime2 = persianDateTime2;
        return dateTime1.Add(new TimeSpan(dateTime2.Ticks)).ToPersianDateTime();
    }

    /// <summary>
    ///     Implements the operator &lt;.
    /// </summary>
    /// <param name="left"> The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operator. </returns>
    public static bool operator <(in PersianDateTime left, in PersianDateTime right) => left.CompareTo(right) < 0;

    /// <summary>
    ///     Implements the operator &lt;=.
    /// </summary>
    /// <param name="left"> The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operator. </returns>
    public static bool operator <=(in PersianDateTime left, in PersianDateTime right) => left.CompareTo(right) <= 0;

    /// <summary>
    ///     Implements the operator ==.
    /// </summary>
    /// <param name="persianDateTime1"> The persian date time1. </param>
    /// <param name="persianDateTime2"> The persian date time2. </param>
    /// <returns> The result of the operator. </returns>
    public static bool operator ==(in PersianDateTime persianDateTime1, in PersianDateTime persianDateTime2)
        => persianDateTime1.Equals(persianDateTime2);

    /// <summary>
    ///     Implements the operator &gt;.
    /// </summary>
    /// <param name="left"> The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operator. </returns>
    public static bool operator >(in PersianDateTime left, in PersianDateTime right) => left.CompareTo(right) > 0;

    /// <summary>
    ///     Implements the operator &gt;=.
    /// </summary>
    /// <param name="left"> The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> The result of the operator. </returns>
    public static bool operator >=(in PersianDateTime left, in PersianDateTime right) => left.CompareTo(right) >= 0;
}