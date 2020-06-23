


using System;

namespace Mohammad.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IsBetweenNumberAttribute : ValidationAttribute
    {
        public long MinValue { get; }
        public long MaxValue { get; }

        public IsBetweenNumberAttribute(long minValue, long maxValue)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IsBetweenDateAttribute : ValidationAttribute
    {
        public DateTime MinValue { get; }
        public DateTime MaxValue { get; }

        public IsBetweenDateAttribute(DateTime minValue, DateTime maxValue)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }
    }
}