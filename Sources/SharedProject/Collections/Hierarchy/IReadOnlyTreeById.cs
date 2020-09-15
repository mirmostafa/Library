using System.Collections.Generic;

namespace Mohammad.Collections.Hierarchy
{
    public interface IReadOnlyTreeById<T> : IEnumerable<T>
    {
        bool HasChild(NodeById<T> parent);
        IEnumerable<NodeById<T>> GetChildNodesOf(NodeById<T> parent);
        NodeById<T> GetParentNodeOf(NodeById<T> child);
    }
}