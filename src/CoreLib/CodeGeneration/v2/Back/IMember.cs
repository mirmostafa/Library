namespace Library.CodeGeneration.v2.Back;

public interface IMember
{
    AccessModifier AccessModifier { get; }
    InheritanceModifier InheritanceModifier { get; }
    bool IsStatic { get; }
    string Name { get; }

    TypePath Type { get; }
}