#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Collections.Generic;
using Mohammad.Collections.Generic;

namespace Mohammad.Collections.ObjectModel
{
    public class TripleValueCollection<T1, T2, T3> : EventualCollection<TripleValue<T1, T2, T3>>
    {
        public TripleValue<T1, T2, T3> Add(T1 value1, T2 value2, T3 value3)
        {
            var result = new TripleValue<T1, T2, T3>(value1, value2, value3);
            this.Add(result);
            return result;
        }
    }

    public class TripleList<T1, T2, T3> : List<TripleValue<T1, T2, T3>>
    {
        public TripleValue<T1, T2, T3> Add(T1 value1, T2 value2, T3 value3)
        {
            var result = new TripleValue<T1, T2, T3>(value1, value2, value3);
            this.Add(result);
            return result;
        }
    }
}