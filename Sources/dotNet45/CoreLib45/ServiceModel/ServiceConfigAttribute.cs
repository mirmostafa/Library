#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.ServiceModel
{
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class ServiceConfigAttribute : Attribute
    {
        public string ServiceUrl { get; set; }
    }
}