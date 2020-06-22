using System;

namespace Mohammad.Collections.Generic
{
    [Serializable]
    public class TripleValue<T1, T2, T3>
    {
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
        public T3 Value3 { get; set; }
        public TripleValue() { }

        public TripleValue(T1 value1, T2 value2, T3 value3)
        {
            this.Value1 = value1;
            this.Value2 = value2;
            this.Value3 = value3;
        }
    }
}