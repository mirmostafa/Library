using System.Data.SqlClient;

namespace Data.SqlServer
{
    public static class ConnectionStringBuilder
    {
        public static string Build(string dataSource, string userId = null, string password = null, string initialCatalog = null, bool? integratedSecurity = null,
            bool? persistSecurityInfo = null, int? connectTimeout = 30, string attachDbFilename = null, string applicationName = null,
            bool? multipleActiveResultSets = null, bool? encrypt = null, bool? userInstance = null)
        {
            var builder = new SqlConnectionStringBuilder();
            if (applicationName != null)
                builder.ApplicationName = applicationName;
            if (attachDbFilename != null)
                builder.AttachDBFilename = attachDbFilename;
            if (connectTimeout != null)
                builder.ConnectTimeout = connectTimeout.Value;
            if (dataSource != null)
                builder.DataSource = dataSource;
            if (encrypt != null)
                builder.Encrypt = encrypt.Value;
            if (initialCatalog != null)
                builder.InitialCatalog = initialCatalog;
            if (integratedSecurity != null)
                builder.IntegratedSecurity = integratedSecurity.Value;
            if (multipleActiveResultSets != null)
                builder.MultipleActiveResultSets = multipleActiveResultSets.Value;
            if (password != null)
                builder.Password = password;
            if (persistSecurityInfo != null)
                builder.PersistSecurityInfo = persistSecurityInfo.Value;
            if (userId != null)
                builder.UserID = userId;
            if (userInstance != null)
                builder.UserInstance = userInstance.Value;
            return builder.ConnectionString;
        }
    }
}