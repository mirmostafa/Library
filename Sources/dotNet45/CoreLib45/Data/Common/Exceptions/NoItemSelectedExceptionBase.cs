#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

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