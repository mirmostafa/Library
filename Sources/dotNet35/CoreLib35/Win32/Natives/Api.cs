#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Library35.Helpers;
using Library35.Win32.Natives.IfacesEnumsStructsClasses;
//using BYTE = UChar;

namespace Library35.Win32.Natives
{
	public partial class Api
	{
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern Boolean AnimateWindow(IntPtr hwnd, Int64 dwTime, Int64 dwFlags);

		/// <summary>
		///     Enables you to produce special effects when showing or hiding windows. There are four types of animation: roll,
		///     slide, collapse or expand, and alpha-blended fade.
		/// </summary>
		/// <param name="hwnd"> Handle to the window to animate. The calling thread must own this window. </param>
		/// <param name="dwTime">
		///     Specifies how long it takes to play the animation, in milliseconds. Typically, an animation takes
		///     200 milliseconds to play.
		/// </param>
		/// <param name="dwFlags">
		///     Specifies the type of animation. This parameter can be one or more of the following values. Note
		///     that, by default, these flags take effect when showing a window. To take effect when hiding a window, use AW_HIDE
		///     and a logical OR operator with the appropriate flags.
		/// </param>
		/// <returns> </returns>
		public static Boolean AnimateWindow(IntPtr hwnd, Int64 dwTime, WindowAnimateFlags dwFlags)
		{
			return AnimateWindow(hwnd, dwTime, dwFlags.ToLong());
		}

		public static int GetLastError()
		{
			return Marshal.GetLastWin32Error();
		}

		#region Common Defintions
		#endregion

