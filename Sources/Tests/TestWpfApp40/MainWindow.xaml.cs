#region File Notice
// Created at: 2013/12/24 3:46 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Threading;
using System.Windows;
using Library40.Wpf.Helpers;

namespace TestWpfApp40
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			SplashScreenManager.CurrActionDesc = "Loading... Wait a moment please";
			SplashScreenManager.Show();
			this.InitializeComponent();
		}

		private void LibraryGlassWindow_Loaded(object sender, RoutedEventArgs e)
		{
			Thread.Sleep(5000);
		}
	}
}