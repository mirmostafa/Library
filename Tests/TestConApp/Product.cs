using Library.SourceGenerator.Contracts;

namespace TestConApp;

[GenerateDto]
public class Product
{
    public string Name { get; set; }
}
