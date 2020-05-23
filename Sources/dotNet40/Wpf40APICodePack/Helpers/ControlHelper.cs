#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using Microsoft.WindowsAPICodePack.Shell;

namespace Library40.Wpf.Helpers
{
	public static class ControlHelper
	{
		/// <summary>
		///     Sets the layout of current window.
		/// </summary>
		/// <param name="window">The window.</param>
		public static void SetLayout(this GlassWindow window)
		{
			//return;
			//Action setLayout = delegate
			//{
			//    if (!window.AeroGlassCompositionEnabled)
			//        window.Background = Brushes.Teal;
			//    else
			//    {
			//        window.SetAeroGlassTransparency();
			//        window.InvalidateVisual();
			//    }
			//};
			//window.AeroGlassCompositionChanged += (e1, e2) => setLayout();
			//setLayout();
			window.SetAeroGlassTransparency();
			window.InvalidateVisual();
		}
	}
}