
using Library.CodeGeneration.v2.Back;

namespace UnitTest;

[TestClass]
public class CodeGenV2Test
{
    [TestMethod]
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