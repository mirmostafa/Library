using System;

namespace Mohammad.ServiceModel
{
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class ServiceConfigAttribute : Attribute
    {
        public string ServiceUrl { get; set; }
    }
}