using System;
using System.Security.Authentication;
using System.Web;
using Mohammad.EventsArgs;
using Mohammad.Exceptions;
using Mohammad.Helpers;
using Mohammad.Web.Asp.Security;

namespace Mohammad.Web.Asp.UI
{
    public abstract class LibraryBasePage<TPage, TUserManager> : LibraryBasePage
        where TUserManager : UserManager<TUserManager>, new() where TPage : LibraryBasePage<TPage, TUserManager>
    {
        protected LibraryBasePage() { this.Load += this.LibraryBasePage_OnLoad; }

        private void LibraryBasePage_OnLoad(object sender, EventArgs e)
        {
            if (!IsAuthorizationCheckOk(this))
                return;
            var args = new ActingEventArgs();
            this.OnLoading(args);
            if (args.Handled)
                return;
            this.OnApplyingTemplate();
            if (this.Page.IsPostBack)
            {
                this.LoadState();
                this.OnPageIsPostBack();
            }
            else
            {
                args = new ActingEventArgs();
                this.OnPageIsNotPostBack(args);
                if (args.Handled)
                    return;
                this.DataBind();
            }
        }

        protected virtual void OnApplyingTemplate() { }

        public static bool IsAuthorizationCheckOk(LibraryBasePage<TPage, TUserManager> page)
        {
            if (page == null)
                return true;

            var loginPageUrl = page.GetLoginPageUrl();
            if (loginPageUrl == null)
                throw new NullReferenceException("Please override GetLoginPageUrl");

            var loginUrlToRedirect = loginPageUrl + "?ReturnUrl=";
            if (!UserManager<TUserManager>.IsLoggedIn)
            {
                if (ObjectHelper.GetAttribute<AuthorizeAttribute>(page) == null)
                    return true;
                HttpContext.Current.Response.Redirect(loginUrlToRedirect + HttpContext.Current.Request.Url.AbsolutePath);
                return false;
            }
            if (UserManager<TUserManager>.CurrentUser.IsAdmin)
                return true;
            var authorization = ObjectHelper.GetAttribute<AuthorizeAttribute>(page);
            if (authorization == null || !authorization.IsAdminPrivilegeRequired)
                return true;
            HttpContext.Current.Response.Redirect(loginUrlToRedirect + HttpContext.Current.Request.Url.AbsolutePath);
            return false;
        }

        protected virtual string GetLoginPageUrl() { return null; }

        protected virtual void CheckAdminAuthority(string actionName = "این عملیات")
        {
            if (!UserManager<TUserManager>.CurrentUser?.IsAdmin ?? false)
                throw new AuthenticationException(actionName, new LibraryException($"شما مجوز انجام {actionName} را ندارید"));
        }
    }
}