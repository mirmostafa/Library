using Library.CodeGeneration.v2.Back;
using Library.Helpers.CodeGen;
using Library.Results;
using Library.Validations;

namespace Library.CodeGeneration.v2;

public sealed class RoslynCodeGenerator : ICodeGeneratorEngine
{
    public Result<string> Generate(INamespace nameSpace)
    {
        Check.MustBeArgumentNotNull(nameSpace);

        if (!nameSpace.Validate().TryParse(out var vr1))
        {
            return vr1.WithValue(string.Empty);
        }
        var root = RoslynHelper.CreateRoot();
        _ = root.AddNameSpace(nameSpace.Name, out var rosNameSpace);
        foreach (var usingNameSpace in nameSpace.UsingNamespaces)
        {
            root = root.AddUsingNameSpace(usingNameSpace);
        }
        foreach (var typeName in nameSpace.Types)
        {
            if (!typeName.Validate().TryParse(out var vr2))
            {
                return vr2.WithValue(string.Empty);
            }

            _ = rosNameSpace.AddType(typeName.Name);
        }
        return Result<string>.CreateSuccess(root.GenerateCode());
    }
}