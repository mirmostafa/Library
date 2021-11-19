using System.Collections;

namespace Library.Data.SqlServer.Dynamics.Collections;

public abstract class SqlObjects<TSqlObject> : IEnumerable<TSqlObject>
    where TSqlObject : class, ISqlObject
{
    private readonly Lazy<IEnumerable<TSqlObject>> _items;

    protected SqlObjects(IEnumerable<TSqlObject> items) => this._items = new Lazy<IEnumerable<TSqlObject>>(() => items);

    protected SqlObjects(Func<IEnumerable<TSqlObject>> itemsCreator) => this._items = new Lazy<IEnumerable<TSqlObject>>(itemsCreator);

    public virtual TSqlObject this[int index] => this.Items.ElementAt(index);
    public TSqlObject? this[string name] => this.Items.FirstOrDefault(item => item.Name.EqualsTo(name));
    protected IEnumerable<TSqlObject> Items => this._items.Value;
    public IEnumerator<TSqlObject> GetEnumerator() => this.Items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
