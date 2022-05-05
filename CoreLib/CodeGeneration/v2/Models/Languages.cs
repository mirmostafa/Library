using System.Collections;
using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.v2;

[Immutable]
public readonly struct Languages : IEnumerable<Language>, IEquatable<Languages>
{
    public static readonly Language Blazor = new("Blazor", "razor.cs");
    public static readonly Language CSharp = new("C#", "cs");
    public static readonly Language Html = new("HTML", "htm");
    public static readonly Language None = new("(Unknown)", "");
    public static readonly Language Xaml = new("XAML", "xaml.cs");

    public static bool operator !=(Languages left, Languages right)
        => !(left == right);

    public static bool operator ==(Languages left, Languages right)
        => left.Equals(right);

    public override bool Equals(object? obj)
                => base.Equals(obj);

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

    public override int GetHashCode()
                => base.GetHashCode();

    public override string? ToString()
        => base.ToString();

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();
}
