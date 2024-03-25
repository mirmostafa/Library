using Library.CodeGeneration.v2.Back;
using Library.Validations;

namespace Library.CodeGeneration.v2;

public static class CodeGeneratorModelsExtensions
{
    public static void AddAttribute<TAttribute>([DisallowNull] this IHasAttributes model, params (string? Name, string Value)[] properties) =>
        model.ArgumentNotNull().Attributes.Add(ICodeGenAttribute.New(TypePath.New<TAttribute>(), properties));

    public static THasAttributes AddAttribute<THasAttributes>([DisallowNull] this THasAttributes model, TypePath attribute, params (string? Name, string Value)[] properties)
        where THasAttributes : IHasAttributes
    {
        model.ArgumentNotNull().Attributes.Add(ICodeGenAttribute.New(TypePath.New(attribute), properties));
        return model;
    }
}