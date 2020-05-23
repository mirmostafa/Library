#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

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