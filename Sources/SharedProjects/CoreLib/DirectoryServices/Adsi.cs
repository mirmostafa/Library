using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using Mohammad.Helpers;

namespace Mohammad.DirectoryServices
{
    public class Adsi
    {
        private readonly string _Domain;
        private readonly string _Password;
        private readonly string _Username;

        public Adsi(string domain, string username, string password)
        {
            this._Domain = domain;
            this._Username = username;
            this._Password = password;
        }

        public override string ToString() => $"{nameof(this._Domain)}: {this._Domain}, {nameof(this._Username)}: {this._Username}";

        private static IEnumerable<string> MakeUserFriendly(IEnumerable<string> strings) => strings.Select(computerName => computerName.Remove("CN=").Remove("OU="));

        public IEnumerable<string> GetComputerNames() => MakeUserFriendly(this.FindAll("(objectClass=computer)").Select(de => de.Name));

        public IEnumerable<string> GetUserNames() => MakeUserFriendly(this.FindAll("(&(objectClass=user)(objectCategory=person))").Select(de => de.Name));
        public IEnumerable<string> GetOrganizationalUnits() => MakeUserFriendly(this.FindAll("(objectClass=organizationalUnit)").Select(de => de.Name));

        private IEnumerable<DirectoryEntry> FindAll(string filter, Action<DirectorySearcher> initializeSearcher = null)
        {
            using (var entry = new DirectoryEntry($"LDAP://{this._Domain}", this._Username, this._Password))
            using (var mySearcher = new DirectorySearcher(entry) {Filter = filter, SizeLimit = int.MaxValue, PageSize = int.MaxValue})
            {
                initializeSearcher?.Invoke(mySearcher);
                foreach (SearchResult resEnt in mySearcher.FindAll())
                    yield return resEnt.GetDirectoryEntry();
            }
        }
    }
}