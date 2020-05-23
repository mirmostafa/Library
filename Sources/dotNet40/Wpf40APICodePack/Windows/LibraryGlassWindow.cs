#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Windows;
using System.Windows.Media;
using Library40.Helpers;
using Library40.Interfaces;
using Microsoft.WindowsAPICodePack.Shell;

namespace Library40.Wpf.Windows
{
	public class LibraryGlassWindow : GlassWindow, ISettingsEnabledElement
	{
		public LibraryGlassWindow()
		{
			this.Loaded += this.OnLoaded;
			this.Closed += this.OnClosed;
		}

		public event EventHandler LoadingSettings;
		public event EventHandler SavingSettings;

		protected virtual void OnClosed(object sender, EventArgs e)
		{
			this.OnSavingSettings(EventArgs.Empty);
		}

		protected virtual void OnLoaded(object sender, RoutedEventArgs e)
		{
			Action setLayout = delegate
			                   {
				                   if (!AeroGlassCompositionEnabled)
					                   this.Background = Brushes.Teal;
				                   else
				                   {
					                   this.SetAeroGlassTransparency();
					                   this.InvalidateVisual();
				                   }
			                   };
			this.AeroGlassCompositionChanged += (e1, e2) => setLayout();
			setLayout();
			this.OnLoadingSettings(EventArgs.Empty);
		}

		protected virtual void OnLoadingSettings(EventArgs e)
		{
			this.LoadingSettings.Raise(this, e);
		}

		private void OnSavingSettings(EventArgs e)
		{
			this.SavingSettings.Raise(this, e);
		}
	}
}