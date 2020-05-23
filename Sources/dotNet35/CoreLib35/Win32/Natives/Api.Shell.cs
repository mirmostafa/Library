#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.InteropServices;
using Library35.Helpers;
using Library35.Win32.Natives.IfacesEnumsStructsClasses;

namespace Library35.Win32.Natives
{
	partial class Api
	{
		public static IntPtr ShellExecute(IntPtr hwnd, ShellExecuteVerbs lpOperation, string lpFile, string lpParameters, string lpDirectory, ShowCommands nShowCmd)
		{
			string verb;
			switch (lpOperation)
			{
				case ShellExecuteVerbs.OpenFile:
					verb = "open";
					break;
				case ShellExecuteVerbs.EditFile:
					verb = "edit";
					break;
				case ShellExecuteVerbs.ExploreFolder:
					verb = "explore";
					break;
				case ShellExecuteVerbs.FindInFolder:
					verb = "find";
					break;
				case ShellExecuteVerbs.PrintFile:
					verb = "print";
					break;
				default:
					throw new ArgumentOutOfRangeException("lpOperation");
			}
			return ShellExecute(hwnd, verb, lpFile, lpParameters, lpDirectory, nShowCmd.ToInt());
		}

		[DllImport("shell32.dll")]
		public static extern IntPtr ShellExecute(IntPtr hwnd,
			[MarshalAs(UnmanagedType.LPTStr)] String lpOperation,
			[MarshalAs(UnmanagedType.LPTStr)] String lpFile,
			[MarshalAs(UnmanagedType.LPTStr)] String lpParameters,
			[MarshalAs(UnmanagedType.LPTStr)] String lpDirectory,
			Int32 nShowCmd);

		[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
		public static extern Int32 SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);
	}
}