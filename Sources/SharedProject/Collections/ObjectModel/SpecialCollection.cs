using System.Collections.Generic;
using System.Linq;

namespace Mohammad.Collections.ObjectModel
{
    public abstract class SpecialCollection<T>
    {
        protected SpecialCollection() => this.InnerItems = new EventualCollection<T>();
        protected EventualCollection<T> InnerItems { get; }

        protected IEnumerable<T1> Pick<T1>()
        {
            return this.InnerItems.Where(item => item is T1).Cast<T1>();
        }

        protected virtual T AddItem(T item)
        {
            this.InnerItems.Add(item);
            return item;
        }
    }
}