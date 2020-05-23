using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Mohammad.Helpers;
using Mohammad.Win32.Natives.IfacesEnumsStructsClasses;
using DWORD = System.Int64;
using LPOVERLAPPED = System.IntPtr;
using HWND = System.IntPtr;
using LONG = System.Int64;
using LONG_PTR = System.IntPtr;

//using BYTE = UChar;
// ReSharper disable InconsistentNaming
// ReSharper disable BuiltInTypeReferenceStyle

namespace Mohammad.Win32.Natives
{
    public partial class Api
    {
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
        // Code for CreateWindowEx, for a windowless message pump.
        internal const int HWND_MESSAGE = -3;
        internal const uint STATUS_ACCESS_DENIED = 0xC0000022;

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", EntryPoint = "EnableWindow", CharSet = CharSet.Auto)]
        public static extern bool EnableWindow(HandleRef hWnd, bool enable);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool AnimateWindow(LPOVERLAPPED hwnd, long dwTime, long dwFlags);

        /// <summary>
        ///     Enables you to produce special effects when showing or hiding windows. There are four types of animation: roll,
        ///     slide, collapse or expand, and alpha-blended fade.
        /// </summary>
        /// <param name="hwnd">Handle to the window to animate. The calling thread must own this window.</param>
        /// <param name="dwTime">
        ///     Specifies how long it takes to play the animation, in milliseconds. Typically, an animation takes
        ///     200 milliseconds to play.
        /// </param>
        /// <param name="dwFlags">
        ///     Specifies the type of animation. This parameter can be one or more of the following values. Note
        ///     that, by default, these flags take effect when showing a window. To take effect when hiding a window, use AW_HIDE
        ///     and a logical OR operator with the appropriate flags.
        /// </param>
        /// <returns></returns>
        public static bool AnimateWindow(LPOVERLAPPED hwnd, long dwTime, WindowAnimateFlags dwFlags)
        {
            return AnimateWindow(hwnd, dwTime, dwFlags.ToLong());
        }

        public static int GetLastError() { return Marshal.GetLastWin32Error(); }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern void SetLastErrorEx(DWORD dwErrCode, DWORD dwType);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern void SetLastError(uint dwErrCode);

