using System.Configuration;
using Mohammad.Logging.Configurations;

namespace Mohammad.Logging.Configuration
{
    public class LoggingConfigurationHandler : ConfigurationSection
    {
        #region Fields

        #region _Severity

        private static readonly ConfigurationProperty _Severity = new ConfigurationProperty("severity",
            typeof(LoggingSeverity),
            LoggingSeverity.Normal,
            ConfigurationPropertyOptions.None);

        #endregion

        #endregion

        #region Properties

        #region Severity

        [ConfigurationProperty("severity", DefaultValue = LoggingSeverity.High)]
        public LoggingSeverity Severity { get { return (LoggingSeverity) base[_Severity]; } set { base[_Severity] = value; } }

        #endregion

        #endregion

        #region Methods

        #region LoggingConfigurationHandler

        public LoggingConfigurationHandler() { this.Severity = LoggingSeverity.High; }

        #endregion

        #endregion
    }
}