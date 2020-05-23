#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Library35.EventsArgs;
using Library35.Exceptions;
using Library35.Globalization.Helpers;
using Library35.Helpers;
using Library35.Windows.Forms;

namespace Library35.Windows.Helpers
{
	/// <summary>
	///     A utility class to do some common tasks about an application
	/// </summary>
	public static class WindowsApplicationHelper
	{
		private static readonly Mutex _Mutex = new Mutex(false, Application.ProductName);

		public static string AssemblyName
		{
			get { return Assembly.GetCallingAssembly().FullName.Substring(0, Assembly.GetCallingAssembly().FullName.IndexOf(',')); }
		}

		/// <summary>
		///     Gets current application company
		/// </summary>
		/// <value></value>
		public static string CompanyName
		{
			get { return GetProp<AssemblyCompanyAttribute>("Company"); }
		}

		/// <summary>
		///     Gets current application version
		/// </summary>
		/// <value></value>
		public static string Version
		{
			get
			{
				//return GetProp<AssemblyVersionAttribute>("Version");
				return Application.ProductVersion;
			}
		}

		/// <summary>
		///     Gets current application title
		/// </summary>
		/// <value></value>
		public static string Title
		{
			get { return GetProp<AssemblyTitleAttribute>("Title"); }
		}

		/// <summary>
		///     Gets current application Guid
		/// </summary>
		/// <value></value>
		public static string Guid
		{
			get { return GetProp<GuidAttribute>("Value"); }
		}

		/// <summary>
		///     Gets current application product name
		/// </summary>
		/// <value></value>
		public static string ProductName
		{
			get { return GetProp<AssemblyProductAttribute>("Product"); }
		}

		public static string Description
		{
			get { return GetProp<AssemblyDescriptionAttribute>("Description"); }
		}

		public static void PrepareApplication(string cultureName = "fa-IR",
			EventHandler<ExceptionOccurredEventArgs<Exception>> exceptionHandler = null,
			bool withDefaultEventHandle = false,
			bool setPersianCalendar = false,
			bool registerMe = false)
		{
			if (registerMe)
				AppReg.RegisterMe();
			var culture = new CultureInfo(cultureName);
			Application.CurrentCulture = culture;
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
			if (setPersianCalendar)
				PersianCultureHelper.SetPersianOptions(culture);
			if (exceptionHandler == null)
				if (!withDefaultEventHandle)
					return;
			exceptionHandler = delegate(object sender, ExceptionOccurredEventArgs<Exception> e)
			                   {
				                   var ex = e.Exception;
				                   while (ex.InnerException != null)
					                   ex = ex.InnerException;
				                   MsgBox.Error(ex.Message);
				                   if (!(ex is CompanyException))
					                   Application.Exit();
			                   };
			Application.ThreadException += (sender, e) => exceptionHandler.Raise(sender, new ExceptionOccurredEventArgs<Exception>(e.Exception));

			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, true);

			AppDomain.CurrentDomain.UnhandledException += (sender, e) => exceptionHandler.Raise(sender, new ExceptionOccurredEventArgs<Exception>(e.ExceptionObject as Exception));
		}

		public static bool AmIAlone()
		{
			return _Mutex.WaitOne(TimeSpan.FromSeconds(5), false);
		}

		/// <summary>
		/// </summary>
		/// <typeparam name="TAttribute"></typeparam>
		/// <returns></returns>
		private static TAttribute GetAtt<TAttribute>() where TAttribute : Attribute
		{
			var assembly = Assembly.GetEntryAssembly();
			if (!Attribute.IsDefined(assembly, typeof (TAttribute)))
				return null;
			return (TAttribute)assembly.GetCustomAttributes(typeof (TAttribute), false)[0];
		}

		/// <summary>
		/// </summary>
		/// <param name="propName"></param>
		/// <typeparam name="TAttribute"></typeparam>
		/// <returns></returns>
		private static string GetProp<TAttribute>(string propName) where TAttribute : Attribute
		{
			var att = GetAtt<TAttribute>();
			return att == null ? String.Empty : att.GetType().GetProperty(propName).GetValue(att, null).ToString();
		}
	}
}