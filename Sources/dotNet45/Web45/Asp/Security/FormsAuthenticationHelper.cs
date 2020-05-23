using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Mohammad.Helpers;

namespace Mohammad.Web.Asp.Security
{
    public static class FormsAuthenticationHelper
    {
        public static string CurrentUserName => HttpContext.Current?.User?.Identity?.Name;
        public static bool IsCurrentUserNameAuthenticated => HttpContext.Current?.User?.Identity?.IsAuthenticated ?? false;
        public static bool IsCurrentUserNameAdmin => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        public static void LogInUser(string username, bool isPersistent, DateTime issueDate = default, DateTime expiration = default)
        {
            if (issueDate == default)
            {
                issueDate = DateTime.Now;
            }

            if (expiration == default)
            {
                expiration = issueDate.AddMinutes(30);
            }

            var ticket = new FormsAuthenticationTicket(1, username, issueDate, expiration, isPersistent, username, FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)) {Path = FormsAuthentication.FormsCookiePath};
            isPersistent.IfTrue(() => cookie.Expires = ticket.Expiration);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}