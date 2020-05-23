using System;
using System.Runtime.Serialization;

namespace Mohammad.Data.Exceptions
{
    [Serializable]
    public sealed class MustBeUniqueException : DataValidationException
    {
        public MustBeUniqueException() { }

        private MustBeUniqueException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}