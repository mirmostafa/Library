using System;

namespace Mohammad.Collections.Generic
{
    [Serializable]
    public class PairValue<T1, T2>
    {
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
        public PairValue() { }

        public PairValue(T1 value1, T2 value2)
        {
            this.Value1 = value1;
            this.Value2 = value2;
        }
    }
}