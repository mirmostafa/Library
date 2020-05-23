using System;

namespace Mohammad.Collections.Generic
{
    public class HierarchicalItem<TData>
    {
        public HierarchicalItem<TData> Parent { get; set; }
        public TData Data { get; }

        public HierarchicalItem(TData data, HierarchicalItem<TData> parent = null)
        {
            if (Equals(default(TData), data))
                throw new ArgumentNullException(nameof(data));
            this.Parent = parent;
            this.Data = data;
        }

        public static implicit operator TData(HierarchicalItem<TData> item) => item.Data;
        public static implicit operator HierarchicalItem<TData>(TData data) => Equals(default(TData), data) ? null : new HierarchicalItem<TData>(data);
        public override string ToString() => this.Data.ToString();
    }
}