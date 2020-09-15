using System;
using System.Collections.Generic;

namespace Mohammad.Collections.ObjectModel
{
    public class TupleCollection<T1, T2, T3> : EventualCollection<Tuple<T1, T2, T3>>
    {
        public Tuple<T1, T2, T3> Add(T1 value1, T2 value2, T3 value3)
        {
            var result = new Tuple<T1, T2, T3>(value1, value2, value3);
            this.Add(result);
            return result;
        }
    }

    public class TripleList<T1, T2, T3> : List<Tuple<T1, T2, T3>>
    {
        public Tuple<T1, T2, T3> Add(T1 value1, T2 value2, T3 value3)
        {
            var result = new Tuple<T1, T2, T3>(value1, value2, value3);
            this.Add(result);
            return result;
        }
    }
}