using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

using Library.DesignPatterns.Markers;
using Library.Interfaces;

#if USE_LONG_ID
using IdType = System.Int64;
#else

using IdType = System.Guid;

#endif

namespace Library.Types;

[Immutable]
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public readonly struct Id(IdType value) :
    ISpanFormattable, IFormattable, ISerializable, ICloneable, IComparable,
    IEquatable<IdType>, IComparable<Id>, IComparable<IdType>,
    IConvertible<IdType>, IEmpty<Id>, IEquatable<Id>
#if USE_LONG_ID
    , IMinMaxValue<Id>
#endif
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Id"/> struct.
    /// </summary>
    public Id()
        : this(GetDefaultValue()) { }

    /// <summary>
    /// Gets an empty instance of current class.
    /// </summary>
    /// <value>An empty instance.</value>
    public static Id Empty { get; } = new(GetDefaultValue());

    /// <summary>
    /// Gets the unique identifier.
    /// </summary>
    /// <value>The unique identifier.</value>
    public IdType Value { get; } = value;
#if USE_LONG_ID
    public static Id MaxValue { get; } = IdType.MaxValue;
    public static Id MinValue { get; } = IdType.MinValue;
#endif
    /// <summary>
    /// Creates a new Id the by the specific identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    public static Id Create(IdType id)
        => new(id);

    /// <summary>
    /// Performs an implicit conversion from <see cref="IdType"/> to <see cref="Id"/>.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator Id(IdType id)
        => new(id);

    /// <summary>
    /// Performs an implicit conversion from <see cref="Id"/> to <see cref="IdType"/>.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator IdType(Id id) =>
        id.Value;

    /// <summary>
    /// Creates new empty.
    /// </summary>
    /// <returns>An empty instance of current class.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public static Id NewEmpty() =>
        new(GetDefaultValue());

    /// <summary>
    /// Implements the operator !=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(Id left, Id right) =>
        !(left == right);

    /// <summary>
    /// Implements the operator !=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(Id left, IdType right)
        => !(left == right);

    /// <summary>
    /// Implements the operator &lt;.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator <(Id left, Id right)
        => left.CompareTo(right) < 0;

    /// <summary>
    /// Implements the operator &lt;.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator <(Id left, IdType right)
        => left.CompareTo(right) < 0;

    /// <summary>
    /// Implements the operator &lt;=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator <=(Id left, Id right)
        => left.CompareTo(right) <= 0;

    /// <summary>
    /// Implements the operator &lt;=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator <=(Id left, IdType right)
        => left.CompareTo(right) <= 0;

    /// <summary>
    /// Implements the operator ==.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(Id left, Id right)
        => left.Equals(right);

    /// <summary>
    /// Implements the operator ==.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(Id left, IdType right)
        => left.Equals(right);

    /// <summary>
    /// Implements the operator &gt;.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator >(Id left, Id right)
        => left.CompareTo(right) > 0;

    /// <summary>
    /// Implements the operator &gt;.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator >(Id left, IdType right)
        => left.CompareTo(right) > 0;

    /// <summary>
    /// Implements the operator &gt;=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator >=(Id left, Id right)
        => left.CompareTo(right) >= 0;

    /// <summary>
    /// Implements the operator &gt;=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator >=(Id left, IdType right)
        => left.CompareTo(right) >= 0;

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public object Clone()
        => new Id(this.Value);

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer
    /// that indicates whether the current instance precedes, follows, or occurs in the same
    /// position in the sort order as the other object.
    /// </summary>
    /// <param name="obj">An object to compare with this instance.</param>
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
    /// <description>This instance precedes <paramref name="obj"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Zero</term>
    /// <description>
    /// This instance occurs in the same position in the sort order as <paramref name="obj"/>.
    /// </description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description>This instance follows <paramref name="obj"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    public int CompareTo(object? obj)
        => this.Value.CompareTo(obj);

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
    public int CompareTo(Id other)
        => this.Value.CompareTo(other.Value);

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
    public int CompareTo(IdType other)
        => this.Value.CompareTo(other);

    public IdType Convert()
            => throw new NotImplementedException();

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true"/> if the current object is equal to the <paramref name="other"/>
    /// parameter; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(IdType other)
        => this.Value.Equals(other);

    /// <summary>
    /// Determines whether the specified <see cref="object"/>, is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="NotImplementedException"></exception>
    public override bool Equals(object? obj)
        => obj is Id id && this.Equals(id);

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true"/> if the current object is equal to the <paramref name="other"/>
    /// parameter; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(Id other)
        => this.Value.Equals(other.Value);

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures
    /// like a hash table.
    /// </returns>
    /// <exception cref="NotImplementedException"></exception>
    public override int GetHashCode()
        => this.Value.GetHashCode();

    /// <summary>
    /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data
    /// needed to serialize the target object.
    /// </summary>
    /// <param name="info">
    /// The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data.
    /// </param>
    /// <param name="context">
    /// The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.
    /// </param>
    /// <exception cref="NotImplementedException"></exception>
    public void GetObjectData(SerializationInfo info, StreamingContext context)
        => info.AddValue(nameof(this.Value), this.Value);

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public string ToString(string? format, IFormatProvider? formatProvider)
        => this.ToString();

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    public override string ToString()
        => this.Value.ToString();

    /// <summary>
    /// Tries to format the value of the current instance into the provided span of characters.
    /// </summary>
    /// <param name="destination">
    /// When this method returns, this instance's value formatted as a span of characters.
    /// </param>
    /// <param name="charsWritten">
    /// When this method returns, the number of characters that were written in <paramref name="destination"/>.
    /// </param>
    /// <param name="format">
    /// A span containing the characters that represent a standard or custom format string that
    /// defines the acceptable format for <paramref name="destination"/>.
    /// </param>
    /// <param name="provider">
    /// An optional object that supplies culture-specific formatting information for <paramref name="destination"/>.
    /// </param>
    /// <returns><see langword="true"/> if the formatting was successful; otherwise, <see langword="false"/>.</returns>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => this.Value.TryFormat(destination, out charsWritten, format);

    /// <summary>
    /// Gets the default value of IdType.
    /// </summary>
    /// <returns>The default value of IdType</returns>
    private static Id GetDefaultValue()
        => typeof(IdType) == typeof(Guid)
                ? Guid.Empty.Cast().To<Id>()
                : typeof(IdType) == typeof(long)
                    ? 0.Cast().To<Id>()
                    : default;

    /// <summary>
    /// Gets the debugger display.
    /// </summary>
    /// <returns></returns>
    private string GetDebuggerDisplay()
        => this.ToString();
}