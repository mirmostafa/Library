using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Mohammad.Globalization.Helpers;
using Mohammad.Primitives;

namespace Mohammad.Helpers
{
    partial class ApplicationHelper
    {
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

        public static void DecryptSection(string appLocation = null, string sectionName = "appSettings")
        {
            var config = GetConfiguration(appLocation);
            var section = config.GetSection(sectionName);
            if (section == null)
            {
                return;
            }

            if (section.SectionInformation.IsProtected)
            {
                return;
            }

            section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
            config.Save();
        }

        public static void EncryptConfigFile<TProgram>(bool encrypt)
        {
            var config = GetConfiguration<TProgram>();
            var connectionStringsSection = GetConnectionStringSection(config);

            if (encrypt)
            {
                connectionStringsSection.SectionInformation.UnprotectSection();
            }
            else
            {
                connectionStringsSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
            }

            config.Save();
        }

        public static void EncryptSection(string appLocation = null, string sectionName = "appSettings")
        {
            var config = GetConfiguration(appLocation);
            var section = config.GetSection(sectionName);
            if (section == null)
            {
                return;
            }

            if (!section.SectionInformation.IsProtected)
            {
                return;
            }

            section.SectionInformation.UnprotectSection();
            config.Save();
        }

        public static VersionInfo GetAssemblyVersion(string filePath)
        {
            var file = new FileInfo(filePath);
            return ((VersionInfo)Assembly.LoadFile(Path.Combine(file.DirectoryName ?? throw new InvalidOperationException(), file.Name))
                    .ToString()
                    .Split(',')
                    .First(s => s.Trim().StartsWith("Version"))
                    .Substring(Assembly.LoadFile(Path.Combine(file.DirectoryName, file.Name))
                        .ToString()
                        .Split(',')
                        .First(s => s.Trim().StartsWith("Version"))
                        .IndexOf("=", StringComparison.Ordinal) + 1))
                .ToString();
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
        public static NameValueCollection GetConfig() => ConfigurationManager.AppSettings;

        /// <summary>
        ///     Application Configuration (AppConfig)
        /// </summary>
        /// <returns> </returns>
        public static string GetConnectionString<TProgram>(string name)
        {
            var config = GetConfiguration<TProgram>();
            var connectionStringsSection = GetConnectionStringSection(config);
            return connectionStringsSection.ConnectionStrings[name].ConnectionString;
        }

        public static bool IsAssemblyDebugBuild(Assembly assembly)
            => assembly.GetCustomAttributes(false).OfType<DebuggableAttribute>().Select(obj2 => obj2.IsJITTrackingEnabled).FirstOrDefault();

        public static bool IsConfigFileEncrypted<TProgram>() => GetConnectionStringSection(GetConfiguration<TProgram>()).SectionInformation.IsProtected;

        public static bool IsConfigFileEncrypted(string appLocation = null, string sectionName = "appSettings") =>
            GetConnectionStringSection(GetConfiguration(appLocation)).SectionInformation.IsProtected;

        public static bool IsPersian() => Thread.CurrentThread.CurrentCulture.Name.EqualsTo("fa-IR");

        public static void Prepare(string cultureName, bool setPersianCalendar = false)
        {
            var culture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            if (setPersianCalendar)
            {
                PersianCultureHelper.SetPersianOptions(culture);
            }
        }

        /// <summary>
        ///     Application Configuration (AppConfig)
        /// </summary>
        public static void SetConnectionString<TProgram>(string name, string connectionString)
        {
            var config = GetConfiguration<TProgram>();
            var connectionStringsSection = GetConnectionStringSection(config);
            connectionStringsSection.ConnectionStrings[name].ConnectionString = connectionString;
            config.Save();
        }

        private static Configuration GetConfiguration<TProgram>() =>
            ConfigurationManager.OpenExeConfiguration(Assembly.GetAssembly(typeof(TProgram)).Location);

        private static Configuration GetConfiguration(string appLocation = null) => appLocation.IsNullOrEmpty()
            ? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            : ConfigurationManager.OpenExeConfiguration(appLocation);

        private static ConnectionStringsSection GetConnectionStringSection(Configuration config) => (ConnectionStringsSection)config.GetSection("connectionStrings");
    }
}