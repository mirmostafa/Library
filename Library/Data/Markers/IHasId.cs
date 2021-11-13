using System.ComponentModel.DataAnnotations;

namespace Library.Data.Markers;

public interface IHasId<TIdType>
{
    [Key]
    TIdType Id { get; set; }
}
