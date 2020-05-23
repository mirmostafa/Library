#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.Validation.Attributes
{
    public class ValidationAttribute : Attribute
    {
        public string FriendlyName { get; set; }
    }
}