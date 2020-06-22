using System;

namespace Mohammad.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NotLessThanAttribute : ValidationAttribute
    {
        public long MinValue { get; private set; }
        public NotLessThanAttribute(long minValue) { this.MinValue = minValue; }
    }
}