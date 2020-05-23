using System.Text;
using System.Windows.Forms;

namespace Mohammad.Win.Controls
{
    partial class DialogBoxController
    {
        private CallBack_EnumWinProc myEnumProc;

        private int WinProc(int uMsg, int wParam, int lParam)
        {
            var strBuffer = new StringBuilder(256);
            try
            {
                GetClassName(wParam, strBuffer, strBuffer.Capacity);
            }
            catch {}
            if (strBuffer.ToString() != "#32770")
                return 0;
            this.myEnumProc = this.EnumWinProc;

            if (uMsg != HCBT_ACTIVATE)
                return 0;
            if (this.Translate)
            {
                GetWindowText(wParam, strBuffer, strBuffer.Capacity);
                var dictionary = this.Dictionary.GetDictionary();
                if (dictionary.ContainsKey(strBuffer.ToString()))
                    SetWindowText(wParam, dictionary[strBuffer.ToString()]);
            }
            EnumChildWindows(wParam, this.myEnumProc, 0);
            if (this.RightToLeft == RightToLeft.Yes)
                SetWindowLong(wParam, GWL_EXSTYLE, GetWindowLong(wParam, GWL_EXSTYLE) | (int) WS_EX_RTLREADING | (int) WS_EX_RIGHT);
            if (this.Fade)
                AnimateWindow(wParam, 150, (int) AW_CENTER | (int) AW_BLEND);

            return 0;
        }

        private int EnumWinProc(int hWnd, int lParam)
        {
            var strBuffer = new StringBuilder(256);
            GetClassName(hWnd, strBuffer, strBuffer.Capacity);
            var ss = strBuffer.ToString();
            if (this.Translate)
            {
                var dictionary = this.Dictionary.GetDictionary();
                GetWindowText(hWnd, strBuffer, strBuffer.Capacity);
                if (string.Compare(ss, "STATIC", true) == 0 || string.Compare(ss, "BUTTON", true) == 0)
                    if (dictionary.ContainsKey(strBuffer.ToString()))
                        SetWindowText(hWnd, dictionary[strBuffer.ToString()]);
                SendMessage(hWnd, (int) WM_SETFONT, this.Font.ToHfont(), 0);
            }
            if (this.RightToLeft == RightToLeft.Yes)
                SetWindowLong(hWnd, GWL_EXSTYLE, GetWindowLong(hWnd, GWL_EXSTYLE) | (int) WS_EX_RTLREADING);

            return 1;
        }
    }
}