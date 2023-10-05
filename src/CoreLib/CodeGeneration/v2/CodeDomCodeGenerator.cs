using System.Reflection;

using Library.CodeGeneration.v2.Back;
using Library.Helpers.CodeGen;
using Library.Results;

namespace Library.CodeGeneration.v2;

public sealed class CodeDomCodeGenerator : ICodeGenerator
{
    public Result<string> Generate(INamespace nameSpace)
    {
        var buffer = CodeDomHelper.Begin();
        var ns = buffer.AddNewNameSpace(nameSpace.Name).UseNameSpace(nameSpace.UsingNamespaces);
        foreach (var type in nameSpace.Types)
        {
            var newClass = ns.AddNewClass(type.Name, type.BaseTypes.Select(x => x.Name).Compact(), type.IsPartial, this.ToTypeAttributes(type.AccessModifier));
            foreach (var member in type.Members)
            {
                //member switch
                //{
                //    Field field => newClass.AddField(field.Type, )
                //}
            }
        }

        return Result<string>.CreateSuccess("");


    }

    private TypeAttributes ToTypeAttributes(AccessModifier accessModifier) => throw new NotImplementedException();
}