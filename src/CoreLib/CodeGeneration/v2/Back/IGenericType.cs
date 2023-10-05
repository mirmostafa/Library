namespace Library.CodeGeneration.v2.Back;

public interface IGenericType
{
    string Constraints { get; }
    string Name { get; }
}

public interface IHasGenericTypes
{
    ISet<IGenericType> GenericTypes { get; }
}