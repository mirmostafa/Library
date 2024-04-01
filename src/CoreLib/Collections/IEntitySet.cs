using System.Collections;

namespace Library.Collections;

public interface IEntitySet<TEntity> : IReadOnlyList<TEntity>
{
}

public sealed class EntitySet<TEntity>(IEnumerable<TEntity> entities) : IEntitySet<TEntity>
{
    private readonly IEnumerable<TEntity> _entities = entities;

    public int Count => EnumerableHelper.Count(this._entities);
    public TEntity this[int index] => this._entities.ElementAt(index);

    public IEnumerator<TEntity> GetEnumerator() => this._entities.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}