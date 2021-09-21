using System.Collections;

namespace Library.CodeGeneration.Models;

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
    IEnumerator IEnumerable.GetEnumerator() 
        => this.GetEnumerator();
}
