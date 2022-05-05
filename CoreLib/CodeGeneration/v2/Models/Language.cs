using System.Diagnostics;
using Library.DesignPatterns.Markers;
using Library.Validations;

namespace Library.CodeGeneration.v2;

[Immutable]
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public readonly struct Language : IEquatable<Language>
{
    public Language(in string name, in string? fileExtension = null)
        => (this.Name, this.FileExtension) = (name.ArgumentNotNull(nameof(name)), fileExtension);
    public void Deconstruct(out string name, out string? fileExtension)
        => (name, fileExtension) = (this.Name, this.FileExtension);

    public string? FileExtension { get; init; }
    public string Name { get; init; }

    public static bool operator !=(Language left, Language right)
        => !(left == right);

    public static bool operator ==(Language left, Language right)
        => left.Equals(right);

    public override bool Equals(object? obj)
        => obj is Language language && this.Equals(language);

    public bool Equals(Language other)
        => this.Name == other.Name && this.FileExtension == other.FileExtension;

    public override int GetHashCode()
        => HashCode.Combine(this.Name, this.FileExtension);

    private string GetDebuggerDisplay()
        => this.Name;
}
