namespace Library.CodeGeneration.v2.Back;

public interface IField : IMember
{
    bool IsReadOnly { get; }

    static IField New(string name, TypePath typeName, bool isReadOnly, bool isStatic, AccessModifier accessModifier, InheritanceModifier inheritanceModifier) =>
        new Field(name, typeName)
        {
            IsReadOnly = isReadOnly,
            IsStatic = isStatic,
            AccessModifier = accessModifier,
            InheritanceModifier = inheritanceModifier
        };
}

internal sealed class Field(string name, TypePath type) : IField
{
    public AccessModifier AccessModifier { get; init; }
    public InheritanceModifier InheritanceModifier { get; init; }
    public bool IsReadOnly { get; init; }
    public bool IsStatic { get; init; }
    public string Name { get; init; } = name;
    public TypePath Type { get; } = type;
}