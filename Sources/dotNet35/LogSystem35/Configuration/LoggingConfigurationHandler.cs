#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Configuration;
using Library35.LogSystem.Configurations;

namespace Library35.LogSystem.Configuration
{
	public class LoggingConfigurationHandler : ConfigurationSection
	{
		#region Fields

		#region _Severity
		private static readonly ConfigurationProperty _Severity = new ConfigurationProperty("severity",
			typeof (LoggingSeverity),
			LoggingSeverity.Normal,
			ConfigurationPropertyOptions.None);
		#endregion

		#endregion

		#region Properties

		#region Severity
		[ConfigurationProperty("severity", DefaultValue = LoggingSeverity.High)]
		public LoggingSeverity Severity
		{
			get { return (LoggingSeverity)base[_Severity]; }
			set { base[_Severity] = value; }
		}
		#endregion

		#endregion

		#region Methods

		#region LoggingConfigurationHandler
		public LoggingConfigurationHandler()
		{
			this.Severity = LoggingSeverity.High;
		}
		#endregion

		#endregion
	}
}