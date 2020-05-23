using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Mohammad.Wpf.Win32;
using Brushes = System.Windows.Media.Brushes;
using SystemColors = System.Windows.SystemColors;

namespace Mohammad.Wpf.Helpers
{
    public static class ControlHelperCodePack
    {
        public static void ExtendGlass(this Window window, Thickness thikness)
        {
            if (Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 2)
                return;
            try
            {
                var isGlassEnabled = 0;
                AeroGlassApi.DwmIsCompositionEnabled(ref isGlassEnabled);
                if (Environment.OSVersion.Version.Major > 5 && isGlassEnabled > 0)
                {
                    // Get the window handle
                    var helper = new WindowInteropHelper(window);
                    var mainWindowSrc = HwndSource.FromHwnd(helper.Handle);
                    mainWindowSrc.CompositionTarget.BackgroundColor = Colors.Transparent;

                    // Get the dpi of the screen
                    var desktop = Graphics.FromHwnd(mainWindowSrc.Handle);
                    var dpiX = desktop.DpiX / 96;
                    var dpiY = desktop.DpiY / 96;

                    // Set Margins
                    var margins = new MARGINS
                                  {
                                      cxLeftWidth = (int) (thikness.Left * dpiX),
                                      cxRightWidth = (int) (thikness.Right * dpiX),
                                      cyBottomHeight = (int) (thikness.Bottom * dpiY),
                                      cyTopHeight = (int) (thikness.Top * dpiY)
                                  };

                    window.Background = Brushes.Transparent;
                    AeroGlassApi.DwmExtendFrameIntoClientArea(mainWindowSrc.Handle, ref margins);
                }
                else
                {
                    window.Background = SystemColors.WindowBrush;
                }
            }
            catch (DllNotFoundException) {}
        }
    }
}