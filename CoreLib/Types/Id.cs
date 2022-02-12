﻿using System.Runtime.Serialization;
using Library.Interfaces;

namespace Library.Types;
public readonly struct Id : IEquatable<Guid>, IComparable, IComparable<Id>, IComparable<Guid>, IConvertible<Guid>, ISpanFormattable, IFormattable, ISerializable, ICloneable, IEmpty<Id>, INew<Id>
{
    public Id(Guid value) =>
        this.Value = value;

    /// <summary>
    /// Gets the unique identifier.
    /// </summary>
    /// <value>
    /// The unique identifier.
    /// </value>
    public Guid Value { get; }
    /// <summary>
    /// Gets an empty instance of currebt class.
    /// </summary>
    /// <value>
    /// An empty instance.
    /// </value>
    public static Id Empty { get; } = new(Guid.Empty);
    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="obj" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="obj" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="obj" /> in the sort order.</description></item></list>
    /// </returns>
    public int CompareTo(object? obj) =>
        this.Value.CompareTo(obj);
    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list>
    /// </returns>
    public int CompareTo(Id other) =>
        this.Value.CompareTo(other.Value);
    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list>
    /// </returns>
    public int CompareTo(Guid other) =>
        this.Value.CompareTo(other);
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(Guid other) =>
        this.Value.Equals(other);
    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>
    /// A <see cref="string" /> that represents this instance.
    /// </returns>
    /// <exception cref="NotImplementedException"></exception>
    public string ToString(string? format, IFormatProvider? formatProvider) =>
        throw new NotImplementedException();

    /// <summary>
    /// Tries to format the value of the current instance into the provided span of characters.
    /// </summary>
    /// <param name="destination">When this method returns, this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination" />.</param>
    /// <param name="format">A span containing the characters that represent a standard or custom format string that defines the acceptable format for <paramref name="destination" />.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <paramref name="destination" />.</param>
    /// <returns>
    ///   <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.
    /// </returns>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) =>
        this.Value.TryFormat(destination, out charsWritten, format);

    /// <summary>
    /// Implements the operator &lt;.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator <(Id left, Id right) =>
        left.CompareTo(right) < 0;
    /// <summary>
    /// Implements the operator &lt;.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator <(Id left, Guid right) =>
        left.CompareTo(right) < 0;
    /// <summary>
    /// Implements the operator &gt;.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator >(Id left, Id right) =>
        left.CompareTo(right) > 0;
    /// <summary>
    /// Implements the operator &gt;.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator >(Id left, Guid right) =>
        left.CompareTo(right) > 0;
    /// <summary>
    /// Implements the operator &lt;=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator <=(Id left, Id right) =>
        left.CompareTo(right) <= 0;
    /// <summary>
    /// Implements the operator &lt;=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator <=(Id left, Guid right) =>
        left.CompareTo(right) <= 0;
    /// <summary>
    /// Implements the operator &gt;=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator >=(Id left, Id right) =>
        left.CompareTo(right) >= 0;
    /// <summary>
    /// Implements the operator &gt;=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator >=(Id left, Guid right)
        => left.CompareTo(right) >= 0;
    /// <summary>
    /// Determines whether the specified <see cref="object" />, is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
    /// <returns>
    ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public override bool Equals(object? obj) =>
        obj is Id id && this.Equals(id);

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
    /// </returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public override int GetHashCode()
        => this.Value.GetHashCode();

    /// <summary>
    /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.
    /// </summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
    /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void GetObjectData(SerializationInfo info, StreamingContext context)
        => throw new NotImplementedException();
    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public object Clone()
        => new Id(this.Value);
    /// <summary>
    /// Creates new empty.
    /// </summary>
    /// <returns>
    /// An empty instance of currebt class.
    /// </returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public static Id NewEmpty() =>
        new(Guid.Empty);
    /// <summary>
    /// Implements the operator ==.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator ==(Id left, Id right) =>
        left.Equals(right);
    /// <summary>
    /// Implements the operator !=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator !=(Id left, Id right) =>
        !(left == right);
    /// <summary>
    /// Implements the operator ==.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator ==(Id left, Guid right) =>
        left.Equals(right);
    /// <summary>
    /// Implements the operator !=.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator !=(Id left, Guid right) =>
        !(left == right);
    /// <summary>
    /// Performs an implicit conversion from <see cref="Id"/> to <see cref="Guid"/>.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static implicit operator Guid(Id id) =>
        id.Value;
    /// <summary>
    /// Performs an implicit conversion from <see cref="Guid"/> to <see cref="Id"/>.
    /// </summary>
    /// <param name="guid">The unique identifier.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static implicit operator Id(Guid guid) =>
        new(guid);    /// <summary>
                      /// Converts to string.
                      /// </summary>
                      /// <returns>
                      /// A <see cref="string" /> that represents this instance.
                      /// </returns>
    public override string ToString() =>
        this.Value.ToString();
}