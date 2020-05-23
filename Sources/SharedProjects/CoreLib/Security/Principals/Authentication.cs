using System;
using System.Security.Principal;
using Mohammad.Annotations;
using Mohammad.Helpers;
using Mohammad.Win32;
using Mohammad.Win32.Natives;

namespace Mohammad.Security.Principals
{
    public sealed class Authentication : IDisposable
    {
        private IntPtr _Token;
        public string Username { get; set; }
        public string Domain { get; set; }
        public string Password { get; set; }
        public LogonType LogonType { get; set; }
        public LogonProvider LogonProvider { get; set; }
        ~Authentication() { this.Dispose(false); }

        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            lock (this)
            {
                if (this._Token != IntPtr.Zero)
                    Api.CloseHandle(this._Token);
                this._Token = IntPtr.Zero;
            }
        }

        public bool LogonUser() => Api.LogonUser(this.Username, this.Domain, this.Password, this.LogonType.ToInt(), this.LogonProvider.ToInt(), ref this._Token);

        public bool ImpersonateLoggedOnUser() => Api.ImpersonateLoggedOnUser(this._Token);

        public WindowsImpersonationContext Impersonate() => new WindowsIdentity(this._Token).Impersonate();

        public static void Run(string username, string domain, string password, LogonType logonType, LogonProvider logonProvider, Action action)
        {
            using (var result = new Authentication {Username = username, Password = password, Domain = domain, LogonType = logonType, LogonProvider = logonProvider})
            {
                result.LogonUser();
                result.Run(action);
            }
        }

        public static Authentication LogonUser(string username, string domain, string password, LogonType logonType, LogonProvider logonProvider)
        {
            var result = new Authentication {Username = username, Password = password, Domain = domain, LogonType = logonType, LogonProvider = logonProvider};
            result.LogonUser();
            return result;
        }

        public void Run([NotNull] Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            using (this.Impersonate())
                action();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}