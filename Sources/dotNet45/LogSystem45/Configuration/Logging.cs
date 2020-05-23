using System.Configuration;
using Mohammad.Logging.Configurations;

namespace Mohammad.Logging.Configuration
{
    public sealed class Logging : ConfigurationSection
    {
        #region Fields

        #region _severity

        private static readonly ConfigurationProperty _severity = new ConfigurationProperty("severity",
            typeof(LoggingSeverity),
            LoggingSeverity.Normal,
            ConfigurationPropertyOptions.None);

        #endregion

        #endregion

        #region Properties

        #region Severity

        [ConfigurationProperty("severity", DefaultValue = LoggingSeverity.Normal)]
        public LoggingSeverity Severity { get { return (LoggingSeverity) base[_severity]; } set { base[_severity] = value; } }

        #endregion

        #endregion
    }
}