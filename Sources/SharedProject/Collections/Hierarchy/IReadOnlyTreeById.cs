#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

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