#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.ComponentModel;
using Library35.LogSystem.Entities;

namespace Library35.LogSystem.Internals
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IWriter<in TLogEntity>
		where TLogEntity : LogEntity
	{
		bool ShowInDebuggerTracer { get; set; }

		void Write(TLogEntity logEntity);

		void LoadLastLog();

		//TextWriter Out
		//{
		//    get;
		//}
	}
}