#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.InteropServices;
using Library40.Win32.Natives.IfacesEnumsStructsClasses;

namespace Library40.Win32.Natives
{
	public partial class Api
	{
		[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern bool CreateProcessAsUser(IntPtr hToken,
			string lpApplicationName,
			string lpCommandLine,
			ref SECURITY_ATTRIBUTES lpProcessAttributes,
			ref SECURITY_ATTRIBUTES lpThreadAttributes,
			bool bInheritHandles,
			uint dwCreationFlags,
			IntPtr lpEnvironment,
			int lpCurrentDirectory,
			ref STARTUPINFO lpStartupInfo,
			out PROCESS_INFORMATION lpProcessInformation);

		[DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern bool CloseHandle(IntPtr handle);

		[DllImport("advapi32.dll", EntryPoint = "CreateProcessAsUser", SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern bool CreateProcessAsUser(IntPtr hToken,
			string lpApplicationName,
			string lpCommandLine,
			ref SECURITY_ATTRIBUTES lpProcessAttributes,
			ref SECURITY_ATTRIBUTES lpThreadAttributes,
			bool bInheritHandle,
			Int32 dwCreationFlags,
			IntPtr lpEnvrionment,
			string lpCurrentDirectory,
			ref STARTUPINFO lpStartupInfo,
			ref PROCESS_INFORMATION lpProcessInformation);

		[DllImport("advapi32.dll", EntryPoint = "DuplicateTokenEx")]
		public static extern bool DuplicateTokenEx(IntPtr hExistingToken,
			Int32 dwDesiredAccess,
			ref SECURITY_ATTRIBUTES lpThreadAttributes,
			Int32 ImpersonationLevel,
			Int32 dwTokenType,
			ref IntPtr phNewToken);
	}
}