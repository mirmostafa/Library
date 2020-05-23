#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Library40.Win.Controls
{
	partial class DialogBoxController
	{
		private const long AW_BLEND = 0x00080000;
		private const long AW_CENTER = 0x00000010;
		private const long AW_HIDE = 0x00010000;
		private const int GWL_EXSTYLE = (-20);
		private const int GWL_HINSTANCE = (-6);
		private const int HCBT_ACTIVATE = 5;
		private const int WH_CBT = 5;
		private const long WM_SETFONT = 0x0030;
		private const long WS_EX_RIGHT = 0x00001000L;
		private const long WS_EX_RTLREADING = 0x00002000L;
		private int hHook;

		[DllImport("user32.dll")]
		private static extern int GetWindowLong(int hwnd, int nIndex);

		[DllImport("user32.dll")]
		private static extern int SetWindowLong(int hwnd, int nIndex, int dwNewLong);

		[DllImport("kernel32.dll")]
		private static extern int GetCurrentThreadId();

		[DllImport("user32.dll")]
		private static extern int SetWindowsHookEx(int idHook, CallBack_WinProc lpfn, int hmod, int dwThreadId);

		[DllImport("user32.dll")]
		private static extern int UnhookWindowsHookEx(int hHook);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern int SetWindowText(int hwnd, string lpString);

		[DllImport("user32.dll")]
		private static extern int EnumChildWindows(int hWndParent, CallBack_EnumWinProc lpEnumFunc, int lParam);

		[DllImport("user32.dll")]
		private static extern int GetClassName(int hwnd, StringBuilder lpClassName, int nMaxCount);

		[DllImport("user32.dll")]
		private static extern int GetWindowText(int hwnd, StringBuilder lpClassName, int nMaxCount);

		[DllImport("user32.dll")]
		private static extern bool AnimateWindow(int hwnd, int tim, int flags);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SendMessage(int hwnd, int message, IntPtr wParam, int lParam);

		#region Nested type: CallBack_EnumWinProc
		private delegate int CallBack_EnumWinProc(int hWnd, int lParam);
		#endregion

		#region Nested type: CallBack_WinProc
		private delegate int CallBack_WinProc(int uMsg, int wParam, int lParam);
		#endregion
	}
}