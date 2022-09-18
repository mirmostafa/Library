using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;

using Library.Globalization.DataTypes;
using Library.Helpers;
using Library.Interfaces;
using Library.Results;
using Library.Validations;

namespace Library.Globalization;

public readonly struct PersianDate
       : IComparable,
         IComparable<PersianDate>,
         IEquatable<PersianDate>,
         ISpanFormattable
{
    private readonly PersianDateTime _date;

    public int CompareTo(object? obj)
        => throw new NotImplementedException();

    public int CompareTo(PersianDate other)
        => throw new NotImplementedException();

    public bool Equals(PersianDate other)
        => throw new NotImplementedException();

    public string ToString(string? format, IFormatProvider? formatProvider)
        => throw new NotImplementedException();

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => throw new NotImplementedException();
}