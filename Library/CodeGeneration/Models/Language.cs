﻿namespace Library.CodeGeneration.Models;

public readonly struct Language : IEquatable<Language>
{
    public Language(in string name, in string? fileExtension = null)
        => (this.Name, this.FileExtension) = (name.ArgumentNotNull(), fileExtension);

    public string Name { get; init; }
    public string? FileExtension { get; init; }

    public override bool Equals(object? obj) => obj is Language language && this.Equals(language);
    public bool Equals(Language other) => this.Name == other.Name && this.FileExtension == other.FileExtension;
    public override int GetHashCode() => HashCode.Combine(this.Name, this.FileExtension);

    public static bool operator ==(Language left, Language right) => left.Equals(right);
    public static bool operator !=(Language left, Language right) => !(left == right);
}
