#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Collections;
using System.Collections.Generic;

namespace Mohammad.Collections.Generic
{
    internal sealed class Enumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _Items;
        public Enumerable(IEnumerable<T> items) => this._Items = items;
        public IEnumerator<T>   GetEnumerator() => this._Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}