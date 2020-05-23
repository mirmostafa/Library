#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Configuration;
using Library40.LogSystem.Configurations;

namespace Library40.LogSystem.Configuration
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