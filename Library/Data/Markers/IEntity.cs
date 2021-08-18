namespace Library.Data.Markers;
public interface IHasId<TIdType>
{
    TIdType Id { get; set; }
}
public interface IIdenticalEntity<TIdType> : IHasId<TIdType>, IEntity
{

}

public interface IEntity
{
}