using System;
using Mohammad.Internals;

namespace Mohammad.Globalization.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    internal sealed class LocalizedDescriptionAttribute : Attribute
    {
        [NotNull]
        public string CultureName { get; set; }

        public string Description { get; set; }

        public LocalizedDescriptionAttribute(string cultureName)
            : this(cultureName, string.Empty) { }

        public LocalizedDescriptionAttribute(string cultureName, string description)
        {
            if (cultureName == null)
                throw new ArgumentNullException("cultureName");
            this.CultureName = cultureName;
            this.Description = description;
        }
    }
}