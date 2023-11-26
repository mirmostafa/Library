using Library.CodeGeneration.Models;
using Library.CodeGeneration.v2.Back;
using Library.Helpers.CodeGen;
using Library.Results;

namespace Library.CodeGeneration.v2;

public interface ICodeGeneratorEngine
{
    Result<string> Generate([DisallowNull] INamespace nameSpace);
}

public static class CodeGeneratorExtensions
{
    public static Result<Code> Generate(
        this ICodeGeneratorEngine codeGenerator,
        in INamespace nameSpace,
        [DisallowNull] in string name,
        [DisallowNull] Language language,
        bool isPartial,
        string? fileName = null)
    {
        Checker.MustBeArgumentNotNull(codeGenerator);

        var statement = codeGenerator.Generate(nameSpace);
        var code = new Code(name, language, RoslynHelper.ReformatCode(statement), isPartial, fileName);
        return Result<Code>.From(statement, code);
    }
}