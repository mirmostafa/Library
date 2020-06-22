using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using Mohammad.Helpers;
using Mohammad.Wmi.Internals;

namespace Mohammad.Wmi
{
    public class WmiTools
    {
        private readonly ManagementScope _Scope;

        private const string QUERY_SELECT_FROM = "SELECT * FROM ";
        public bool IsLocal { get; }
        public string Ip { get; }
        public string Username { get; }

        private WmiTools()
        {
            this.IsLocal = true;
            this._Scope = ConnectScope();
        }

        private WmiTools(string ip, string username, string password, string domain)
        {
            this.IsLocal = false;
            this.Ip = ip;
            this.Username = username;
            this._Scope = ConnectScope(ip, username, password, domain);
        }

        public static WmiTools GetInstance() => new WmiTools();
        public static WmiTools GetInstance(string ip, string username, string password, string domain) => new WmiTools(ip, username, password, domain);
        public IEnumerable<TWmiClass> GetQuery<TWmiClass>() where TWmiClass : WmiBase, new() => Query<TWmiClass>(this._Scope);
        public static IEnumerable<TWmiQueryClass> GetQueryLocal<TWmiQueryClass>() where TWmiQueryClass : WmiBase, new() => Query<TWmiQueryClass>(ConnectScope());

        public static IEnumerable<TWmiQueryClass> GetQueryRemote<TWmiQueryClass>(string ip, string username, string password, string domain)
            where TWmiQueryClass : WmiBase, new() => Query<TWmiQueryClass>(ConnectScope(ip, username, password, domain));

        private static ManagementScope ConnectScope(string ip, string username, string password, string domain)
        {
            var connection = new ConnectionOptions {Username = username, Password = password, Authority = $"ntlmdomain:{domain}"};
            var result = new ManagementScope($@"\\{ip}\root\CIMV2", connection);
            result.Connect();
            return result;
        }

        private static ManagementScope ConnectScope()
        {
            var result = new ManagementScope("root\\CIMV2");
            result.Connect();
            return result;
        }

        private static IEnumerable<TWmiQueryClass> Query<TWmiQueryClass>(ManagementScope scope) where TWmiQueryClass : WmiBase, new()
            => Query<TWmiQueryClass>(scope, GetWmiClassName<TWmiQueryClass>(), GetProperties<TWmiQueryClass>());

        private static IEnumerable<TWmiQueryClass> Query<TWmiQueryClass>(ManagementScope scope, string wmiClassName, IEnumerable<KeyValuePair<string, string>> dic)
            where TWmiQueryClass : WmiBase, new()
        {
            var query = new ObjectQuery($"{QUERY_SELECT_FROM}{wmiClassName}");
            using (var searcher = new ManagementObjectSearcher(scope, query))
            {
                var props = dic as IList<KeyValuePair<string, string>> ?? dic.ToList();
                foreach (var o in searcher.Get())
                {
                    var queryObj = (ManagementObject) o;
                    var result = new TWmiQueryClass();
                    foreach (var prop in props)
                        ObjectHelper.SetProperty(result, prop.Key, queryObj[prop.Value]);
                    yield return result;
                }
            }
        }

        private static IEnumerable<KeyValuePair<string, string>> GetProperties<TWmiQueryClass>() where TWmiQueryClass : WmiBase, new()
            =>
                typeof(TWmiQueryClass).GetProperties()
                    .Where(p => ObjectHelper.GetAttribute<WmiPropAttribute>(p) != null)
                    .Select(p => new KeyValuePair<string, string>(p.Name, ObjectHelper.GetAttribute<WmiPropAttribute>(p).Name ?? p.Name));

        private static string GetWmiClassName<TWmiQueryClass>() where TWmiQueryClass : WmiBase, new()
            => ObjectHelper.GetAttribute<WmiClassAttribute>(typeof(TWmiQueryClass))?.ClassName ?? typeof(TWmiQueryClass).Name;

        public static DateTime ToDateTime(string dateString) => ManagementDateTimeConverter.ToDateTime(dateString);

        public override string ToString() => this.IsLocal ? "Local Machine" : $"{nameof(this.Ip)}: {this.Ip}, {nameof(this.Username)}: {this.Username}";
    }
}