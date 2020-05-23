#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using Library35.LogSystem.Entities;
using Library35.LogSystem.Internals;

namespace Library35.LogSystem
{
	/// <summary>
	///     Facade for writing a log entry.
	/// </summary>
	public class Logger<TWriter> : Logger<TWriter, LogEntity>
		where TWriter : class, IWriter<LogEntity>
	{
		public Logger(TWriter writer, bool raiseEventOnly)
			: base(writer, raiseEventOnly)
		{
		}

		public Logger(TWriter writer)
			: base(writer)
		{
		}

		protected Logger()
		{
		}
	}
}