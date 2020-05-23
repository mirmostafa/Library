#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NotLessThanAttribute : ValidationAttribute
    {
        public long MinValue { get; }
        public NotLessThanAttribute(long minValue) => this.MinValue = minValue;
    }
}