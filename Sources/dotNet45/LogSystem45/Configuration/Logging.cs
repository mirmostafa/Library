using System.Configuration;
using Mohammad.Logging.Configurations;

namespace Mohammad.Logging.Configuration
{
    public sealed class Logging : ConfigurationSection
    {
        private static readonly ConfigurationProperty _severity = new ConfigurationProperty("severity",
            typeof(LoggingSeverity),
            LoggingSeverity.Normal,
            ConfigurationPropertyOptions.None);

        [ConfigurationProperty("severity", DefaultValue = LoggingSeverity.Normal)]
        public LoggingSeverity Severity
        {
            get => (LoggingSeverity)base[_severity];
            set => base[_severity] = value;
        }
    }
}