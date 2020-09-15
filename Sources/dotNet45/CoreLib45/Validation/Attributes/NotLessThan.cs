using System;

namespace Mohammad.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NotLessThanAttribute : ValidationAttribute
    {
        public NotLessThanAttribute(long minValue) => this.MinValue = minValue;
        public long MinValue { get; }
    }
}