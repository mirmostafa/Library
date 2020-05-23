#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Configuration;
using Library35.LogSystem.Configurations;

namespace Library35.LogSystem.Configuration
{
	public sealed class Logging : ConfigurationSection
	{
		#region Fields

		#region _severity
		private static readonly ConfigurationProperty _severity = new ConfigurationProperty("severity",
			typeof (LoggingSeverity),
			LoggingSeverity.Normal,
			ConfigurationPropertyOptions.None);
		#endregion

		#endregion

		#region Properties

		#region Severity
		[ConfigurationProperty("severity", DefaultValue = LoggingSeverity.Normal)]
		public LoggingSeverity Severity
		{
			get { return (LoggingSeverity)base[_severity]; }
			set { base[_severity] = value; }
		}
		#endregion

		#endregion
	}
}