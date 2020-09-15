using System;
using System.Runtime.Serialization;

namespace Mohammad.Data.Common.Exceptions
{
    [Serializable]
    public class NoItemSelectedException : DataValidationException
    {
        public NoItemSelectedException()
        {
        }

        private NoItemSelectedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}