using System.ComponentModel.DataAnnotations;

using Library.CodeGeneration.v2;
using Library.CodeGeneration.v2.Back;

using Xunit.Abstractions;

namespace UnitTests;

[Trait("Category", nameof(Library.CodeGeneration))]
[Trait("Category", nameof(RoslynCodeGenerator))]
public sealed class SourceGeneratorTest(ITestOutputHelper output)
{
    [Fact]
    public void PropertyWithAttribute()
    {
        var prop = new CodeGenProperty("FirstName", typeof(string));
        prop.Attributes.Add(ICodeGenAttribute.New(typeof(RequiredAttribute)));

        var codegen = new RoslynCodeGenerator();
        var code = codegen.Generate(prop);
        output.WriteLine(code);
    }
}