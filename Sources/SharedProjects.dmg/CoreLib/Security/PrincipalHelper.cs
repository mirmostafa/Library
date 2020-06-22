using System.DirectoryServices.AccountManagement;
using Mohammad.Helpers;

namespace Mohammad.Security
{
    public class PrincipalHelper
    {
        //public static bool IsValidDomainUser(string domain, string userName, string password)
        //{
        //    using (var context = new PrincipalContext(ContextType.Domain, domain))
        //        return context.ValidateCredentials(userName, password);
        //}

        public static bool IsValidDomainUser(string domain, string userName, string password)
            => new PrincipalContext(ContextType.Domain, domain).Dispose(context => context.ValidateCredentials(userName, password));
    }
}