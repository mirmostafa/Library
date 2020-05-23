namespace Mohammad.Collections.Generic
{
    public class Tuple<T1, T2>
    {
        public T1 Arg1 { get; set; }
        public T2 Arg2 { get; set; }
        public Tuple() { }

        public Tuple(T1 arg1, T2 arg2)
        {
            this.Arg1 = arg1;
            this.Arg2 = arg2;
        }
    }

    public class Tuple<T1, T2, T3>
    {
        public T1 Arg1 { get; set; }
        public T2 Arg2 { get; set; }
        public T3 Arg3 { get; set; }
        public Tuple() { }

        public Tuple(T1 arg1, T2 arg2, T3 arg3)
        {
            this.Arg1 = arg1;
            this.Arg2 = arg2;
            this.Arg3 = arg3;
        }
    }

    public class Tuple<T1, T2, T3, T4>
    {
        public T1 Arg1 { get; set; }
        public T2 Arg2 { get; set; }
        public T3 Arg3 { get; set; }
        public T4 Arg4 { get; set; }
        public Tuple() { }

        public Tuple(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            this.Arg1 = arg1;
            this.Arg2 = arg2;
            this.Arg3 = arg3;
            this.Arg4 = arg4;
        }
    }
}