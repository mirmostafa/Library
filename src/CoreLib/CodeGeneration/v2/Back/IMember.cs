namespace Library.CodeGeneration.v2.Back;

public interface IMember : IHasAttribute
{
    string? Comment { get; }

    bool IsAbstract { get; }

    bool IsStatic { get; }

    MemberPrefixes MemberPrefix { get; }

    string Name { get; }
}
