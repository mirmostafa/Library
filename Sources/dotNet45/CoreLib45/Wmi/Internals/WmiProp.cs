using System;

namespace Mohammad.Wmi.Internals
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class WmiPropAttribute : Attribute
    {
        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class WmiClassAttribute : Attribute
    {
        public string Namespace { get; set; } = "CIMV2";
        public string ClassName { get; set; }
    }
}