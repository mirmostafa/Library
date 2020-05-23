#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IsBetweenNumberAttribute : ValidationAttribute
    {
        public IsBetweenNumberAttribute(long minValue, long maxValue)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }

        public long MinValue { get; }
        public long MaxValue { get; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IsBetweenDateAttribute : ValidationAttribute
    {
        public IsBetweenDateAttribute(DateTime minValue, DateTime maxValue)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }

        public DateTime MinValue { get; }
        public DateTime MaxValue { get; }
    }
}