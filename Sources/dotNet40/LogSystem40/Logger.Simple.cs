#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using Library40.LogSystem.Entities;
using Library40.LogSystem.Internals;

namespace Library40.LogSystem
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