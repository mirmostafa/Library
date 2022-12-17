using System.Diagnostics;

using Library.DesignPatterns.Markers;
using Library.Validations;

namespace Library.CodeGeneration.v2;

/// <summary>
///
/// </summary>
/// <seealso cref="IEquatable&lt;Language&gt;" />
[Immutable]
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public readonly struct Language : IEquatable<Language>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Language"/> struct.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="fileExtension">The file extension.</param>
    public Language(in string name, in string? fileExtension = null)
        => (this.Name, this.FileExtension) = (name.ArgumentNotNull(nameof(name)), fileExtension);

    /// <summary>
    /// Gets the file extension.
    /// </summary>
    /// <value>
    /// The file extension.
    /// </value>
    public string? FileExtension { get; }

    /// <summary>
    /// Gets the name of language.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; }

    public static bool operator !=(Language left, Language right)
        => !(left == right);

    /// <summary>
    /// Implements the operator ==.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    public static bool operator ==(Language left, Language right)
        => left.Equals(right);

    /// <summary>
    /// Deconstructs the specified name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="fileExtension">The file extension.</param>
    public void Deconstruct(out string name, out string? fileExtension)
        => (name, fileExtension) = (this.Name, this.FileExtension);

    /// <summary>
    /// Indicates whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    ///   <see langword="true" /> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <see langword="false" />.
    /// </returns>
    public override bool Equals(object? obj)
        => obj is Language language && this.Equals(language);

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(Language other)
        => this.Name == other.Name && this.FileExtension == other.FileExtension;

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public override int GetHashCode()
        => HashCode.Combine(this.Name, this.FileExtension);

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="string" /> that represents this instance.
    /// </returns>
    public override string ToString()
        => this.Name;

    /// <summary>
    /// Gets the debugger display.
    /// </summary>
    /// <returns></returns>
    private string GetDebuggerDisplay()
        => this.ToString();
}