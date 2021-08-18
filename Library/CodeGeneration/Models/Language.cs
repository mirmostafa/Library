using System.Collections;

namespace Library.CodeGeneration.Models
{
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

    public readonly struct Languages : IEnumerable<Language>, IEquatable<Languages>
    {
        public static readonly Language None = new("(Unknown)", "");
        public static readonly Language CSharp = new("C#", "cs");
        public static readonly Language Xaml = new("XAML", "xaml.cs");
        public static readonly Language Blazor = new("Blazor", "cshtml.cs");
        public static readonly Language Html = new("HTML", "htm");

        public override bool Equals(object? obj)
            => base.Equals(obj);
        public override int GetHashCode()
            => base.GetHashCode();
        public override string? ToString()
            => base.ToString();

        public static bool operator ==(Languages left, Languages right)
            => left.Equals(right);

        public static bool operator !=(Languages left, Languages right)
            => !(left == right);

        public bool Equals(Languages other)
            => this == other;
        public IEnumerator<Language> GetEnumerator()
        {
            yield return None;
            yield return CSharp;
            yield return Xaml;
            yield return Blazor;
            yield return Html;
        }
        IEnumerator IEnumerable.GetEnumerator() =>
            this.GetEnumerator();
    }
}
