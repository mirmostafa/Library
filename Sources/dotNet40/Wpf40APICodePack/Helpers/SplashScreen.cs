#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Library40.Wpf.Helpers
{
	public class SplashScreenManager
	{
		private static SplashScreen _SplashScreen;
		private static Task _SplashTask;

		public SplashScreenManager(object t = null)
		{
		}

		public static string CurrActionDesc
		{
			get { return _SplashScreen.CurrActionDesc; }
			set { } //_SplashScreen.CurrActionDesc = value; }
		}

		public void Show(bool b = true)
		{
			Show();
		}

		public static void Show()
		{
			_SplashTask = Task.Factory.StartNew(() =>
			                                    {
				                                    _SplashScreen = new SplashScreen
				                                                    {
					                                                    //Owner = Application.Current.MainWindow
					                                                    Topmost = true
				                                                    };
				                                    _SplashScreen.Show();
				                                    //Application.Current.MainWindow.Loaded += delegate { Close(); };
				                                    return;
				                                    Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Loaded,
					                                    (DispatcherOperationCallback)delegate(object splashObj)
					                                                                 {
						                                                                 var splashScreen = (SplashScreen)splashObj;
						                                                                 Thread.Sleep(TimeSpan.FromSeconds(5));
						                                                                 splashScreen.Close();
						                                                                 return null;
					                                                                 },
					                                    _SplashScreen);
			                                    },
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.FromCurrentSynchronizationContext());
		}

		public static void Close()
		{
			_SplashScreen.Close();
		}
	}
}