        /// <summary>
        ///     Sends the specified message to a window or windows. The SendMessage function calls
        ///     the window procedure for the specified window and does not return until the window
        ///     procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">
        ///     Handle to the window whose window procedure will receive the message.
        ///     If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system,
        ///     including disabled or invisible unowned windows, overlapped windows, and pop-up windows;
        ///     but the message is not sent to child windows.
        /// </param>
        /// <param name="msg">Specifies the message to be sent.</param>
        /// <param name="wParam">Specifies additional message-specific information.</param>
        /// <param name="lParam">Specifies additional message-specific information.</param>
        /// <returns>A return code specific to the message being sent.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "w")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "l")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "h")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wnd")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Param")]
        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible",
            Justification = "This is used from another assembly, also it's in an internal namespace")]
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern LPOVERLAPPED SendMessage(LPOVERLAPPED hWnd, uint msg, LPOVERLAPPED wParam, LPOVERLAPPED lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool PostMessage(LPOVERLAPPED hWnd, uint msg, int wParam, int lParam);

        /// <summary>
        ///     Sends the specified message to a window or windows. The SendMessage function calls
        ///     the window procedure for the specified window and does not return until the window
        ///     procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">
        ///     Handle to the window whose window procedure will receive the message.
        ///     If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system,
        ///     including disabled or invisible unowned windows, overlapped windows, and pop-up windows;
        ///     but the message is not sent to child windows.
        /// </param>
        /// <param name="msg">Specifies the message to be sent.</param>
        /// <param name="wParam">Specifies additional message-specific information.</param>
        /// <param name="lParam">Specifies additional message-specific information.</param>
        /// <returns>A return code specific to the message being sent.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "w")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "l")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "h")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wnd")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Param")]
        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern LPOVERLAPPED SendMessage(LPOVERLAPPED hWnd, uint msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern LPOVERLAPPED SendMessage(LPOVERLAPPED hWnd, uint msg, int wParam, int lParam);

        /// <summary>
        ///     Sends the specified message to a window or windows. The SendMessage function calls
        ///     the window procedure for the specified window and does not return until the window
        ///     procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">
        ///     Handle to the window whose window procedure will receive the message.
        ///     If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system,
        ///     including disabled or invisible unowned windows, overlapped windows, and pop-up windows;
        ///     but the message is not sent to child windows.
        /// </param>
        /// <param name="msg">Specifies the message to be sent.</param>
        /// <param name="wParam">Specifies additional message-specific information.</param>
        /// <param name="lParam">Specifies additional message-specific information.</param>
        /// <returns>A return code specific to the message being sent.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Param")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wnd")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "h")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "l")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "w")]
        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "2#", Justification = "This is an in/out parameter")]
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern LPOVERLAPPED SendMessage(LPOVERLAPPED hWnd, uint msg, ref int wParam, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);

        // Various helpers for forcing binding to proper 
        // version of Comctl32 (v6).
        [DllImport("kernel32.dll", SetLastError = true, ThrowOnUnmappableChar = true, BestFitMapping = false)]
        public static extern LPOVERLAPPED LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(LPOVERLAPPED graphicsObjectHandle);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int LoadString(LPOVERLAPPED hInstance, int uID, StringBuilder buffer, int nBufferMax);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(LPOVERLAPPED hwnd, int index);

        /// <summary>
        ///     Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified
        ///     offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs..</param>
        /// <param name="nIndex">
        ///     The zero-based offset to the value to be set. Valid values are in the range zero through the
        ///     number of bytes of extra window memory, minus the size of an integer. To set any other value, specify one of the
        ///     following values: GWL_EXSTYLE, GWL_HINSTANCE, GWL_ID, GWL_STYLE, GWL_USERDATA, GWL_WNDPROC
        /// </param>
        /// <param name="dwNewLong">The replacement value.</param>
        /// <returns>
        ///     If the function succeeds, the return value is the previous value of the specified 32-bit integer.
        ///     If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern LONG SetWindowLong([In] HWND hWnd, [In] int nIndex, [In] LONG dwNewLong);

        [DllImport("user32.dll")]
        public static extern LONG_PTR SetWindowLongPtr([In] HWND hWnd, [In] int nIndex, [In] LONG_PTR dwNewLong);

        public static LPOVERLAPPED SetWindowLongPtr(HandleRef hWnd, int nIndex, LPOVERLAPPED dwNewLong)
        {
            return LPOVERLAPPED.Size == 8 ? SetWindowLongPtr64(hWnd, nIndex, dwNewLong) : new LPOVERLAPPED(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern LPOVERLAPPED SetWindowLongPtr64(HandleRef hWnd, int nIndex, LPOVERLAPPED dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(LPOVERLAPPED hwnd, uint crKey, byte bAlpha, uint dwFlags);

        /// <summary>
        ///     Destroys an icon and frees any memory the icon occupied.
        /// </summary>
        /// <param name="hIcon">Handle to the icon to be destroyed. The icon must not be in use. </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get
        ///     extended error information, call GetLastError.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "h")]
        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible",
            Justification = "This is used from other assemblies, also it's in an internal namespace")]
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyIcon(LPOVERLAPPED hIcon);

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
        /// <param name="bMenu">[in] Specifies whether the window has a menu.</param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     If the function fails, the return value is zero. To get extended error information, call GetLastError.
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
        /// <param name="bMenu">[in] Specifies whether the window has a menu.</param>
        /// <param name="dwExStyle">
        ///     [in] Specifies the extended window style of the window whose required size is to be calculated.
        ///     For more information, see CreateWindowEx.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool AdjustWindowRectEx(ref RECT lpRect, uint dwStyle, bool bMenu, uint dwExStyle);

        /// <summary>
        ///     The AnimateWindow function enables you to produce special effects when showing or hiding windows. There are four
        ///     types of animation: roll, slide, collapse or expand, and alpha-blended fade.
        /// </summary>
        /// <param name="hwnd">[in] Handle to the window to animate. The calling thread must own this window.</param>
        /// <param name="time">
        ///     [in] Specifies how long it takes to play the animation, in milliseconds. Typically, an animation
        ///     takes 200 milliseconds to play.
        /// </param>
        /// <param name="flags">[in] Specifies the type of animation.</param>
        /// <returns></returns>
        [DllImport("user32", SetLastError = true)]
        public static extern bool AnimateWindow(LPOVERLAPPED hwnd, int time, AnimateWindowFlags flags);

        /// <summary>
        ///     The BringWindowToTop function brings the specified window to the top of the Z order. If the window is a top-level
        ///     window, it is activated. If the window is a child window, the top-level parent window associated with the child
        ///     window is activated.
        /// </summary>
        /// <param name="hwnd">[in] Handle to the window to bring to the top of the Z order.</param>
        /// <returns></returns>
        [DllImport("user32", SetLastError = true)]
        public static extern bool BringWindowToTop(LPOVERLAPPED hwnd);

        /// <summary>
        ///     The ChildWindowFromPoint function determines which, if any, of the child windows belonging to a parent window
        ///     contains the specified point. The search is restricted to immediate child windows. Grandchildren, and deeper
        ///     descendant windows are not searched.
        /// </summary>
        /// <param name="hWndParent">[in] Handle to the parent window.</param>
        /// <param name="point">
        ///     [in] Specifies a POINT structure that defines the client coordinates (relative to hWndParent of the
        ///     point to be checked.
        /// </param>
        /// <returns></returns>
        [DllImport("user32", SetLastError = true)]
        public static extern LPOVERLAPPED ChildWindowFromPoint(LPOVERLAPPED hWndParent, ref POINT point);

        /// <summary>
        ///     The ChildWindowFromPointEx function determines which, if any, of the child windows belonging to the specified
        ///     parent window contains the specified point. The function can ignore invisible, disabled, and transparent child
        ///     windows. The search is restricted to immediate child windows. Grandchildren and deeper descendants are not
        ///     searched.
        /// </summary>
        /// <param name="hwndParent">[in] Handle to the parent window.</param>
        /// <param name="pt">
        ///     [in] Specifies a POINT structure that defines the client coordinates (relative to hwndParent) of the
        ///     point to be checked.
        /// </param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport("user32", SetLastError = true)]
        public static extern LPOVERLAPPED ChildWindowFromPointEx(LPOVERLAPPED hwndParent, POINT pt, ChildWindowFromPointFlags uFlags);

        /// <summary>
        ///     The CloseWindow function minimizes (but does not destroy) the specified window.
        /// </summary>
        /// <param name="hWnd">[in] Handle to the window to be minimized.</param>
        /// <returns></returns>
        [DllImport("user32", SetLastError = true)]
        public static extern bool CloseWindow(LPOVERLAPPED hWnd);

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
        /// <param name="hWnd">[in] Handle to the window to be destroyed.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "DestroyWindow", CallingConvention = CallingConvention.StdCall)]
        public static extern bool DestroyWindow(LPOVERLAPPED hWnd);

        /// <summary>
        ///     The EndTask function is called to forcibly close a specified window.
        /// </summary>
        /// <param name="hWnd">[in] Handle to the window to be closed.</param>
        /// <param name="fShutDown">[in] Ignored. Must be FALSE.</param>
        /// <param name="fForce">
        ///     [in] A TRUE for this parameter will force the destruction of the window if an initial attempt
        ///     fails to gently close the window using WM_CLOSE. With a FALSE for this parameter, only the close with WM_CLOSE is
        ///     attempted.
        /// </param>
        /// <returns></returns>
        [DllImport("user32", SetLastError = true)]
        public static extern bool EndTask([In] LPOVERLAPPED hWnd, [In] bool fShutDown, [In] bool fForce);

        [DllImport("user32", SetLastError = true)]
        public static extern WNDENUMPROC EnumChildProc(LPOVERLAPPED hwnd, LPOVERLAPPED lParam);

        [DllImport("user32", SetLastError = true)]
        public static extern bool EnumChildWindows(LPOVERLAPPED hWndParent, WNDENUMPROC lpEnumFunc, LPOVERLAPPED lParam);

        [DllImport("user32", SetLastError = true)]
        public static extern bool EnumThreadWindows(long dwThreadId, WNDENUMPROC lpfn, LPOVERLAPPED lParam);

        [DllImport("user32", SetLastError = true)]
        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, LPOVERLAPPED lParam);

        /// <summary>
        ///     The FindWindow function retrieves a handle to the top-level window whose class name and window name match the
        ///     specified strings. This function does not search child windows. This function does not perform a case-sensitive
        ///     search.
        ///     To search child windows, beginning with a specified child window, use the FindWindowEx function.
        /// </summary>
        /// <param name="lpClassName">
        ///     [in] Pointer to a null-terminated string that specifies the class name or a class atom created by a previous call
        ///     to the RegisterClass or RegisterClassEx function. The atom must be in the low-order word of lpClassName; the
        ///     high-order word must be zero.
        ///     If lpClassName points to a string, it specifies the window class name. The class name can be any name registered
        ///     with RegisterClass or RegisterClassEx, or any of the predefined control-class names.
        ///     If lpClassName is NULL, it finds any window whose title matches the lpWindowName parameter.
        /// </param>
        /// <param name="lpWindowName">
        ///     [in] Pointer to a null-terminated string that specifies the window name (the window's
        ///     title). If this parameter is NULL, all window names match.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is a handle to the window that has the specified class name and window
        ///     name.
        ///     If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32", SetLastError = true)]
        public static extern LPOVERLAPPED FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        ///     The FindWindowEx function retrieves a handle to a window whose class name and window name match the specified
        ///     strings. The function searches child windows, beginning with the one following the specified child window. This
        ///     function does not perform a case-sensitive search.
        /// </summary>
        /// <param name="hwndParent">
        ///     [in] Handle to the parent window whose child windows are to be searched.
        ///     If hwndParent is NULL, the function uses the desktop window as the parent window. The function searches among
        ///     windows that are child windows of the desktop.
        ///     Microsoft Windows 2000 and Windows XP: If hwndParent is HWND_MESSAGE, the function searches all message-only
        ///     windows.
        /// </param>
        /// <param name="hwndChildAfter">
        ///     [in] Handle to a child window. The search begins with the next child window in the Z order. The child window must
        ///     be a direct child window of hwndParent, not just a descendant window.
        ///     If hwndChildAfter is NULL, the search begins with the first child window of hwndParent.
        ///     Note that if both hwndParent and hwndChildAfter are NULL, the function searches all top-level and message-only
        ///     windows.
        /// </param>
        /// <param name="lpszClass">
        ///     [in] Pointer to a null-terminated string that specifies the class name or a class atom created by a previous call
        ///     to the RegisterClass or RegisterClassEx function. The atom must be placed in the low-order word of lpszClass; the
        ///     high-order word must be zero.
        ///     If lpszClass is a string, it specifies the window class name. The class name can be any name registered with
        ///     RegisterClass or RegisterClassEx, or any of the predefined control-class names, or it can be MAKEINTATOM(0x800). In
        ///     this latter case, 0x8000 is the atom for a menu class. For more information, see the Remarks section of this topic.
        /// </param>
        /// <param name="lpszWindow">
        ///     [in] Pointer to a null-terminated string that specifies the window name (the window's title).
        ///     If this parameter is NULL, all window names match.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is a handle to the window that has the specified class and window names.
        ///     If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32", SetLastError = true)]
        public static extern LPOVERLAPPED FindWindowEx(LPOVERLAPPED hwndParent, LPOVERLAPPED hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32", SetLastError = true)]
        public static extern bool GetAltTabInfo(LPOVERLAPPED hwnd, int iItem, PALTTABINFO pati, string pszItemText, uint cchItemText);

        [DllImport("user32", SetLastError = true)]
        public static extern bool GetClientRect(LPOVERLAPPED hWnd, LPRECT lpRect);

        [DllImport("user32", SetLastError = true)]
        public static extern LPOVERLAPPED GetDesktopWindow();

        [DllImport("user32", SetLastError = true)]
        public static extern LPOVERLAPPED GetForegroundWindow();

        [DllImport("user32", SetLastError = true)]
        public static extern LPOVERLAPPED GetParent(LPOVERLAPPED hWnd);

        [DllImport("user32", SetLastError = true)]
        public static extern LPOVERLAPPED GetShellWindow();

        [DllImport("user32", SetLastError = true)]
        public static extern LPOVERLAPPED GetTopWindow(LPOVERLAPPED hWnd);

        [DllImport("user32", SetLastError = true)]
        public static extern LPOVERLAPPED GetWindow(LPOVERLAPPED hWnd, GetWindowCommand wCmd);

        [DllImport("user32", SetLastError = true)]
        public static extern uint GetWindowModuleFileName(LPOVERLAPPED hwnd, string lpszFileName, uint cchFileNameMax);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint GetWindowModuleFileName(LPOVERLAPPED hwnd, StringBuilder lpszFileName, uint cchFileNameMax);

        [DllImport("user32", SetLastError = true)]
        public static extern int GetWindowText(LPOVERLAPPED hWnd, string lpString, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowText", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(LPOVERLAPPED hWnd, StringBuilder lpWindowText, int nMaxCount);

        [DllImport("user32", SetLastError = true)]
        public static extern int GetWindowTextLength(LPOVERLAPPED hWnd);

        [DllImport("user32", SetLastError = true)]
        public static extern long GetWindowThreadProcessId(LPOVERLAPPED hWnd, LPOVERLAPPED lpdwProcessId);

        [DllImport("user32", SetLastError = true)]
        public static extern int InternalGetWindowText(LPOVERLAPPED hWnd, string lpString, int nMaxCount);

        [DllImport("user32", SetLastError = true)]
        public static extern bool IsChild(LPOVERLAPPED hWndParent, LPOVERLAPPED hWnd);

        [DllImport("user32", SetLastError = true)]
        public static extern bool IsGUIThread(bool bConvert);

        [DllImport("user32", SetLastError = true)]
        public static extern bool IsHungAppWindow(LPOVERLAPPED hWnd);

        [DllImport("user32", SetLastError = true)]
        public static extern bool IsIconic(LPOVERLAPPED hWnd);

        [DllImport("user32", SetLastError = true)]
        public static extern bool IsProcessDPIAware();

        [DllImport("user32", SetLastError = true)]
        public static extern bool IsWindow(LPOVERLAPPED hWnd);

        [DllImport("user32", SetLastError = true)]
        public static extern bool IsWindowUnicode(LPOVERLAPPED hWnd);

        [DllImport("user32", SetLastError = true)]
        public static extern bool IsWindowVisible(LPOVERLAPPED hWnd);

        [DllImport("user32", SetLastError = true)]
        public static extern bool IsZoomed(LPOVERLAPPED hWnd);

        [DllImport("user32", SetLastError = true)]
        public static extern bool LockSetForegroundWindow(uint uLockCode);

        [DllImport("user32", SetLastError = true)]
        public static extern void LogicalToPhysicalPoint(LPOVERLAPPED hWnd, POINT lpPoint);

        [DllImport("user32", SetLastError = true)]
        public static extern bool MoveWindow(LPOVERLAPPED hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32", SetLastError = true)]
        public static extern bool OpenIcon(LPOVERLAPPED hWnd);

        [DllImport("user32", SetLastError = true)]
        public static extern void PhysicalToLogicalPoint(LPOVERLAPPED hWnd, POINT lpPoint);

        [DllImport("user32", SetLastError = true)]
        public static extern LPOVERLAPPED RealChildWindowFromPoint(LPOVERLAPPED hwndParent, POINT ptParentClientCoords);

        [DllImport("user32", SetLastError = true)]
        public static extern uint RealGetWindowClass(LPOVERLAPPED hwnd, string pszType, uint cchType);

        [DllImport("user32", SetLastError = true)]
        public static extern bool RegisterShellHookWindow(LPOVERLAPPED hWnd);

        [DllImport("user32", SetLastError = true)]
        public static extern bool SetForegroundWindow(LPOVERLAPPED hWnd);

        [DllImport("user32", SetLastError = true)]
        public static extern LPOVERLAPPED SetParent(LPOVERLAPPED hWndChild, LPOVERLAPPED hWndNewParent);

        [DllImport("user32", SetLastError = true)]
        public static extern bool SetProcessDefaultLayout(long dwDefaultLayout);

        [DllImport("user32", SetLastError = true)]
        public static extern bool SetProcessDPIAware();

        [DllImport("user32", SetLastError = true)]
        public static extern bool SetWindowPos(LPOVERLAPPED hWnd, LPOVERLAPPED hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32", SetLastError = true)]
        public static extern bool SetWindowText(LPOVERLAPPED hWnd, string lpString);

        [DllImport("user32", SetLastError = true)]
        public static extern bool ShowOwnedPopups(LPOVERLAPPED hWnd, bool fShow);

        [DllImport("user32", SetLastError = true)]
        public static extern bool ShowWindow(LPOVERLAPPED hWnd, int nCmdShow);

        [DllImport("user32", SetLastError = true)]
        public static extern bool ShowWindowAsync(LPOVERLAPPED hWnd, int nCmdShow);

        [DllImport("user32", SetLastError = true)]
        public static extern void SwitchToThisWindow(LPOVERLAPPED hWnd, bool fAltTab);

        [DllImport("user32", SetLastError = true)]
        public static extern LPOVERLAPPED WindowFromPhysicalPoint(POINT Point);

        [DllImport("user32", SetLastError = true)]
        public static extern LPOVERLAPPED WindowFromPoint(POINT Point);

        [DllImport("user32", SetLastError = true)]
        public static extern bool EnableWindow(LPOVERLAPPED hWnd, bool bEnable);

        [DllImport("user32", SetLastError = true)]
        public static extern bool IsWindowEnabled(LPOVERLAPPED hWnd);

        [DllImport("user32", SetLastError = true)]
        public static extern LPOVERLAPPED GetMenu(LPOVERLAPPED hWnd);

        //[DllImport("kernel32.dll")]
        //public static extern uint GetLastError();

        [DllImport("advapi32.dll")]
        public static extern bool InitiateSystemShutdown([MarshalAs(UnmanagedType.LPStr)] string lpMachinename, [MarshalAs(UnmanagedType.LPStr)] string lpMessage,
            int dwTimeout, bool bForceAppsClosed, bool bRebootAfterShutdown);

        [DllImport("kernel32.dll")]
        public static extern uint FormatMessage(uint dwFlags, LPOVERLAPPED lpSource, uint dwMessageId, uint dwLanguageId, [Out] StringBuilder lpBuffer, uint nSize,
            LPOVERLAPPED Arguments);

        // the version, the sample is built upon:
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern uint FormatMessage(uint dwFlags, LPOVERLAPPED lpSource, uint dwMessageId, uint dwLanguageId, ref LPOVERLAPPED lpBuffer, uint nSize,
            LPOVERLAPPED pArguments);

        // the parameters can also be passed as a string array:
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern uint FormatMessage(uint dwFlags, LPOVERLAPPED lpSource, uint dwMessageId, uint dwLanguageId, ref LPOVERLAPPED lpBuffer, uint nSize,
            string[] Arguments);

        [DllImport("user32.dll")]
        public static extern bool EnumDesktopWindows(LPOVERLAPPED hDesktop, EnumDesktopWindowsDelegate lpfn, LPOVERLAPPED lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern LPOVERLAPPED GetActiveWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern LPOVERLAPPED GetFocus();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern LPOVERLAPPED SetActiveWindow(LPOVERLAPPED hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetDlgCtrlID(LPOVERLAPPED hwndCtl);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern LPOVERLAPPED GetDlgItem(LPOVERLAPPED hDlg, int nIDDlgItem);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetDlgItemText(LPOVERLAPPED hDlg, int nIDDlgItem, string lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern LPOVERLAPPED GetNextDlgGroupItem(LPOVERLAPPED hDlg, LPOVERLAPPED hCtl, bool bPrevious);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern LPOVERLAPPED GetNextDlgTabItem(LPOVERLAPPED hDlg, LPOVERLAPPED hCtl, bool bPrevious);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetDlgItemInt(LPOVERLAPPED hDlg, int nIDDlgItem, uint uValue, bool bSigned);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetDlgItemText(LPOVERLAPPED hDlg, int nIDDlgItem, string lpString);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetClassName(LPOVERLAPPED hWnd, string lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool FlashWindow(LPOVERLAPPED hWnd, bool bInvert);

        /// <summary>
        ///     Gets the HiWord
        /// </summary>
        /// <param name="dword">The value to get the hi word from.</param>
        /// <param name="size">Size</param>
        /// <returns>The upper half of the dword.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dword")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "HIWORD")]
        public static int HIWORD(long dword, int size)
        {
            return (short) (dword >> size);
        }

        /// <summary>
        ///     Gets the LoWord
        /// </summary>
        /// <param name="dword">The value to get the low word from.</param>
        /// <returns>The lower half of the dword.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "LOWORD")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dword")]
        public static int LOWORD(long dword)
        {
            return (short) (dword & 0xFFFF);
        }

        public delegate bool EnumDesktopWindowsDelegate(LPOVERLAPPED hWnd, int lParam);

        /// <summary>
        ///     The EnumChildProc function is an application-defined callback function used with the EnumChildWindows function. It
        ///     receives the child window handles. The WNDENUMPROC type defines a pointer to this callback function. EnumChildProc
        ///     is a placeholder for the application-defined function name.
        /// </summary>
        /// <param name="hWnd">[in] Handle to a child window of the parent window specified in EnumChildWindows.</param>
        /// <param name="lParam">[in] Specifies the application-defined value given in EnumChildWindows.</param>
        /// <returns>To continue enumeration, the callback function must return TRUE; to stop enumeration, it must return FALSE.</returns>
        public delegate bool WNDENUMPROC(LPOVERLAPPED hWnd, LPOVERLAPPED lParam);

        public struct LayeredWindowAttributes
        {
            public const uint LWA_ALPHA = 0x00000002;
            public const uint LWA_COLORKEY = 0x00000001;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DWM_BLURBEHIND
        {
            public DwmBlurBehindDwFlags dwFlags;
            public bool fEnable;
            public LPOVERLAPPED hRgnBlur;
            public bool fTransitionOnMaximized;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DWM_PRESENT_PARAMETERS
        {
            internal int cbSize;
            internal bool fQueue;
            internal ulong cRefreshStart;
            internal uint cBuffer;
            internal bool fUseSourceRate;
            internal UNSIGNED_RATIO uiNumerator;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DWM_THUMBNAIL_PROPERTIES
        {
            internal DwmThumbnailFlags dwFlags;
            internal RECT rcDestination;
            internal RECT rcSource;
            internal byte opacity;
            internal bool fVisible;
            internal bool fSourceClientAreaOnly;
        }

        internal enum DwmBlurBehindDwFlags : uint
        {
            DWM_BB_ENABLE = 0x00000001,
            DWM_BB_BLURREGION = 0x00000002,
            DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004
        }

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

        [StructLayout(LayoutKind.Sequential)]
        internal struct MARGINS
        {
            public int cxLeftWidth; // width of left border that retains its size
            public int cxRightWidth; // width of right border that retains its size
            public int cyTopHeight; // height of top border that retains its size
            public int cyBottomHeight; // height of bottom border that retains its size
        }

        /// <summary>
        ///     A Wrapper for a RECT struct
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "RECT")]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        [StructLayout(LayoutKind.Sequential)]
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
        }

        /// <summary>
        ///     A Wrapper for a SIZE struct
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "SIZE")]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        [StructLayout(LayoutKind.Sequential)]
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
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct UNSIGNED_RATIO
        {
            internal uint uiNumerator;
            internal uint uiDenominator;
        }

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
            internal LPOVERLAPPED hwnd;
        }

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

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            internal LPOVERLAPPED hwnd;
            internal uint message;
            internal LPOVERLAPPED wParam;
            internal LPOVERLAPPED lParam;
            internal uint time;
            internal POINT pt;
        }

        /// <summary>
        ///     A Wrapper for a POINT struct
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "POINT")]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        [StructLayout(LayoutKind.Sequential)]
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
            /// <param name="x">The x coordinate of the point.</param>
            /// <param name="y">The y coordinate of the point.</param>
            [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y")]
            [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct WNDCLASSEX
        {
            internal uint cbSize;
            internal uint style;

            [MarshalAs(UnmanagedType.FunctionPtr)]
            internal WNDPROC lpfnWndProc;

            internal int cbClsExtra;
            internal int cbWndExtra;
            internal LPOVERLAPPED hInstance;
            internal LPOVERLAPPED hIcon;
            internal LPOVERLAPPED hCursor;
            internal LPOVERLAPPED hbrBackground;

            [MarshalAs(UnmanagedType.LPTStr)]
            internal string lpszMenuName;

            [MarshalAs(UnmanagedType.LPTStr)]
            internal string lpszClassName;

            internal LPOVERLAPPED hIconSm;
        }

        internal delegate int WNDPROC(LPOVERLAPPED hWnd, uint uMessage, LPOVERLAPPED wParam, LPOVERLAPPED lParam);
    }
}