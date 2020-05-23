#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.IO;
using System.Reflection;

namespace Library40.Helpers
{
	public static partial class ApplicationHelper
	{
		private const string propertyNameTitle = "Title";
		private const string propertyNameDescription = "Description";
		private const string propertyNameProduct = "Product";
		private const string propertyNameCopyright = "Copyright";
		private const string propertyNameCompany = "Company";
		private static string _Company;

		public static string ProductTitle
		{
			get
			{
				var result = CalculatePropertyValue<AssemblyTitleAttribute>(propertyNameTitle);
				if (string.IsNullOrEmpty(result))
					// otherwise, just get the name of the assembly itself.
					result = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
				return result;
			}
		}

		public static string Version
		{
			get
			{
				// first, try to get the version string from the assembly.
				var version = Assembly.GetExecutingAssembly().GetName().Version;
				return version != null ? version.ToString() : string.Empty;
			}
		}

		public static string Description
		{
			get { return CalculatePropertyValue<AssemblyDescriptionAttribute>(propertyNameDescription); }
		}

		public static string Copyright
		{
			get { return CalculatePropertyValue<AssemblyCopyrightAttribute>(propertyNameCopyright); }
		}

		public static string Company
		{
			get { return _Company ?? CalculatePropertyValue<AssemblyCompanyAttribute>(propertyNameCompany); }
			set { _Company = value; }
		}

		private static string CalculatePropertyValue<T>(string propertyName)
		{
			var result = string.Empty;
			var attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof (T), false);
			if (attributes.Length > 0)
			{
				var attrib = (T)attributes[0];
				var property = attrib.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
				if (property != null)
					result = property.GetValue(attributes[0], null) as string;
			}

			return result;
		}
	}
}