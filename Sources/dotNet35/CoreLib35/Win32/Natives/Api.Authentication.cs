#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Runtime.InteropServices;

namespace Library35.Win32.Natives
{
	public partial class Api
	{
		/// <summary>
		///     The LogonUser function attempts to log a user on to the local computer.
		///     (http://msdn.microsoft.com/en-us/library/aa378184(VS.85).aspx)
		/// </summary>
		/// <param name="lpszUsername">
		///     A pointer to a null-terminated string that specifies the name of the user. This is the name
		///     of the user account to log on to. If you use the user principal name (UPN) format, User@DNSDomainName, the
		///     lpszDomain parameter must be NULL.
		/// </param>
		/// <param name="lpszDomain">
		///     A pointer to a null-terminated string that specifies the name of the domain or server whose
		///     account database contains the lpszUsername account. If this parameter is NULL, the user name must be specified in
		///     UPN format. If this parameter is ".", the function validates the account by using only the local account database.
		/// </param>
		/// <param name="lpszPassword">
		///     A pointer to a null-terminated string that specifies the plaintext password for the user
		///     account specified by lpszUsername. When you have finished using the password, clear the password from memory by
		///     calling the SecureZeroMemory function. For more information about protecting passwords, see Handling Passwords.
		/// </param>
		/// <param name="dwLogonType"> The type of logon operation to perform. Check out Constants.LOGON32_LOGON_... </param>
		/// <param name="dwLogonProvider"> Specifies the logon provider. Check out Constants.LOGON32_PROVIDER_... </param>
		/// <param name="phToken">
		///     A pointer to a handle variable that receives a handle to a token that represents the specified
		///     user.
		/// </param>
		/// <returns>
		///     If the function succeeds, the function returns nonzero. If the function fails, it returns zero. To get
		///     extended error information, call GetLastError.
		/// </returns>
		[DllImport("advapi32.DLL", SetLastError = true)]
		//public static extern int LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
		public static extern bool LogonUser([In] string lpszUsername,
			[In] string lpszDomain,
			[In] string lpszPassword,
			[In] int dwLogonType,
			[In] int dwLogonProvider,
			[In] [Out] ref IntPtr phToken);

		/// <summary>
		///     The ImpersonateLoggedOnUser function lets the calling thread impersonate the security context of a logged-on user.
		/// </summary>
		/// <param name="hToken">
		///     A handle to a primary or impersonation access token that represents a logged-on user. This can be
		///     a token handle returned by a call to LogonUser, CreateRestrictedToken, DuplicateToken, DuplicateTokenEx,
		///     OpenProcessToken, or OpenThreadToken functions. If hToken is a handle to a primary token, the token must have
		///     TOKEN_QUERY and TOKEN_DUPLICATE access. If hToken is a handle to an impersonation token, the token must have
		///     TOKEN_QUERY and TOKEN_IMPERSONATE access.
		/// </param>
		/// <returns>
		///     If the function succeeds, the return value is nonzero.If the function fails, the return value is zero. To get
		///     extended error information, call GetLastError.
		/// </returns>
		[DllImport("advapi32.DLL", SetLastError = true)]
		public static extern bool ImpersonateLoggedOnUser([In] IntPtr hToken);
	}
}