using Library.SourceGenerator.Contracts;

namespace TestConApp;

[GenerateDto]
public class Product
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public partial class ExampleViewModel
{
    [AutoNotify]
    private string _Text = "private field text";

    [AutoNotify(PropertyName = "Count")]
    private int _Amount = 5;
}