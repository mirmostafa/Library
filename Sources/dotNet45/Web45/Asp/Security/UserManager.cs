#region Code Identifications

// Created on     2017/07/29
// Last update on 2018/01/20 by Mohammad Mir mostafa 

#endregion

using System.Web.Security;
using Mohammad.Web.Asp.Helpers;

namespace Mohammad.Web.Asp.Security {
    public abstract class UserManager<TUserManager, TUserData>
        where TUserManager : UserManager<TUserManager, TUserData>, new() where TUserData : UserData
    {
        private static readonly TUserManager _Instance = new TUserManager();
        public static UserData CurrentUser { get => Vars.Session.CurrentUser; private set => Vars.Session.CurrentUser = value; }
        public static bool IsLoggedIn => CurrentUser != null && FormsAuthenticationHelper.IsCurrentUserNameAuthenticated;
        protected abstract TUserData OnSigningIn(string userId, string password, bool rememberMe);
        protected virtual void OnSigningOut() { }

        public static void SignIn(string userId, string password, bool rememberMe)
        {
            CurrentUser = _Instance.OnSigningIn(userId, password, rememberMe);
            FormsAuthenticationHelper.LogInUser(userId, rememberMe);
            FormsAuthentication.RedirectFromLoginPage(userId, rememberMe);
        }

        public static void SignOut()
        {
            _Instance.OnSigningOut();
            CurrentUser = null;
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }

    public abstract class UserManager<TUserManager> : UserManager<TUserManager, UserData>
        where TUserManager : UserManager<TUserManager, UserData>, new() { }
}