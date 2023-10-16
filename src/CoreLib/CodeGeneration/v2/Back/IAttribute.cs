namespace Library.CodeGeneration.v2.Back;

public interface IAttribute
{
    string Name { get; }

    static IAttribute New(string name) =>
        new ClassAttribute(name);
}

internal class ClassAttribute(string name) : IAttribute
{
    public string Name { get; } = name;
}