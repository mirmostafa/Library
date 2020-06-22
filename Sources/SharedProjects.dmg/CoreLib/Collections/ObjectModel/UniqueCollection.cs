using System;

namespace Mohammad.Collections.ObjectModel
{
    [Serializable]
    public class UniqueCollection<T> : EventualCollection<T>
    {
        protected override void InsertItem(int index, T item)
        {
            if (this.Contains(item))
                return;
            base.InsertItem(index, item);
        }
    }
}