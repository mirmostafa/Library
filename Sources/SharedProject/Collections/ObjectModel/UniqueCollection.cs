#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.Collections.ObjectModel
{
    [Serializable]
    public class UniqueCollection<T> : EventualCollection<T>
    {
        protected override void InsertItem(int index, T item)
        {
            if (this.Contains(item))
            {
                return;
            }

            base.InsertItem(index, item);
        }
    }
}