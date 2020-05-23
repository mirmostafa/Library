#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.InteropServices;

namespace Library35.Windows.Forms.Internals
{
	internal sealed class MsgBoxExNative
	{
		#region Fields

		#region GENERIC_ALL
		public const int GENERIC_ALL = 0x10000000;
		#endregion

		#region DESKTOP_SWITCHDESKTOP
		public const int DESKTOP_SWITCHDESKTOP = 0x100;
		#endregion

		#endregion

		#region Methods

		#region SwitchDesktop
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SwitchDesktop(IntPtr desktop);
		#endregion

		#region SetThreadDesktop
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetThreadDesktop(IntPtr desktop);
		#endregion

		#region OpenInputDesktop
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr OpenInputDesktop(int flags, [MarshalAs(UnmanagedType.Bool)] bool inherit, int desiredAccess);
		#endregion

		#region GetThreadDesktop
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetThreadDesktop(int threadId);
		#endregion

		#region CreateDesktop
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CreateDesktop([MarshalAs(UnmanagedType.VBByRefStr)] ref string desktop,
			[MarshalAs(UnmanagedType.VBByRefStr)] ref string device,
			IntPtr devmode,
			int flags,
			int desiredAccess,
			IntPtr lpsa);
		#endregion

		#region CloseDesktop
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool CloseDesktop(IntPtr desktop);
		#endregion

		#endregion
	}
}