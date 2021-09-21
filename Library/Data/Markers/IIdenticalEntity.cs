namespace Library.Data.Markers;

public interface IIdenticalEntity<TIdType> : IHasId<TIdType>, IEntity
{

}

public interface IIdenticalEntity : IIdenticalEntity<long>
{

}