using System.Collections;

namespace Library.CodeGeneration.Models;

public readonly struct Languages : IEnumerable<Language>, IEquatable<Languages>
{
    public static readonly Language BlazorCodeBehind = new("Blazor Code", "razor.cs");
    public static readonly Language BlazorFront = new("Blazor HTML", "razor");
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
        var me = this;
        return this.GetType()
            .GetFields()
            .Where(x => x.FieldType == typeof(Language))
            .Select(x => x.Cast().To<System.Reflection.FieldInfo>().GetValue(me).Cast().To<Language>())
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();

    public override int GetHashCode()
        => base.GetHashCode();

    public override string? ToString()
        => base.ToString();
}