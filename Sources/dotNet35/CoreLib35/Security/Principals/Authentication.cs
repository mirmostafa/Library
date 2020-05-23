#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Security.Principal;
using Library35.Helpers;
using Library35.Win32;
using Library35.Win32.Natives;

namespace Library35.Security.Principals
{
	public class Authentication : IDisposable
	{
		private IntPtr _Token;

		public string Username { get; set; }

		public string Domain { get; set; }

		public string Password { get; set; }

		public LogonType LogonType { get; set; }

		public LogonProvider LogonProvider { get; set; }

		#region IDisposable Members
		/// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
				lock (this)
				{
					if (this._Token != IntPtr.Zero)
						Api.CloseHandle(this._Token);
					this._Token = IntPtr.Zero;
				}
		}

		public bool LogonUser()
		{
			return Api.LogonUser(this.Username, this.Domain, this.Password, this.LogonType.ToInt(), this.LogonProvider.ToInt(), ref this._Token);
		}

		public bool ImpersonateLoggedOnUser()
		{
			return Api.ImpersonateLoggedOnUser(this._Token);
		}

		public WindowsImpersonationContext Impersonate()
		{
			var identity = new WindowsIdentity(this._Token);
			return identity.Impersonate();
		}
	}
}