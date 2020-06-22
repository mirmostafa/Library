using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;

namespace Mohammad.Security.Principals
{
    public class ImpersonationContext
    {
        private readonly string _Domain;
        private readonly string _Password;
        private readonly string _Username;
        private WindowsImpersonationContext _Context;
        private IntPtr _Token;
        private const int LOGON32_PROVIDER_DEFAULT = 0;
        private const int LOGON32_LOGON_INTERACTIVE = 2;
        protected bool IsInContext => this._Context != null;

        public ImpersonationContext(string domain, string username, string password)
        {
            if (domain == null)
                throw new ArgumentNullException(nameof(domain));
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            this._Domain = domain;
            this._Username = username;
            this._Password = password;
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(IntPtr handle);

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Enter()
        {
            if (this.IsInContext)
                return;
            this._Token = new IntPtr(0);
            this._Token = IntPtr.Zero;
            var logonSuccessfull = LogonUser(this._Username, this._Domain, this._Password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref this._Token);
            if (logonSuccessfull == false)
            {
                var error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
            }
            var identity = new WindowsIdentity(this._Token);
            this._Context = identity.Impersonate();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Leave()
        {
            if (this.IsInContext == false)
                return;
            this._Context.Undo();

            if (this._Token != IntPtr.Zero)
                CloseHandle(this._Token);
            this._Context = null;
        }

        public static void Run(string domain, string username, string password, Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var context = new ImpersonationContext(domain, username, password);
            context.Enter();
            try
            {
                action();
            }
            finally
            {
                context.Leave();
            }
        }

        public void Run(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            this.Enter();
            action();
            this.Leave();
        }
    }
}