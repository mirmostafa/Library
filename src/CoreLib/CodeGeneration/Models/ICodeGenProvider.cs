namespace Library.CodeGeneration.Models;

public interface ICodeGenProvider
{
    Codes GenerateBehindCode(in INameSpace nameSpace, in GenerateCodesParameters? arguments = default);
}