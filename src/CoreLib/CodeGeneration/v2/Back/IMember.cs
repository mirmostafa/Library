namespace Library.CodeGeneration.v2.Back;

public interface IMember
{
    string? Comment { get; }

    bool IsStatic { get; }

    AccessModifier MemberPrefix { get; }

    string Name { get; }
}