		#region General Definitions
		/// <summary>
		///     Sends the specified message to a window or windows. The SendMessage function calls
		///     the window procedure for the specified window and does not return until the window
		///     procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">
		///     Handle to the window whose window procedure will receive the message. If this parameter is
		///     HWND_BROADCAST, the message is sent to all top-level windows in the system, including disabled or invisible unowned
		///     windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.
		/// </param>
		/// <param name="msg"> Specifies the message to be sent. </param>
		/// <param name="wParam"> Specifies additional message-specific information. </param>
		/// <param name="lParam"> Specifies additional message-specific information. </param>
		/// <returns> A return code specific to the message being sent. </returns>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "w"),
		 SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "l"),
		 SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "h"),
		 SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wnd"),
		 SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Param"),
		 SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification = "This is used from another assembly, also it's in an internal namespace")]
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		///     Sends the specified message to a window or windows. The SendMessage function calls
		///     the window procedure for the specified window and does not return until the window
		///     procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">
		///     Handle to the window whose window procedure will receive the message. If this parameter is
		///     HWND_BROADCAST, the message is sent to all top-level windows in the system, including disabled or invisible unowned
		///     windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.
		/// </param>
		/// <param name="msg"> Specifies the message to be sent. </param>
		/// <param name="wParam"> Specifies additional message-specific information. </param>
		/// <param name="lParam"> Specifies additional message-specific information. </param>
		/// <returns> A return code specific to the message being sent. </returns>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "w"),
		 SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "l"),
		 SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "h"),
		 SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wnd"),
		 SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Param"),
		 SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

		/// <summary>
		///     Sends the specified message to a window or windows. The SendMessage function calls
		///     the window procedure for the specified window and does not return until the window
		///     procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">
		///     Handle to the window whose window procedure will receive the message. If this parameter is
		///     HWND_BROADCAST, the message is sent to all top-level windows in the system, including disabled or invisible unowned
		///     windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.
		/// </param>
		/// <param name="msg"> Specifies the message to be sent. </param>
		/// <param name="wParam"> Specifies additional message-specific information. </param>
		/// <param name="lParam"> Specifies additional message-specific information. </param>
		/// <returns> A return code specific to the message being sent. </returns>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Param"),
		 SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wnd"),
		 SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "h"),
		 SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "l"),
		 SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "w"),
		 SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"),
		 SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "2#", Justification = "This is an in/out parameter"),
		 DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, ref int wParam, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);

		// Various helpers for forcing binding to proper 
		// version of Comctl32 (v6).
		[DllImport("kernel32.dll", SetLastError = true, ThrowOnUnmappableChar = true, BestFitMapping = false)]
		public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteObject(IntPtr graphicsObjectHandle);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern int LoadString(IntPtr hInstance, int uID, StringBuilder buffer, int nBufferMax);

		/// <summary>
		///     Destroys an icon and frees any memory the icon occupied.
		/// </summary>
		/// <param name="hIcon"> Handle to the icon to be destroyed. The icon must not be in use. </param>
		/// <returns>
		///     If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To
		///     get extended error information, call GetLastError.
		/// </returns>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "h"),
		 SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible", Justification = "This is used from other assemblies, also it's in an internal namespace"),
		 DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DestroyIcon(IntPtr hIcon);
		#endregion

		#region Window Handling

		#region Delegates
		public delegate bool EnumDesktopWindowsDelegate(IntPtr hWnd, int lParam);

		/// <summary>
		///     The EnumChildProc function is an application-defined callback function used with the EnumChildWindows function. It
		///     receives the child window handles. The WNDENUMPROC type defines a pointer to this callback function. EnumChildProc
		///     is a placeholder for the application-defined function name.
		/// </summary>
		/// <param name="hWnd"> [in] Handle to a child window of the parent window specified in EnumChildWindows. </param>
		/// <param name="lParam"> [in] Specifies the application-defined value given in EnumChildWindows. </param>
		/// <returns> To continue enumeration, the callback function must return TRUE; to stop enumeration, it must return FALSE. </returns>
		public delegate bool WNDENUMPROC(IntPtr hWnd, IntPtr lParam);
		#endregion

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SoundSentry();

		/// <summary>
		///     The AdjustWindowRect function calculates the required size of the window rectangle, based on the desired
		///     client-rectangle size. The window rectangle can then be passed to the CreateWindow function to create a window
		///     whose client area is the desired size.
		///     To specify an extended window style, use the AdjustWindowRectEx function.
		/// </summary>
		/// <param name="lpRect">
		///     [in, out] Pointer to a RECT structure that contains the coordinates of the top-left and
		///     bottom-right corners of the desired client area. When the function returns, the structure contains the coordinates
		///     of the top-left and bottom-right corners of the window to accommodate the desired client area.
		/// </param>
		/// <param name="dwStyle">
		///     [in] Specifies the window style of the window whose required size is to be calculated. Note that
		///     you cannot specify the WS_OVERLAPPED style.
		/// </param>
		/// <param name="bMenu"> [in] Specifies whether the window has a menu. </param>
		/// <returns>
		///     If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To
		///     get extended error information, call GetLastError.
		/// </returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool AdjustWindowRect(ref RECT lpRect, uint dwStyle, bool bMenu);

		/// <summary>
		///     The AdjustWindowRectEx function calculates the required size of the window rectangle, based on the desired size of
		///     the client rectangle. The window rectangle can then be passed to the CreateWindowEx function to create a window
		///     whose client area is the desired size.
		/// </summary>
		/// <param name="lpRect">
		///     [in, out] Pointer to a RECT structure that contains the coordinates of the top-left and
		///     bottom-right corners of the desired client area. When the function returns, the structure contains the coordinates
		///     of the top-left and bottom-right corners of the window to accommodate the desired client area.
		/// </param>
		/// <param name="dwStyle">
		///     [in] Specifies the window style of the window whose required size is to be calculated. Note that
		///     you cannot specify the WS_OVERLAPPED style.
		/// </param>
		/// <param name="bMenu"> [in] Specifies whether the window has a menu. </param>
		/// <param name="dwExStyle">
		///     [in] Specifies the extended window style of the window whose required size is to be
		///     calculated. For more information, see CreateWindowEx.
		/// </param>
		/// <returns>
		///     If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To
		///     get extended error information, call GetLastError.
		/// </returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool AdjustWindowRectEx(ref RECT lpRect, uint dwStyle, bool bMenu, uint dwExStyle);

		/// <summary>
		///     The AnimateWindow function enables you to produce special effects when showing or hiding windows. There are four
		///     types of animation: roll, slide, collapse or expand, and alpha-blended fade.
		/// </summary>
		/// <param name="hwnd"> [in] Handle to the window to animate. The calling thread must own this window. </param>
		/// <param name="time">
		///     [in] Specifies how long it takes to play the animation, in milliseconds. Typically, an animation
		///     takes 200 milliseconds to play.
		/// </param>
		/// <param name="flags"> [in] Specifies the type of animation. </param>
		/// <returns> </returns>
		[DllImport("user32", SetLastError = true)]
		public static extern bool AnimateWindow(IntPtr hwnd, int time, AnimateWindowFlags flags);

		/// <summary>
		///     The BringWindowToTop function brings the specified window to the top of the Z order. If the window is a top-level
		///     window, it is activated. If the window is a child window, the top-level parent window associated with the child
		///     window is activated.
		/// </summary>
		/// <param name="hwnd"> [in] Handle to the window to bring to the top of the Z order. </param>
		/// <returns> </returns>
		[DllImport("user32", SetLastError = true)]
		public static extern bool BringWindowToTop(IntPtr hwnd);

		/// <summary>
		///     The ChildWindowFromPoint function determines which, if any, of the child windows belonging to a parent window
		///     contains the specified point. The search is restricted to immediate child windows. Grandchildren, and deeper
		///     descendant windows are not searched.
		/// </summary>
		/// <param name="hWndParent"> [in] Handle to the parent window. </param>
		/// <param name="point">
		///     [in] Specifies a POINT structure that defines the client coordinates (relative to hWndParent of
		///     the point to be checked.
		/// </param>
		/// <returns> </returns>
		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr ChildWindowFromPoint(IntPtr hWndParent, ref POINT point);

		/// <summary>
		///     The ChildWindowFromPointEx function determines which, if any, of the child windows belonging to the specified
		///     parent window contains the specified point. The function can ignore invisible, disabled, and transparent child
		///     windows. The search is restricted to immediate child windows. Grandchildren and deeper descendants are not
		///     searched.
		/// </summary>
		/// <param name="hwndParent"> [in] Handle to the parent window. </param>
		/// <param name="pt">
		///     [in] Specifies a POINT structure that defines the client coordinates (relative to hwndParent) of the
		///     point to be checked.
		/// </param>
		/// <param name="uFlags"> </param>
		/// <returns> </returns>
		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr ChildWindowFromPointEx(IntPtr hwndParent, POINT pt, ChildWindowFromPointFlags uFlags);

		/// <summary>
		///     The CloseWindow function minimizes (but does not destroy) the specified window.
		/// </summary>
		/// <param name="hWnd"> [in] Handle to the window to be minimized. </param>
		/// <returns> </returns>
		[DllImport("user32", SetLastError = true)]
		public static extern Boolean CloseWindow(IntPtr hWnd);

		/// <summary>
		///     The DestroyWindow function destroys the specified window. The function sends WM_DESTROY and WM_NCDESTROY messages
		///     to the window to deactivate it and remove the keyboard focus from it. The function also destroys the window's menu,
		///     flushes the thread message queue, destroys timers, removes clipboard ownership, and breaks the clipboard viewer
		///     chain (if the window is at the top of the viewer chain).
		///     If the specified window is a parent or owner window, DestroyWindow automatically destroys the associated child or
		///     owned windows when it destroys the parent or owner window. The function first destroys child or owned windows, and
		///     then it destroys the parent or owner window.
		///     DestroyWindow also destroys modeless dialog boxes created by the CreateDialog function.
		/// </summary>
		/// <param name="hWnd"> [in] Handle to the window to be destroyed. </param>
		/// <returns> </returns>
		[DllImport("user32.dll", SetLastError = true, EntryPoint = "DestroyWindow", CallingConvention = CallingConvention.StdCall)]
		public static extern Boolean DestroyWindow(IntPtr hWnd);

		/// <summary>
		///     The EndTask function is called to forcibly close a specified window.
		/// </summary>
		/// <param name="hWnd"> [in] Handle to the window to be closed. </param>
		/// <param name="fShutDown"> [in] Ignored. Must be FALSE. </param>
		/// <param name="fForce">
		///     [in] A TRUE for this parameter will force the destruction of the window if an initial attempt
		///     fails to gently close the window using WM_CLOSE. With a FALSE for this parameter, only the close with WM_CLOSE is
		///     attempted.
		/// </param>
		/// <returns> </returns>
		[DllImport("user32", SetLastError = true)]
		public static extern Boolean EndTask([In] IntPtr hWnd, [In] Boolean fShutDown, [In] Boolean fForce);

		[DllImport("user32", SetLastError = true)]
		public static extern WNDENUMPROC EnumChildProc(IntPtr hwnd, IntPtr lParam);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean EnumChildWindows(IntPtr hWndParent, WNDENUMPROC lpEnumFunc, IntPtr lParam);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean EnumThreadWindows(Int64 dwThreadId, WNDENUMPROC lpfn, IntPtr lParam);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean EnumWindows(WNDENUMPROC lpEnumFunc, IntPtr lParam);

		/// <summary>
		///     The FindWindow function retrieves a handle to the top-level window whose class name and window name match the
		///     specified strings. This function does not search child windows. This function does not perform a case-sensitive
		///     search.
		///     To search child windows, beginning with a specified child window, use the FindWindowEx function.
		/// </summary>
		/// <param name="lpClassName">
		///     [in] Pointer to a null-terminated string that specifies the class name or a class atom
		///     created by a previous call to the RegisterClass or RegisterClassEx function. The atom must be in the low-order word
		///     of lpClassName; the high-order word must be zero. If lpClassName points to a string, it specifies the window class
		///     name. The class name can be any name registered with RegisterClass or RegisterClassEx, or any of the predefined
		///     control-class names. If lpClassName is NULL, it finds any window whose title matches the lpWindowName parameter.
		/// </param>
		/// <param name="lpWindowName">
		///     [in] Pointer to a null-terminated string that specifies the window name (the window's
		///     title). If this parameter is NULL, all window names match.
		/// </param>
		/// <returns>
		///     If the function succeeds, the return value is a handle to the window that has the specified class name and
		///     window name. If the function fails, the return value is NULL. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

		/// <summary>
		///     The FindWindowEx function retrieves a handle to a window whose class name and window name match the specified
		///     strings. The function searches child windows, beginning with the one following the specified child window. This
		///     function does not perform a case-sensitive search.
		/// </summary>
		/// <param name="hwndParent">
		///     [in] Handle to the parent window whose child windows are to be searched. If hwndParent is
		///     NULL, the function uses the desktop window as the parent window. The function searches among windows that are child
		///     windows of the desktop. Microsoft Windows 2000 and Windows XP: If hwndParent is HWND_MESSAGE, the function searches
		///     all message-only windows.
		/// </param>
		/// <param name="hwndChildAfter">
		///     [in] Handle to a child window. The search begins with the next child window in the Z
		///     order. The child window must be a direct child window of hwndParent, not just a descendant window. If
		///     hwndChildAfter is NULL, the search begins with the first child window of hwndParent. Note that if both hwndParent
		///     and hwndChildAfter are NULL, the function searches all top-level and message-only windows.
		/// </param>
		/// <param name="lpszClass">
		///     [in] Pointer to a null-terminated string that specifies the class name or a class atom created
		///     by a previous call to the RegisterClass or RegisterClassEx function. The atom must be placed in the low-order word
		///     of lpszClass; the high-order word must be zero. If lpszClass is a string, it specifies the window class name. The
		///     class name can be any name registered with RegisterClass or RegisterClassEx, or any of the predefined control-class
		///     names, or it can be MAKEINTATOM(0x800). In this latter case, 0x8000 is the atom for a menu class. For more
		///     information, see the Remarks section of this topic.
		/// </param>
		/// <param name="lpszWindow">
		///     [in] Pointer to a null-terminated string that specifies the window name (the window's title).
		///     If this parameter is NULL, all window names match.
		/// </param>
		/// <returns>
		///     If the function succeeds, the return value is a handle to the window that has the specified class and window
		///     names. If the function fails, the return value is NULL. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, String lpszClass, String lpszWindow);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean GetAltTabInfo(IntPtr hwnd, int iItem, PALTTABINFO pati, String pszItemText, UInt32 cchItemText);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean GetClientRect(IntPtr hWnd, LPRECT lpRect);

		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr GetDesktopWindow();

		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr GetParent(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr GetShellWindow();

		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr GetTopWindow(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCommand wCmd);

		[DllImport("user32", SetLastError = true)]
		public static extern UInt32 GetWindowModuleFileName(IntPtr hwnd, String lpszFileName, UInt32 cchFileNameMax);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern uint GetWindowModuleFileName(IntPtr hwnd, StringBuilder lpszFileName, uint cchFileNameMax);

		[DllImport("user32", SetLastError = true)]
		public static extern int GetWindowText(IntPtr hWnd, String lpString, int nMaxCount);

		[DllImport("user32.dll", EntryPoint = "GetWindowText", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

		[DllImport("user32", SetLastError = true)]
		public static extern int GetWindowTextLength(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		public static extern Int64 GetWindowThreadProcessId(IntPtr hWnd, IntPtr lpdwProcessId);

		[DllImport("user32", SetLastError = true)]
		public static extern int InternalGetWindowText(IntPtr hWnd, String lpString, int nMaxCount);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean IsChild(IntPtr hWndParent, IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean IsGUIThread(Boolean bConvert);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean IsHungAppWindow(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean IsIconic(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean IsProcessDPIAware();

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean IsWindow(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean IsWindowUnicode(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean IsWindowVisible(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean IsZoomed(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean LockSetForegroundWindow(UInt32 uLockCode);

		[DllImport("user32", SetLastError = true)]
		public static extern void LogicalToPhysicalPoint(IntPtr hWnd, POINT lpPoint);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, Boolean bRepaint);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean OpenIcon(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		public static extern void PhysicalToLogicalPoint(IntPtr hWnd, POINT lpPoint);

		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr RealChildWindowFromPoint(IntPtr hwndParent, POINT ptParentClientCoords);

		[DllImport("user32", SetLastError = true)]
		public static extern UInt32 RealGetWindowClass(IntPtr hwnd, String pszType, UInt32 cchType);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean RegisterShellHookWindow(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean SetProcessDefaultLayout(Int64 dwDefaultLayout);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean SetProcessDPIAware();

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, UInt32 uFlags);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean SetWindowText(IntPtr hWnd, String lpString);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean ShowOwnedPopups(IntPtr hWnd, Boolean fShow);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean ShowWindowAsync(IntPtr hWnd, int nCmdShow);

		[DllImport("user32", SetLastError = true)]
		public static extern void SwitchToThisWindow(IntPtr hWnd, Boolean fAltTab);

		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr WindowFromPhysicalPoint(POINT Point);

		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr WindowFromPoint(POINT Point);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean EnableWindow(IntPtr hWnd, Boolean bEnable);

		[DllImport("user32", SetLastError = true)]
		public static extern Boolean IsWindowEnabled(IntPtr hWnd);

		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr GetMenu(IntPtr hWnd);

		//[DllImport("kernel32.dll")]
		//public static extern uint GetLastError();

		[DllImport("advapi32.dll")]
		public static extern bool InitiateSystemShutdown([MarshalAs(UnmanagedType.LPStr)] string lpMachinename,
			[MarshalAs(UnmanagedType.LPStr)] string lpMessage,
			Int32 dwTimeout,
			bool bForceAppsClosed,
			bool bRebootAfterShutdown);

		[DllImport("kernel32.dll")]
		public static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, [Out] StringBuilder lpBuffer, uint nSize, IntPtr Arguments);

		// the version, the sample is built upon:
		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, ref IntPtr lpBuffer, uint nSize, IntPtr pArguments);

		// the parameters can also be passed as a string array:
		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, ref IntPtr lpBuffer, uint nSize, string[] Arguments);

		[DllImport("user32.dll")]
		public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDesktopWindowsDelegate lpfn, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetActiveWindow();

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetFocus();

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetActiveWindow(IntPtr hWnd);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern int GetDlgCtrlID(IntPtr hwndCtl);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern UInt32 GetDlgItemText(IntPtr hDlg, int nIDDlgItem, String lpString, int nMaxCount);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetNextDlgGroupItem(IntPtr hDlg, IntPtr hCtl, Boolean bPrevious);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetNextDlgTabItem(IntPtr hDlg, IntPtr hCtl, Boolean bPrevious);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern Boolean SetDlgItemInt(IntPtr hDlg, int nIDDlgItem, UInt32 uValue, Boolean bSigned);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern Boolean SetDlgItemText(IntPtr hDlg, int nIDDlgItem, String lpString);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern int GetClassName(IntPtr hWnd, String lpClassName, int nMaxCount);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern Boolean FlashWindow(IntPtr hWnd, Boolean bInvert);
		#endregion

		#region General Declarations
		// Various important window messages

		// FormatMessage constants and structs.
		internal const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

		// App recovery and restart return codes
		internal const uint ResultFailed = 0x80004005;
		internal const uint ResultFalse = 1;
		internal const uint ResultInvalidArgument = 0x80070057;
		internal const uint ResultNotFound = 0x80070490;
		internal const int WM_ENTERIDLE = 0x0121;
		internal const int WM_USER = 0x0400;

		/// <summary>
		///     Gets the HiWord
		/// </summary>
		/// <param name="dword"> The value to get the hi word from. </param>
		/// <param name="size"> Size </param>
		/// <returns> The upper half of the dword. </returns>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dword"),
		 SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "HIWORD")]
		public static int HIWORD(long dword, int size)
		{
			return (short)(dword >> size);
		}

		/// <summary>
		///     Gets the LoWord
		/// </summary>
		/// <param name="dword"> The value to get the low word from. </param>
		/// <returns> The lower half of the dword. </returns>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "LOWORD"),
		 SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dword")]
		public static int LOWORD(long dword)
		{
			return (short)(dword & 0xFFFF);
		}
		#endregion

		#region GDI and DWM Declarations
		// Disabled non-client rendering; window style is ignored.
		internal const int DWM_BB_BLURREGION = 0x00000002; // hRgnBlur has been specified
		internal const int DWM_BB_ENABLE = 0x00000001; // fEnable has been specified

		internal const int DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004;
		// fTransitionOnMaximized has been specified

		internal const int DWMNCRP_DISABLED = 1;
		// Enabled non-client rendering; window style is ignored.
		internal const int DWMNCRP_ENABLED = 2;
		internal const int DWMNCRP_USEWINDOWSTYLE = 0;
		// Enable/disable non-client rendering Use DWMNCRP_* values.
		internal const int DWMWA_NCRENDERING_ENABLED = 1;
		// Non-client rendering policy.
		internal const int DWMWA_NCRENDERING_POLICY = 2;
		// Potentially enable/forcibly disable transitions 0 or 1.
		internal const int DWMWA_TRANSITIONS_FORCEDISABLED = 3;

		#region Nested type: DWM_BLURBEHIND
		[StructLayout(LayoutKind.Sequential)]
		internal struct DWM_BLURBEHIND
		{
			public DwmBlurBehindDwFlags dwFlags;
			public bool fEnable;
			public IntPtr hRgnBlur;
			public bool fTransitionOnMaximized;
		};
		#endregion

		#region Nested type: DWM_PRESENT_PARAMETERS
		[StructLayout(LayoutKind.Sequential)]
		internal struct DWM_PRESENT_PARAMETERS
		{
			internal int cbSize;
			internal bool fQueue;
			internal UInt64 cRefreshStart;
			internal uint cBuffer;
			internal bool fUseSourceRate;
			internal UNSIGNED_RATIO uiNumerator;
		};
		#endregion

		#region Nested type: DWM_THUMBNAIL_PROPERTIES
		[StructLayout(LayoutKind.Sequential)]
		internal struct DWM_THUMBNAIL_PROPERTIES
		{
			internal DwmThumbnailFlags dwFlags;
			internal RECT rcDestination;
			internal RECT rcSource;
			internal byte opacity;
			internal bool fVisible;
			internal bool fSourceClientAreaOnly;
		};
		#endregion

		#region Nested type: DwmBlurBehindDwFlags
		internal enum DwmBlurBehindDwFlags : uint
		{
			DWM_BB_ENABLE = 0x00000001,
			DWM_BB_BLURREGION = 0x00000002,
			DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004
		}
		#endregion

		#region Nested type: DwmThumbnailFlags
		internal enum DwmThumbnailFlags : uint
		{
			DWM_TNP_RECTDESTINATION = 0x00000001,
			//Indicates a value for rcDestination has been specified.
			DWM_TNP_RECTSOURCE = 0x00000002, //Indicates a value for rcSource has been specified.
			DWM_TNP_OPACITY = 0x00000004, //Indicates a value for opacity has been specified.
			DWM_TNP_VISIBLE = 0x00000008, // Indicates a value for fVisible has been specified.
			DWM_TNP_SOURCECLIENTAREAONLY = 0x00000010
			//Indicates a value for fSourceClientAreaOnly has been specified.
		}
		#endregion

		#region Nested type: MARGINS
		[StructLayout(LayoutKind.Sequential)]
		internal struct MARGINS
		{
			public int cxLeftWidth; // width of left border that retains its size
			public int cxRightWidth; // width of right border that retains its size
			public int cyTopHeight; // height of top border that retains its size
			public int cyBottomHeight; // height of bottom border that retains its size
		};
		#endregion

		#region Nested type: RECT
		/// <summary>
		///     A Wrapper for a RECT struct
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "RECT"),
		 SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible"), StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			/// <summary>
			///     Position of left edge
			/// </summary>
			[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
			public int left;

			/// <summary>
			///     Position of top edge
			/// </summary>
			[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
			public int top;

			/// <summary>
			///     Position of right edge
			/// </summary>
			[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
			public int right;

			/// <summary>
			///     Position of bottom edge
			/// </summary>
			[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
			public int bottom;
		};
		#endregion

		#region Nested type: SIZE
		/// <summary>
		///     A Wrapper for a SIZE struct
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "SIZE"),
		 SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible"), StructLayout(LayoutKind.Sequential)]
		public struct SIZE
		{
			/// <summary>
			///     Width
			/// </summary>
			[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "cx")]
			[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
			public int cx;

			/// <summary>
			///     Height
			/// </summary>
			[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
			public int cy;
		};
		#endregion

		#region Nested type: UNSIGNED_RATIO
		[StructLayout(LayoutKind.Sequential)]
		internal struct UNSIGNED_RATIO
		{
			internal UInt32 uiNumerator;
			internal UInt32 uiDenominator;
		};
		#endregion

		#endregion

		#region Elevation COM Object

		#region Nested type: BIND_OPTS3
		[StructLayout(LayoutKind.Sequential)]
		internal struct BIND_OPTS3
		{
			internal uint cbStruct;
			internal uint grfFlags;
			internal uint grfMode;
			internal uint dwTickCountDeadline;
			internal uint dwTrackFlags;
			internal uint dwClassContext;
			internal uint locale;
			// This will be passed as null, so the type doesn't matter.
			private readonly object pServerInfo;
			internal IntPtr hwnd;
		}
		#endregion

		#region Nested type: CLSCTX
		[Flags]
		internal enum CLSCTX
		{
			CLSCTX_INPROC_SERVER = 0x1,
			CLSCTX_INPROC_HANDLER = 0x2,
			CLSCTX_LOCAL_SERVER = 0x4,
			CLSCTX_REMOTE_SERVER = 0x10,
			CLSCTX_NO_CODE_DOWNLOAD = 0x400,
			CLSCTX_NO_CUSTOM_MARSHAL = 0x1000,
			CLSCTX_ENABLE_CODE_DOWNLOAD = 0x2000,
			CLSCTX_NO_FAILURE_LOG = 0x4000,
			CLSCTX_DISABLE_AAA = 0x8000,
			CLSCTX_ENABLE_AAA = 0x10000,
			CLSCTX_FROM_DEFAULT_CONTEXT = 0x20000,
			CLSCTX_INPROC = CLSCTX_INPROC_SERVER | CLSCTX_INPROC_HANDLER,
			CLSCTX_SERVER = CLSCTX_INPROC_SERVER | CLSCTX_LOCAL_SERVER | CLSCTX_REMOTE_SERVER,
			CLSCTX_ALL = CLSCTX_SERVER | CLSCTX_INPROC_HANDLER
		}
		#endregion

		#endregion

		#region Windows OS structs and consts
		// Code for CreateWindowEx, for a windowless message pump.
		internal const int HWND_MESSAGE = -3;

		internal const uint STATUS_ACCESS_DENIED = 0xC0000022;

		#region Nested type: MSG
		[StructLayout(LayoutKind.Sequential)]
		internal struct MSG
		{
			internal IntPtr hwnd;
			internal uint message;
			internal IntPtr wParam;
			internal IntPtr lParam;
			internal uint time;
			internal POINT pt;
		}
		#endregion

		#region Nested type: POINT
		/// <summary>
		///     A Wrapper for a POINT struct
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "POINT"),
		 SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible"), StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			/// <summary>
			///     The X coordinate of the point
			/// </summary>
			[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "X")]
			[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
			public int X;

			/// <summary>
			///     The Y coordinate of the point
			/// </summary>
			[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Y")]
			[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
			public int Y;

			/// <summary>
			///     Initialize the point
			/// </summary>
			/// <param name="x"> The x coordinate of the point. </param>
			/// <param name="y"> The y coordinate of the point. </param>
			[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y"),
			 SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
			public POINT(int x, int y)
			{
				this.X = x;
				this.Y = y;
			}
		}
		#endregion

		#region Nested type: WNDCLASSEX
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal struct WNDCLASSEX
		{
			internal uint cbSize;
			internal uint style;
			[MarshalAs(UnmanagedType.FunctionPtr)]
			internal WNDPROC lpfnWndProc;
			internal int cbClsExtra;
			internal int cbWndExtra;
			internal IntPtr hInstance;
			internal IntPtr hIcon;
			internal IntPtr hCursor;
			internal IntPtr hbrBackground;
			[MarshalAs(UnmanagedType.LPTStr)]
			internal string lpszMenuName;
			[MarshalAs(UnmanagedType.LPTStr)]
			internal string lpszClassName;
			internal IntPtr hIconSm;
		}
		#endregion

		#region Nested type: WNDPROC
		internal delegate int WNDPROC(IntPtr hWnd, uint uMessage, IntPtr wParam, IntPtr lParam);
		#endregion

		#endregion
	}
}