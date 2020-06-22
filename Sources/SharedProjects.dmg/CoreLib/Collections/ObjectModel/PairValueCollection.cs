using System.Collections.Generic;
using Mohammad.Collections.Generic;

namespace Mohammad.Collections.ObjectModel
{
    public class PairValueCollection<T1, T2> : EventualCollection<PairValue<T1, T2>>
    {
        public PairValue<T1, T2> Add(T1 value1, T2 value2)
        {
            var result = new PairValue<T1, T2>(value1, value2);
            this.Add(result);
            return result;
        }
    }

    public class PairList<T1, T2> : List<PairValue<T1, T2>>
    {
        public T2 this[T1 value1]
        {
            get { return this.Find(item => Equals(item.Value1, value1)).Value2; }
            set { this.Find(item => Equals(item.Value1, value1)).Value2 = value; }
        }

        public PairValue<T1, T2> Add(T1 value1, T2 value2)
        {
            var result = new PairValue<T1, T2>(value1, value2);
            this.Add(result);
            return result;
        }
    }
}