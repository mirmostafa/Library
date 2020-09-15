using System;

namespace Mohammad.Helpers.Console.Interpret
{
    public sealed class ProgramCommandAttribute : Attribute
    {
        public ProgramCommandAttribute(char shortKey) => this.ShortKey = shortKey;
        public char ShortKey { get; set; }
    }
}