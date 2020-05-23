using System;

namespace Mohammad.Exceptions
{
    [Serializable]
    public class BreakException : Exception
    {
        public static void Throw() { throw new BreakException(); }
    }
}