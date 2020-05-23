#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Collections.Generic;

namespace Mohammad.Collections.Hierarchy
{
    public interface IReadOnlyTreeByIdComplex<TItem, TParent>
    {
        IEnumerable<NodeByIdComplex<TItem, TParent>> GetChildNodesOf(long parentId);
        NodeByIdComplex<TItem, TParent> GetParentNodeOf(long childId);
    }
}