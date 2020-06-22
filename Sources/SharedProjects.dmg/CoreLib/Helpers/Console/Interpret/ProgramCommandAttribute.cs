using System;

namespace Mohammad.Helpers.Console.Interpret
{
    public sealed class ProgramCommandAttribute : Attribute
    {
        public char ShortKey { get; set; }
        public ProgramCommandAttribute(char shortKey) { this.ShortKey = shortKey; }
    }
}