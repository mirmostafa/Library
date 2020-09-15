using System;
using System.ComponentModel;

namespace Mohammad.Logging
{
    [Flags]
    public enum LogLevel
    {
        None = 0,

        /// <summary>
        ///     Information
        /// </summary>
        [Description("Information")]
        Info = 2,

        /// <summary>
        ///     Exception
        /// </summary>
        [Description("Exception")]
        Error = 4,

        /// <summary>
        ///     Warning
        /// </summary>
        [Description("Warning")]
        Warning = 8,

        /// <summary>
        ///     Fatal Error
        /// </summary>
        [Description("Fatal Error")]
        Fatal = 16,

        /// <summary>
        ///     An internal log to be used in a program internally.
        /// </summary>
        Internal = 32,

        /// <summary>
        ///     Debug
        /// </summary>
        [Description("Debug")]
        Debug = 64,

        /// <summary>
        ///     Status
        /// </summary>
        [Description("Status")]
        Status = 128,

        /// <summary>
        ///     Normal
        /// </summary>
        [Description("Normal")]
        Normal = Info | Error | Warning | Internal
    }
}