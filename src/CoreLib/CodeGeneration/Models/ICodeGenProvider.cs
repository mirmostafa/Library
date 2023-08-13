namespace Library.CodeGeneration.Models;

public interface ICodeGenProvider
{
    Codes GenerateCode(in INameSpace nameSpace, in GenerateCodesParameters? arguments = default);
}