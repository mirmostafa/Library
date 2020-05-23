#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using Library35.WinMgmInstrumentation.Internals;

namespace Library35.WinMgmInstrumentation.Win32
{
	[WmiClass(ClassName = "Win32_OperatingSystem")]
	public class OperatingSystem : WmiBase
	{
		[WmiProp]
		public string Caption { get; protected set; }
	}
}