using System;
using System.Diagnostics;
using System.Reflection;
using Mohammad.Globalization;
using Mohammad.Helpers;
using Mohammad.Logging.Attributes;
using Mohammad.Logging.Internals;

namespace Mohammad.Logging.Entities
{
    public class LogEntity
    {
        private StackTrace _StackTrace;
        public object Text { get; set; }
        public object MoreInfo { get; set; }
        public Exception Exception { get; set; }
        public string Descriptor { get; set; }
        public PersianDateTime Time { get; set; }

        public StackTrace StackTrace
        {
            get { return this.Level != LogLevel.Debug && this.Level != LogLevel.Fatal ? null : this._StackTrace; }
            set { this._StackTrace = value; }
        }

        public LogLevel Level { get; set; }
        public object Tag { get; set; }
        public string StartDelimiter { get; set; }
        public string EndDelimiter { get; set; } = ", ";
        public LogEntity() { this.Initialize(); }

        private static string GetDescriptor()
        {
            try
            {
                var caller = CodeHelper.GetCallerMethod(Utilities.SkipFrameCount() - 1, true);
                if (caller == null)
                    return "Lambda Expression!";
                if (caller.Name.Contains("<"))
                    if (caller.ReflectedType != null)
                        caller = caller.ReflectedType.GetMethod(caller.Name.GetPhrase(1,'<', '>'),
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

                var callerDesc = ObjectHelper.GetAttribute(caller, new LogDescriptionAttribute(caller.Name.SeparateCamelCase())).Description ??
                                 caller.Name.SeparateCamelCase();
                var classDesc = caller.GetDeclaringTypeAttribute(new LogDescriptionAttribute(caller.GetClassName().SeparateCamelCase())).Description ??
                                caller.GetClassName().SeparateCamelCase();
                return $"{classDesc}-{callerDesc}";
            }
            catch
            {
                return string.Empty;
            }
        }

        protected virtual void Initialize()
        {
            var caller = CodeHelper.GetCallerMethod(Utilities.SkipFrameCount(), true);
            if (caller != null)
                switch (ObjectHelper.GetAttribute(caller, new LogDescriptionAttribute(caller.GetClassName().SeparateCamelCase())).AutoFillTag)
                {
                    case AutoFillTag.None:
                        break;
                    case AutoFillTag.SenderType:
                        this.Tag = caller.DeclaringType;
                        break;
                }
            this.Descriptor = GetDescriptor();
            this.Level = LogLevel.Info;
            this.Time = PersianDateTime.Now;
            this.StackTrace = GetStackTrace();
        }

        private static StackTrace GetStackTrace() { return new StackTrace(Utilities.SkipFrameCount() - 2); }

        public override string ToString()
        {
            //return String.CompareOrdinal((this.Text ?? string.Empty).ToString(), "-") == 0
            //        ? "-----------------------------------------------------------------"
            //        : string.Format("[{0, -19}] [{1, -8}] [{2, -30}] {3}{4}{5}",
            //            this.Time,
            //            this.Level,
            //            this.Descriptor,
            //            this.Text,
            //            this.Exception != null ? string.Concat(Environment.NewLine, "\t", this.Exception.Message) : "",
            //            this.StackTrace != null ? string.Concat(Environment.NewLine, this.StackTrace) : "");
            var result = string.CompareOrdinal((this.Text ?? string.Empty).ToString(), "-") == 0
                ? Environment.NewLine
                : string.Format("{0}{2}{1}{0}{3}{1}{0}{4}{1}{0}{5}{1}{0}{6}{1}{0}{7}{1}{8}",
                    this.StartDelimiter,
                    this.EndDelimiter,
                    this.Time,
                    this.Level,
                    this.Descriptor,
                    this.Text,
                    this.Exception != null ? string.Concat(Environment.NewLine, "\t", this.Exception.Message) : "",
                    this.StackTrace != null ? string.Concat(Environment.NewLine, this.StackTrace) : "",
                    Environment.NewLine);
            return result;
        }
    }
}