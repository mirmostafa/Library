using System;

namespace Mohammad.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class NotNullAttribute : ValidationAttribute {}
}