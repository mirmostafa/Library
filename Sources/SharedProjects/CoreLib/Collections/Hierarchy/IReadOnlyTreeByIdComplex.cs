using System.Collections.Generic;

namespace Mohammad.Collections.Hierarchy
{
    public interface IReadOnlyTreeByIdComplex<TItem, TParent>
    {
        IEnumerable<NodeByIdComplex<TItem, TParent>> GetChildNodesOf(long parentId);
        NodeByIdComplex<TItem, TParent> GetParentNodeOf(long childId);
    }
}