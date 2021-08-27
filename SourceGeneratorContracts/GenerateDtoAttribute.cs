namespace Library.SourceGenerator.Contracts;
[AttributeUsage(AttributeTargets.Class)]
public sealed class GenerateDtoAttribute : Attribute
{
    public string? DtoClassName { get; set; }
}
