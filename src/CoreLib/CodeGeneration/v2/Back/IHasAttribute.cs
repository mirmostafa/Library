namespace Library.CodeGeneration.v2.Back;

public interface IHasAttribute
{
    ISet<IAttribute> Attributes { get; }
}
