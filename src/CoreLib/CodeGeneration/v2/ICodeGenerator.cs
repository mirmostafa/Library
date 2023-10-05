using Library.CodeGeneration.v2.Back;
using Library.Results;

namespace Library.CodeGeneration.v2;

public interface ICodeGenerator
{
    Result<string> Generate(INamespace @namespace);
}