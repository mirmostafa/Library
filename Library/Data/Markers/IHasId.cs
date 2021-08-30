namespace Library.Data.Markers;

public interface IHasId<TIdType>
{
    TIdType Id { get; set; }
}
