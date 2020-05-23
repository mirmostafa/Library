using System;

namespace Mohammad.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IsBetweenAttribute : ValidationAttribute
    {
        public long MinValue { get; private set; }
        public long MaxValue { get; private set; }

        public IsBetweenAttribute(long minValue, long maxValue)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }
    }
}