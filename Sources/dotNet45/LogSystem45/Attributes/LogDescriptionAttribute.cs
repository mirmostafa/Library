using System;
using System.ComponentModel;

namespace Mohammad.Logging.Attributes
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public enum AutoFillTag
    {
        None,
        SenderType
    }

    public class LogDescriptionAttribute : Attribute
    {
        public LogDescriptionAttribute()
        {
        }

        public LogDescriptionAttribute(string description) => this.Description = description;
        public string Description { get; }
        public AutoFillTag AutoFillTag { get; set; }
    }
}