using System.Diagnostics;

using Library.Validations;

namespace Library.CodeGeneration.Models;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public readonly struct Language(in string name, in string? fileExtension = null) : IEquatable<Language>
{
    public string? FileExtension { get; init; } = fileExtension;

    public string Name { get; init; } = name.ArgumentNotNull();

    public static bool operator !=(Language left, Language right) => !(left == right);

    public static bool operator ==(Language left, Language right) => left.Equals(right);

    public override bool Equals(object? obj) =>
        obj is Language language && this.Equals(language);

    public bool Equals(Language other) =>
        this.Name == other.Name && this.FileExtension == other.FileExtension;

    public override int GetHashCode() =>
        HashCode.Combine(this.Name, this.FileExtension);

    public override string ToString() =>
        this.Name;

    private string GetDebuggerDisplay() =>
        this.ToString();
}