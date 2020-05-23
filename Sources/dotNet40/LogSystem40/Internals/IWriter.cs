#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.ComponentModel;
using Library40.LogSystem.Entities;

namespace Library40.LogSystem.Internals
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