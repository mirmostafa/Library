namespace Library.CodeGeneration.v2.Back;

public interface IAttribute
{
    TypePath Name { get; }
    (string Name, string Vallue) Properties { get; }
}