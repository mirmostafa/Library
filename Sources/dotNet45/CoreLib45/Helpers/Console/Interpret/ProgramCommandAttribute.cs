#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.Helpers.Console.Interpret
{
    public sealed class ProgramCommandAttribute : Attribute
    {
        public ProgramCommandAttribute(char shortKey) => this.ShortKey = shortKey;
        public char ShortKey { get; set; }
    }
}