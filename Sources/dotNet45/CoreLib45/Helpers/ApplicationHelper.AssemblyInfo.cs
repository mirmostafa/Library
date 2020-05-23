#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace Mohammad.Helpers
{
    public static partial class ApplicationHelper
    {
        #region Fields

        private static string _Company;

        #endregion

        public static string ApplicationTitle => CalculatePropertyValue<AssemblyTitleAttribute>("Title");

        public static string Company
        {
            get => _Company ?? CalculatePropertyValue<AssemblyCompanyAttribute>("Company");
            set => _Company = value;
        }

        public static string Copyright => CalculatePropertyValue<AssemblyCopyrightAttribute>("Copyright");

        public static string Description  => CalculatePropertyValue<AssemblyDescriptionAttribute>("Description");
        public static string Guid         => CalculatePropertyValue<GuidAttribute>("Value");
        public static string ProductTitle => CalculatePropertyValue<AssemblyProductAttribute>("Product");

        public static string Version
        {
            get
            {
                // first, try to get the version string from the assembly.
                var version = Assembly.GetEntryAssembly().GetName().Version;
                return version?.ToString() ?? string.Empty;
            }
        }

        private static string CalculatePropertyValue<T>(string propertyName)
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
                return string.Empty;
            var attributes = assembly.GetCustomAttributes(typeof(T), false);
            if (attributes.Length <= 0)
                return string.Empty;
            var attrib   = (T) attributes[0];
            var property = attrib.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            return property != null ? property.GetValue(attributes[0], null) as string : string.Empty;
        }

        public static string GetAboutAppString(string moreInfo = null)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Product:    \t{ProductTitle}");
            builder.AppendLine($"Application:\t{ApplicationTitle}");
            builder.AppendLine($"Version:    \t{Version}");
            builder.AppendLine(Description);
            builder.AppendLine();
            if (!moreInfo.IsNullOrEmpty())
            {
                builder.AppendLine(moreInfo);
                builder.AppendLine();
            }

            builder.AppendLine(Copyright);
            return builder.ToString();
        }

        public static void StartMeUpWithWindows(string appName, RegistryKey root = null)
        {
            using (var registryKey = (root ?? Registry.CurrentUser).OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                registryKey?.SetValue(appName, Environment.CommandLine.Remove("/crashed"));
                registryKey?.Flush();
            }
        }
    }
}