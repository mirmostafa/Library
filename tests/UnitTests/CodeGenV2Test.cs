using Library.CodeGeneration.v2.Back;

namespace UnitTests;


public class CodeGenV2Test
{
    [Fact]
    public void Test1()
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