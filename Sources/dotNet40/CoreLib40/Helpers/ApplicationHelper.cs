#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using Library40.Globalization.Helpers;

namespace Library40.Helpers
{
	public partial class ApplicationHelper
	{
		public static bool IsPersian()
		{
			return Thread.CurrentThread.CurrentCulture.Name.EqualsTo("fa-IR");
		}

		public static void Prepare(string cultureName, bool setPersianCalendar = false)
		{
			var culture = new CultureInfo(cultureName);
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
			if (setPersianCalendar)
				PersianCultureHelper.SetPersianOptions(culture);
		}

		public static bool CheckCompatibility(string fileFullName)
		{
			try
			{
				Assembly.LoadFile(fileFullName);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		///     Application Configuration (AppConfig)
		/// </summary>
		/// <param name="name"> </param>
		/// <returns> </returns>
		public static string GetConfig(string name)
		{
			try
			{
				return ConfigurationManager.AppSettings[name];
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		///     Application Configuration (AppConfig)
		/// </summary>
		/// <param name="index"> </param>
		/// <returns> </returns>
		public static string GetConfig(int index)
		{
			try
			{
				return ConfigurationManager.AppSettings[index];
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		///     Application Configuration (AppConfig)
		/// </summary>
		/// <returns> </returns>
		public static NameValueCollection GetConfig()
		{
			return ConfigurationManager.AppSettings;
		}

		/// <summary>
		///     Application Configuration (AppConfig)
		/// </summary>
		/// <returns> </returns>
		public static string GetConnectionString<TProgram>(string name)
		{
			var config = GetConfiguration<TProgram>();
			var connectionStringsSection = GetConnectionStringSetction(config);
			return connectionStringsSection.ConnectionStrings[name].ConnectionString;
		}

		/// <summary>
		///     Application Configuration (AppConfig)
		/// </summary>
		public static void SetConnectionString<TProgram>(string name, string connetionString)
		{
			var config = GetConfiguration<TProgram>();
			var connectionStringsSection = GetConnectionStringSetction(config);
			connectionStringsSection.ConnectionStrings[name].ConnectionString = connetionString;
			config.Save();
		}

		private static ConnectionStringsSection GetConnectionStringSetction(Configuration config)
		{
			return (ConnectionStringsSection)config.GetSection("connectionStrings");
		}

		private static Configuration GetConfiguration<TProgram>()
		{
			return ConfigurationManager.OpenExeConfiguration(Assembly.GetAssembly(typeof (TProgram)).Location);
		}

		public static bool IsConfigFileEncrypted<TProgram>()
		{
			return GetConnectionStringSetction(GetConfiguration<TProgram>()).SectionInformation.IsProtected;
		}

		public static void EncryptConfigFile<TProgram>(bool encrypt)
		{
			var config = GetConfiguration<TProgram>();
			var connectionStringsSection = GetConnectionStringSetction(config);

			if (encrypt)
				connectionStringsSection.SectionInformation.UnprotectSection();
			else
				connectionStringsSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
			config.Save();
		}

		public static bool IsAssemblyDebugBuild(Assembly assemb)
		{
			return assemb.GetCustomAttributes(false).OfType<DebuggableAttribute>().Select(obj2 => (obj2).IsJITTrackingEnabled).FirstOrDefault();
		}
	}
}