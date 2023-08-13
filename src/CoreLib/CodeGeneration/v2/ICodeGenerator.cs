using Library.CodeGeneration.v2.Back;

namespace Library.CodeGeneration.v2;

public interface ICodeGenerator
{
    string Generate(INamespace @namespace);

    Code Generate(INamespace @namespace, string name, Language language, bool isPartial) =>
        Code.ToCode(name, language, this.Generate(@namespace), isPartial);
}