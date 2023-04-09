using Library.CodeGeneration.v2.Back;

namespace UnitTests;


[Trait("Category", "Code Generators")]
public sealed class CodeGenV2Test
{
    [Fact]
    public void CodeGenV2Basic()
    {
        var datamodels = INamespace
            .New("Data.Models")
            .AddType(IClass.New("PersonDto")
                           .AddProperty("Name", typeof(string))
                           .AddProperty("Age", typeof(int)));
        //CodeDomCodeGenProvider.GenerateBehindCode(datamodels);
        //var code = ICodeGenerator.
    }
}