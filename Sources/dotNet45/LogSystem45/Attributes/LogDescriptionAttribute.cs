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
        public string Description { get; private set; }
        public AutoFillTag AutoFillTag { get; set; }
        public LogDescriptionAttribute() { }
        public LogDescriptionAttribute(string description) { this.Description = description; }
    }
}