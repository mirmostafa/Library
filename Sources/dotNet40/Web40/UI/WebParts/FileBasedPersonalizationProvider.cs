#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace Library40.Web.UI.WebParts
{
	/// <summary>
	///     Add the following code in-place in web.config
	///     <system.web>
	///         <webParts>
	///             <personalization defaultProvider="FileBasedPersonalizationProvider">
	///                 <providers>
	///                     <clear />
	///                     <add name="FileBasedPersonalizationProvider"
	///                         type="WebApplication11.FileBasedPersonalizationProvider" />
	///                 </providers>
	///             </personalization>
	///         </webParts>
	///         ...
	///     </system.web>
	/// </summary>
	public class FileBasedPersonalizationProvider : PersonalizationProvider
	{
		private static string _PersonalizationFolder = "~/App_Data/Personalization";

		/// <summary>
		///     When overridden in a derived class, gets or sets the name of the application configured for the provider.
		/// </summary>
		/// <value></value>
		/// <returns>The application configured for the personalization provider.</returns>
		public override string ApplicationName { get; set; }

		/// <summary>
		///     Gets or sets the application data folder.
		/// </summary>
		/// <value>The application data folder.</value>
		public static string PersonalizationFolder
		{
			get { return _PersonalizationFolder; }
			set { _PersonalizationFolder = value; }
		}

		/// <summary>
		///     When overridden in a derived class, returns a collection containing zero or more
		///     <see
		///         cref="T:System.Web.UI.WebControls.WebParts.PersonalizationStateInfo" />
		///     -derived objects based on scope and specific query parameters.
		/// </summary>
		/// <param name="scope">
		///     A <see cref="T:System.Web.UI.WebControls.WebParts.PersonalizationScope" /> with the personalization information to
		///     be queried. This value cannot be null.
		/// </param>
		/// <param name="query">
		///     A <see cref="T:System.Web.UI.WebControls.WebParts.PersonalizationStateQuery" /> containing a query. This value can
		///     be null.
		/// </param>
		/// <param name="pageIndex">The location where the query starts.</param>
		/// <param name="pageSize">The number of records to return.</param>
		/// <param name="totalRecords">The total number of records available.</param>
		/// <returns>
		///     A <see cref="T:System.Web.UI.WebControls.WebParts.PersonalizationStateInfoCollection" /> containing zero or more
		///     <see
		///         cref="T:System.Web.UI.WebControls.WebParts.PersonalizationStateInfo" />
		///     -derived objects.
		/// </returns>
		public override PersonalizationStateInfoCollection FindState(PersonalizationScope scope, PersonalizationStateQuery query, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		///     When overridden in a derived class, returns the number of rows in the underlying data store that exist within the
		///     specified scope.
		/// </summary>
		/// <param name="scope">
		///     A <see cref="T:System.Web.UI.WebControls.WebParts.PersonalizationScope" /> of the personalization information to be
		///     queried. This value cannot be null.
		/// </param>
		/// <param name="query">
		///     A <see cref="T:System.Web.UI.WebControls.WebParts.PersonalizationStateQuery" /> containing a query. This value can
		///     be null.
		/// </param>
		/// <returns>
		///     The number of rows in the underlying data store that exist for the specified <paramref name="scope" /> parameter.
		/// </returns>
		public override int GetCountOfState(PersonalizationScope scope, PersonalizationStateQuery query)
		{
			return 0;
		}

		/// <summary>
		///     When overridden in a derived class, loads raw personalization data from the underlying data store.
		/// </summary>
		/// <param name="webPartManager">
		///     The <see cref="T:System.Web.UI.WebControls.WebParts.WebPartManager" /> managing the personalization data.
		/// </param>
		/// <param name="path">The path for personalization information to be used as the retrieval key.</param>
		/// <param name="userName">The user name for personalization information to be used as the retrieval key.</param>
		/// <param name="sharedDataBlob">
		///     The returned data for the
		///     <see
		///         cref="F:System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared" />
		///     scope.
		/// </param>
		/// <param name="userDataBlob">
		///     The returned data for the
		///     <see
		///         cref="F:System.Web.UI.WebControls.WebParts.PersonalizationScope.User" />
		///     scope.
		/// </param>
		protected override void LoadPersonalizationBlobs(WebPartManager webPartManager, string path, string userName, ref byte[] sharedDataBlob, ref byte[] userDataBlob)
		{
			sharedDataBlob = null;
			StreamReader reader = null;
			try
			{
				var filePath = GetPath(null, path);
				if (File.Exists(filePath))
				{
					reader = new StreamReader(filePath);
					sharedDataBlob = Convert.FromBase64String(reader.ReadLine());
				}
			}
			catch (FileNotFoundException)
			{
				//Do Nothing...
			}
			finally
			{
				if ((reader != null))
					reader.Close();
			}

			if (!string.IsNullOrEmpty(userName))
			{
				reader = null;
				userDataBlob = null;
				try
				{
					var filePath = GetPath(userName, path);
					if (File.Exists(filePath))
					{
						reader = new StreamReader(filePath);
						userDataBlob = Convert.FromBase64String(reader.ReadLine());
					}
				}
				catch (FileNotFoundException)
				{
					//Do Nothing...
				}
				finally
				{
					if ((reader != null))
						reader.Close();
				}
			}
		}

		/// <summary>
		///     Gets the path.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		private static string GetPath(string userName, string path)
		{
			var sha = new SHA256Managed();
			var encoding = new UnicodeEncoding();
			var computeHash = sha.ComputeHash(encoding.GetBytes(path));
			var hash = Convert.ToBase64String(computeHash).Replace('/', '_').Replace('\\', '_').Replace("=", "").ToUpper();

			const string invalidCharacters = "[]{}<>/\\?,.;':`-=~!@#$%^&*()_+";
			foreach (var c in invalidCharacters)
				hash.Replace(c.ToString(), "");

			var returnPath =
				HttpContext.Current.Server.MapPath(string.IsNullOrEmpty(userName)
					? string.Format("{0}/{1}.Personalization.bin", PersonalizationFolder, hash)
					: string.Format("{0}/{1}.{2}.Personalization.bin", PersonalizationFolder, userName.Replace('\\', '_'), hash));
			if (!new FileInfo(returnPath).Directory.Exists)
				new FileInfo(returnPath).Directory.Create();
			return returnPath;
		}

		/// <summary>
		///     When overridden in a derived class, deletes raw personalization data from the underlying data store.
		/// </summary>
		/// <param name="webPartManager">
		///     The <see cref="T:System.Web.UI.WebControls.WebParts.WebPartManager" /> managing the personalization data.
		/// </param>
		/// <param name="path">The path for personalization information to be used as the data store key.</param>
		/// <param name="userName">The user name for personalization information to be used as the data store key.</param>
		protected override void ResetPersonalizationBlob(WebPartManager webPartManager, string path, string userName)
		{
			// Delete the specified personalization file
			try
			{
				File.Delete(GetPath(userName, path));
			}
			catch (FileNotFoundException)
			{
			}
		}

		/// <summary>
		///     When overridden in a derived class, deletes personalization state from the underlying data store based on the
		///     specified parameters.
		/// </summary>
		/// <param name="scope">
		///     A <see cref="T:System.Web.UI.WebControls.WebParts.PersonalizationScope" /> of the personalization information to be
		///     reset. This value cannot be null.
		/// </param>
		/// <param name="paths">The paths for personalization information to be deleted.</param>
		/// <param name="usernames">The user names for personalization information to be deleted.</param>
		/// <returns>The number of rows deleted.</returns>
		public override int ResetState(PersonalizationScope scope, string[] paths, string[] usernames)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		///     When overridden in a derived class, deletes Web Parts personalization data from the underlying data store based on
		///     the specified parameters.
		/// </summary>
		/// <param name="path">
		///     The path of the personalization data to be deleted. This value can be null but cannot be an empty
		///     string ("").
		/// </param>
		/// <param name="userInactiveSinceDate">The date indicating the last time a Web site user changed personalization data.</param>
		/// <returns>
		///     The number of rows deleted from the underlying data store.
		/// </returns>
		public override int ResetUserState(string path, DateTime userInactiveSinceDate)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		///     When overridden in a derived class, saves raw personalization data to the underlying data store.
		/// </summary>
		/// <param name="webPartManager">
		///     The <see cref="T:System.Web.UI.WebControls.WebParts.WebPartManager" /> managing the personalization data.
		/// </param>
		/// <param name="path">The path for personalization information to be used as the data store key.</param>
		/// <param name="userName">The user name for personalization information to be used as the key.</param>
		/// <param name="dataBlob">The byte array of data to be saved.</param>
		protected override void SavePersonalizationBlob(WebPartManager webPartManager, string path, string userName, byte[] dataBlob)
		{
			StreamWriter writer = null;
			try
			{
				writer = new StreamWriter(GetPath(userName, path), false);
				writer.WriteLine(Convert.ToBase64String(dataBlob));
			}
			finally
			{
				if ((writer != null))
					writer.Close();
			}
		}

		/// <summary>
		///     Sets the authorization.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="webPartManager">The web part manager1.</param>
		/// <param name="cookieName">Name of the cookie.</param>
		public static void SetAuthorization(Page page, WebPartManager webPartManager, string cookieName = "WebParts")
		{
			if (!page.IsPostBack)
			{
				var authCookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
				if (authCookie == null)
				{
					var cookie = HttpContext.Current.Request.Cookies.Get(cookieName);
					string userId;
					if (cookie == null)
					{
						userId = Guid.NewGuid().ToString().Replace("-", "");
						cookie = new HttpCookie(cookieName, userId)
						         {
							         Expires = DateTime.Now.AddYears(10)
						         };
						HttpContext.Current.Response.Cookies.Add(cookie);
					}
					else
						userId = cookie.Value;
					var authTicket = new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddSeconds(30), false, "roles");
					HttpContext.Current.Session["Expired"] = authTicket.Expiration.ToString();
					var encryptedTicket = FormsAuthentication.Encrypt(authTicket);
					authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
					HttpContext.Current.Response.Cookies.Add(authCookie);
					HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString());
				}
				webPartManager.DisplayMode = WebPartManager.DesignDisplayMode;
			}

			webPartManager.DisplayMode = WebPartManager.DesignDisplayMode;
		}
	}
}