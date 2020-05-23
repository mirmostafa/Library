#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.ComponentModel;

namespace Library35.LogSystem
{
	namespace Entities
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		public enum LogLevel
		{
			/// <summary>
			///     Information
			/// </summary>
			[Description("Information")]
			Info,
			/// <summary>
			///     Warning
			/// </summary>
			[Description("Warning")]
			Warn,
			/// <summary>
			///     Error
			/// </summary>
			[Description("Error")]
			Error,
			/// <summary>
			///     Fatal Error
			/// </summary>
			[Description("Fatal Error")]
			Fatal,
			/// <summary>
			///     An internal log to be used in a program internally.
			/// </summary>
			Internal,
			/// <summary>
			///     Debug
			/// </summary>
			[Description("Debug")]
			Debug
		}
	}

	namespace EventsArgs
	{
		//public abstract class LogEventArgs<TLogEntity> : EventArgs
		//    where TLogEntity : LogEntity
		//{
		//    internal LogEventArgs(TLogEntity logEntity)
		//    {
		//        this.LogEntity = logEntity;
		//    }

		//    public TLogEntity LogEntity { get; private set; }
		//}

		//public class LoggingEventArgs<TLogEntity> : LogEventArgs<TLogEntity>
		//    where TLogEntity : LogEntity
		//{
		//    internal LoggingEventArgs(TLogEntity logEntity)
		//        : base(logEntity)
		//    {
		//    }

		//    public bool Handled { get; set; }
		//}

		//public class LoggedEventArgs<TLogEntity> : LogEventArgs<TLogEntity>
		//    where TLogEntity : LogEntity
		//{
		//    internal LoggedEventArgs(TLogEntity logEntity)
		//        : base(logEntity)
		//    {
		//    }
		//}
	}

	namespace Configurations
	{
		public enum LoggingSeverity
		{
			High,
			Normal,
			Never
		}
	}
}