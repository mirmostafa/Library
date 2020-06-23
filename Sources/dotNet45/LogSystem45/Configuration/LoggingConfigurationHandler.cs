using System.Configuration;
using Mohammad.Logging.Configurations;

namespace Mohammad.Logging.Configuration
{
    public class LoggingConfigurationHandler : ConfigurationSection
    {
        private static readonly ConfigurationProperty _Severity = new ConfigurationProperty("severity",
            typeof(LoggingSeverity),
            LoggingSeverity.Normal,
            ConfigurationPropertyOptions.None);

        [ConfigurationProperty("severity", DefaultValue = LoggingSeverity.High)]
        public LoggingSeverity Severity
        {
            get => (LoggingSeverity)base[_Severity];
            set => base[_Severity] = value;
        }

        public LoggingConfigurationHandler() => this.Severity = LoggingSeverity.High;
    }
}