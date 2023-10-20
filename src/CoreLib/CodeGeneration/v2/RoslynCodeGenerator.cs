using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Library.CodeGeneration.v2.Back;
using Library.Helpers.CodeGen;
using Library.Results;
using Library.Validations;

using Microsoft.CodeAnalysis.CSharp.Syntax;

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
        root.AddNameSpace(nameSpace.Name, out var rosNameSpace);
        rosNameSpace.AddUsingNameSpace(nameSpace.UsingNamespaces);
        return null;
    }
}
