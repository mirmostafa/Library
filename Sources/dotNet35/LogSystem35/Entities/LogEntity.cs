#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Diagnostics;
using System.Reflection;
using Library35.Globalization;
using Library35.Helpers;
using Library35.LogSystem.Attributes;
using Library35.LogSystem.Internals;

namespace Library35.LogSystem.Entities
{
	public class LogEntity
	{
		private string _EndDelimiter = ", ";
		private StackTrace _StackTrace;

		public LogEntity()
		{
			this.Initialize();
		}

		public Object Text { get; set; }

		public Exception Exception { get; set; }

		public string Descriptor { get; set; }

		public PersianDateTime Time { get; set; }

		public StackTrace StackTrace
		{
			get { return this.Level != LogLevel.Debug && this.Level != LogLevel.Fatal ? null : this._StackTrace; }
			set { this._StackTrace = value; }
		}

		public LogLevel Level { get; set; }

		public Object Tag { get; set; }
		public string StartDelimiter { get; set; }
		public string EndDelimiter
		{
			get { return this._EndDelimiter; }
			set { this._EndDelimiter = value; }
		}

		private static string GetDescriptor()
		{
			var caller = MethodHelper.GetCallerMethod(Utilities.SkipFrameCount() - 1, true);
			if (caller != null)
			{
				if (caller.Name.Contains("<"))
					caller = caller.ReflectedType.GetMethod(caller.Name.GetPhrase('<', '>', 1), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

				var callerDesc = MethodHelper.GetAttribute(caller, new LogDescriptionAttribute(caller.Name.SeparateCamelCase())).Description ?? caller.Name.SeparateCamelCase();
				var classDesc = caller.GetAttribute(new LogDescriptionAttribute(caller.GetClassName().SeparateCamelCase())).Description ?? caller.GetClassName().SeparateCamelCase();
				return string.Format("{0}-{1}", classDesc, callerDesc);
			}
			return "Lambda Expression!";
		}

		protected virtual void Initialize()
		{
			var caller = MethodHelper.GetCallerMethod(Utilities.SkipFrameCount(), true);
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

		private static StackTrace GetStackTrace()
		{
			return new StackTrace(Utilities.SkipFrameCount() - 2);
		}

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
			var result = String.CompareOrdinal((this.Text ?? string.Empty).ToString(), "-") == 0
				? Environment.NewLine
				: string.Format("{0}{2}{1}{0}{3}{1}{0}{4}{1}{0}{5}{1}{0}{6}{1}{0}{7}{1}",
					this.StartDelimiter,
					this.EndDelimiter,
					this.Time,
					this.Level,
					this.Descriptor,
					this.Text,
					this.Exception != null ? string.Concat(Environment.NewLine, "\t", this.Exception.Message) : "",
					this.StackTrace != null ? string.Concat(Environment.NewLine, this.StackTrace) : "");
			return result;
		}
	}
}