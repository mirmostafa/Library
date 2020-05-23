#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.InteropServices;
using Library35.Win32.Natives.IfacesEnumsStructsClasses;

namespace Library35.Win32.Natives
{
	public partial class Api
	{
		[DllImport("advapi32.dll", EntryPoint = "InitiateSystemShutdown")]
		public static extern int InitiateSystemShutdownA(string lpMachineName, string lpMessage, int dwTimeout, int bForceAppsClosed, int bRebootAfterShutdown);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern int ExitWindowsEx(uint uFlags, uint dwReason);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetCurrentProcess();

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern int OpenProcessToken(IntPtr processHandle, int desiredAccess, ref IntPtr tokenHandle);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern int LookupPrivilegeValue(string systemName, string name, ref long luid);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern int AdjustTokenPrivileges(IntPtr tokenHandle,
			bool disableAllPrivileges,
			ref TokenPrivileges newState,
			int bufferLength,
			IntPtr previousState,
			IntPtr length);
	}
}