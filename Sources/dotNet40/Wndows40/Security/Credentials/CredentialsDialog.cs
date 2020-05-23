#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Library40.Win32.Natives;

namespace Library40.Win.Security.Credentials
{
	/// <summary>
	///     Encapsulates dialog functionality from the Credential Management API.
	/// </summary>
	public sealed class CredentialsDialog
	{
		/// <summary>
		///     The only valid bitmap height (in pixels) of a user-defined banner.
		/// </summary>
		private const int _ValidBannerHeight = 60;

		/// <summary>
		///     The only valid bitmap width (in pixels) of a user-defined banner.
		/// </summary>
		private const int _ValidBannerWidth = 320;

		private Image _Banner;
		private string _Caption = String.Empty;
		private bool _ExcludeCertificates = true;
		private string _Message = String.Empty;
		private string _Name = String.Empty;
		private string _Password = String.Empty;
		private bool _Persist = true;
		private bool _SaveDisplayed = true;
		private string _Target = String.Empty;

		/// <summary>
		///     Initializes a new instance of the <see cref="T:SecureCredentialsLibrary.CredentialsDialog" /> class
		///     with the specified target.
		/// </summary>
		/// <param name="target">The name of the target for the credentials, typically a server name.</param>
		public CredentialsDialog(string target)
			: this(target, null)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="T:SecureCredentialsLibrary.CredentialsDialog" /> class
		///     with the specified target and caption.
		/// </summary>
		/// <param name="target">The name of the target for the credentials, typically a server name.</param>
		/// <param name="caption">The caption of the dialog (null will cause a system default title to be used).</param>
		public CredentialsDialog(string target, string caption)
			: this(target, caption, null)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="T:SecureCredentialsLibrary.CredentialsDialog" /> class
		///     with the specified target, caption and message.
		/// </summary>
		/// <param name="target">The name of the target for the credentials, typically a server name.</param>
		/// <param name="caption">The caption of the dialog (null will cause a system default title to be used).</param>
		/// <param name="message">The message of the dialog (null will cause a system default message to be used).</param>
		public CredentialsDialog(string target, string caption, string message)
			: this(target, caption, message, null)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="T:SecureCredentialsLibrary.CredentialsDialog" /> class
		///     with the specified target, caption, message and banner.
		/// </summary>
		/// <param name="target">The name of the target for the credentials, typically a server name.</param>
		/// <param name="caption">The caption of the dialog (null will cause a system default title to be used).</param>
		/// <param name="message">The message of the dialog (null will cause a system default message to be used).</param>
		/// <param name="banner">The image to display on the dialog (null will cause a system default image to be used).</param>
		public CredentialsDialog(string target, string caption, string message, Image banner)
		{
			this.Target = target;
			this.Caption = caption;
			this.Message = message;
			this.Banner = banner;
		}

		/// <summary>
		///     Gets or sets if the dialog will be shown even if the credentials
		///     can be returned from an existing credential in the credential manager.
		/// </summary>
		public bool AlwaysDisplay { get; set; }

		/// <summary>
		///     Gets or sets if the dialog is populated with name/password only.
		/// </summary>
		public bool ExcludeCertificates
		{
			get { return this._ExcludeCertificates; }
			set { this._ExcludeCertificates = value; }
		}

		/// <summary>
		///     Gets or sets if the credentials are to be persisted in the credential manager.
		/// </summary>
		public bool Persist
		{
			get { return this._Persist; }
			set { this._Persist = value; }
		}

		/// <summary>
		///     Gets or sets if the name is read-only.
		/// </summary>
		public bool KeepName { get; set; }

		/// <summary>
		///     Gets or sets the name for the credentials.
		/// </summary>
		public string Name
		{
			get { return this._Name; }
			set
			{
				if (value != null)
					if (value.Length > CREDUI.MAX_USERNAME_LENGTH)
					{
						var message = String.Format(Thread.CurrentThread.CurrentUICulture, "The name has a maximum length of {0} characters.", CREDUI.MAX_USERNAME_LENGTH);
						throw new ArgumentException(message, "Name");
					}
				this._Name = value;
			}
		}

		/// <summary>
		///     Gets or sets the password for the credentials.
		/// </summary>
		public string Password
		{
			get { return this._Password; }
			set
			{
				if (value != null)
					if (value.Length > CREDUI.MAX_PASSWORD_LENGTH)
					{
						var message = String.Format(Thread.CurrentThread.CurrentUICulture, "The password has a maximum length of {0} characters.", CREDUI.MAX_PASSWORD_LENGTH);
						throw new ArgumentException(message, "Password");
					}
				this._Password = value;
			}
		}

		/// <summary>
		///     Gets or sets if the save checkbox status.
		/// </summary>
		public bool SaveChecked { get; set; }

		/// <summary>
		///     Gets or sets if the save checkbox is displayed.
		/// </summary>
		/// <remarks>
		///     This value only has effect if Persist is true.
		/// </remarks>
		public bool SaveDisplayed
		{
			get { return this._SaveDisplayed; }
			set { this._SaveDisplayed = value; }
		}

		/// <summary>
		///     Gets or sets the name of the target for the credentials, typically a server name.
		/// </summary>
		public string Target
		{
			get { return this._Target; }
			set
			{
				if (value == null)
					throw new ArgumentException("The target cannot be a null value.", "Target");
				if (value.Length > CREDUI.MAX_GENERIC_TARGET_LENGTH)
				{
					var message = String.Format(Thread.CurrentThread.CurrentUICulture, "The target has a maximum length of {0} characters.", CREDUI.MAX_GENERIC_TARGET_LENGTH);
					throw new ArgumentException(message, "Target");
				}
				this._Target = value;
			}
		}

		/// <summary>
		///     Gets or sets the caption of the dialog.
		/// </summary>
		/// <remarks>
		///     A null value will cause a system default caption to be used.
		/// </remarks>
		public string Caption
		{
			get { return this._Caption; }
			set
			{
				if (value != null)
					if (value.Length > CREDUI.MAX_CAPTION_LENGTH)
					{
						var message = String.Format(Thread.CurrentThread.CurrentUICulture, "The caption has a maximum length of {0} characters.", CREDUI.MAX_CAPTION_LENGTH);
						throw new ArgumentException(message, "Caption");
					}
				this._Caption = value;
			}
		}

		/// <summary>
		///     Gets or sets the message of the dialog.
		/// </summary>
		/// <remarks>
		///     A null value will cause a system default message to be used.
		/// </remarks>
		public string Message
		{
			get { return this._Message; }
			set
			{
				if (value != null)
					if (value.Length > CREDUI.MAX_MESSAGE_LENGTH)
					{
						var message = String.Format(Thread.CurrentThread.CurrentUICulture, "The message has a maximum length of {0} characters.", CREDUI.MAX_MESSAGE_LENGTH);
						throw new ArgumentException(message, "Message");
					}
				this._Message = value;
			}
		}

		/// <summary>
		///     Gets or sets the image to display on the dialog.
		/// </summary>
		/// <remarks>
		///     A null value will cause a system default image to be used.
		/// </remarks>
		public Image Banner
		{
			get { return this._Banner; }
			set
			{
				if (value != null)
				{
					if (value.Width != _ValidBannerWidth)
						throw new ArgumentException("The banner image width must be 320 pixels.", "Banner");
					if (value.Height != _ValidBannerHeight)
						throw new ArgumentException("The banner image height must be 60 pixels.", "Banner");
				}
				this._Banner = value;
			}
		}

		/// <summary>
		///     Shows the credentials dialog.
		/// </summary>
		/// <returns>Returns a DialogResult indicating the user action.</returns>
		public DialogResult Show()
		{
			return this.Show(null, this.Name, this.Password, this.SaveChecked);
		}

		/// <summary>
		///     Shows the credentials dialog with the specified save checkbox status.
		/// </summary>
		/// <param name="saveChecked">True if the save checkbox is checked.</param>
		/// <returns>Returns a DialogResult indicating the user action.</returns>
		public DialogResult Show(bool saveChecked)
		{
			return this.Show(null, this.Name, this.Password, saveChecked);
		}

		/// <summary>
		///     Shows the credentials dialog with the specified name.
		/// </summary>
		/// <param name="name">The name for the credentials.</param>
		/// <returns>Returns a DialogResult indicating the user action.</returns>
		public DialogResult Show(string name)
		{
			return this.Show(null, name, this.Password, this.SaveChecked);
		}

		/// <summary>
		///     Shows the credentials dialog with the specified name and password.
		/// </summary>
		/// <param name="name">The name for the credentials.</param>
		/// <param name="password">The password for the credentials.</param>
		/// <returns>Returns a DialogResult indicating the user action.</returns>
		public DialogResult Show(string name, string password)
		{
			return this.Show(null, name, password, this.SaveChecked);
		}

		/// <summary>
		///     Shows the credentials dialog with the specified name, password and save checkbox status.
		/// </summary>
		/// <param name="name">The name for the credentials.</param>
		/// <param name="password">The password for the credentials.</param>
		/// <param name="saveChecked">True if the save checkbox is checked.</param>
		/// <returns>Returns a DialogResult indicating the user action.</returns>
		public DialogResult Show(string name, string password, bool saveChecked)
		{
			return this.Show(null, name, password, saveChecked);
		}

		/// <summary>
		///     Shows the credentials dialog with the specified owner.
		/// </summary>
		/// <param name="owner">The System.Windows.Forms.IWin32Window the dialog will display in front of.</param>
		/// <returns>Returns a DialogResult indicating the user action.</returns>
		public DialogResult Show(IWin32Window owner)
		{
			return this.Show(owner, this.Name, this.Password, this.SaveChecked);
		}

		/// <summary>
		///     Shows the credentials dialog with the specified owner and save checkbox status.
		/// </summary>
		/// <param name="owner">The System.Windows.Forms.IWin32Window the dialog will display in front of.</param>
		/// <param name="saveChecked">True if the save checkbox is checked.</param>
		/// <returns>Returns a DialogResult indicating the user action.</returns>
		public DialogResult Show(IWin32Window owner, bool saveChecked)
		{
			return this.Show(owner, this.Name, this.Password, saveChecked);
		}

		/// <summary>
		///     Shows the credentials dialog with the specified owner, name and password.
		/// </summary>
		/// <param name="owner">The System.Windows.Forms.IWin32Window the dialog will display in front of.</param>
		/// <param name="name">The name for the credentials.</param>
		/// <param name="password">The password for the credentials.</param>
		/// <returns>Returns a DialogResult indicating the user action.</returns>
		public DialogResult Show(IWin32Window owner, string name, string password)
		{
			return this.Show(owner, name, password, this.SaveChecked);
		}

		/// <summary>
		///     Shows the credentials dialog with the specified owner, name, password and save checkbox status.
		/// </summary>
		/// <param name="owner">The System.Windows.Forms.IWin32Window the dialog will display in front of.</param>
		/// <param name="name">The name for the credentials.</param>
		/// <param name="password">The password for the credentials.</param>
		/// <param name="saveChecked">True if the save checkbox is checked.</param>
		/// <returns>Returns a DialogResult indicating the user action.</returns>
		public DialogResult Show(IWin32Window owner, string name, string password, bool saveChecked)
		{
			if (Environment.OSVersion.Version.Major < 5)
				throw new ApplicationException("The Credential Management API requires Windows XP / Windows Server 2003 or later.");
			this.Name = name;
			this.Password = password;
			this.SaveChecked = saveChecked;

			return this.ShowDialog(owner);
		}

		/// <summary>
		///     Confirmation action to be applied.
		/// </summary>
		/// <param name="value">True if the credentials should be persisted.</param>
		public void Confirm(bool value)
		{
			switch (CREDUI.ConfirmCredentials(this.Target, value))
			{
				case CREDUI.ReturnCodes.NO_ERROR:
					break;

				case CREDUI.ReturnCodes.ERROR_INVALID_PARAMETER:
					// for some reason, this is encountered when credentials are overwritten
					break;
				case CREDUI.ReturnCodes.ERROR_NOT_FOUND:
					throw new ApplicationException("Target not found.");
				default:
					throw new ApplicationException("Credential confirmation failed.");
			}
		}

		/// <summary>
		///     Returns a DialogResult indicating the user action.
		/// </summary>
		/// <param name="owner">The System.Windows.Forms.IWin32Window the dialog will display in front of.</param>
		/// <remarks>
		///     Sets the name, password and SaveChecked accessors to the state of the dialog as it was dismissed by the user.
		/// </remarks>
		private DialogResult ShowDialog(IWin32Window owner)
		{
			// set the api call parameters
			var name = new StringBuilder(CREDUI.MAX_USERNAME_LENGTH);
			name.Append(this.Name);

			var password = new StringBuilder(CREDUI.MAX_PASSWORD_LENGTH);
			password.Append(this.Password);

			var saveChecked = Convert.ToInt32(this.SaveChecked);

			var info = this.GetInfo(owner);
			var flags = this.GetFlags();

			// make the api call
			var code = CREDUI.PromptForCredentials(ref info, this.Target, IntPtr.Zero, 0, name, CREDUI.MAX_USERNAME_LENGTH, password, CREDUI.MAX_PASSWORD_LENGTH, ref saveChecked, flags);

			// clean up resources
			if (this.Banner != null)
				Api.DeleteObject(info.hbmBanner);

			// set the accessors from the api call parameters
			this.Name = name.ToString();
			this.Password = password.ToString();
			this.SaveChecked = Convert.ToBoolean(saveChecked);

			return this.GetDialogResult(code);
		}

		/// <summary>
		///     Returns the info structure for dialog display settings.
		/// </summary>
		/// <param name="owner">The System.Windows.Forms.IWin32Window the dialog will display in front of.</param>
		private CREDUI.INFO GetInfo(IWin32Window owner)
		{
			var info = new CREDUI.INFO();
			if (owner != null)
				info.hwndParent = owner.Handle;
			info.pszCaptionText = this.Caption;
			info.pszMessageText = this.Message;
			if (this.Banner != null)
				info.hbmBanner = new Bitmap(this.Banner, _ValidBannerWidth, _ValidBannerHeight).GetHbitmap();
			info.cbSize = Marshal.SizeOf(info);
			return info;
		}

		/// <summary>
		///     Returns the flags for dialog display options.
		/// </summary>
		private CREDUI.FLAGS GetFlags()
		{
			var flags = CREDUI.FLAGS.GENERIC_CREDENTIALS;

			// grrrr... can't seem to get this to work...
			// if (incorrectPassword) flags = flags | CredUI.CREDUI_FLAGS.INCORRECT_PASSWORD;

			if (this.AlwaysDisplay)
				flags = flags | CREDUI.FLAGS.ALWAYS_SHOW_UI;

			if (this.ExcludeCertificates)
				flags = flags | CREDUI.FLAGS.EXCLUDE_CERTIFICATES;

			if (this.Persist)
			{
				flags = flags | CREDUI.FLAGS.EXPECT_CONFIRMATION;
				if (this.SaveDisplayed)
					flags = flags | CREDUI.FLAGS.SHOW_SAVE_CHECK_BOX;
			}
			else
				flags = flags | CREDUI.FLAGS.DO_NOT_PERSIST;

			if (this.KeepName)
				flags = flags | CREDUI.FLAGS.KEEP_USERNAME;

			return flags;
		}

		/// <summary>
		///     Returns a DialogResult from the specified code.
		/// </summary>
		/// <param name="code">The credential return code.</param>
		private DialogResult GetDialogResult(CREDUI.ReturnCodes code)
		{
			DialogResult result;
			switch (code)
			{
				case CREDUI.ReturnCodes.NO_ERROR:
					result = DialogResult.OK;
					break;

				case CREDUI.ReturnCodes.ERROR_CANCELLED:
					result = DialogResult.Cancel;
					break;

				case CREDUI.ReturnCodes.ERROR_NO_SUCH_LOGON_SESSION:
					throw new ApplicationException("No such logon session.");

				case CREDUI.ReturnCodes.ERROR_NOT_FOUND:
					throw new ApplicationException("Not found.");

				case CREDUI.ReturnCodes.ERROR_INVALID_ACCOUNT_NAME:
					throw new ApplicationException("Invalid account name.");

				case CREDUI.ReturnCodes.ERROR_INSUFFICIENT_BUFFER:
					throw new ApplicationException("Insufficient buffer.");

				case CREDUI.ReturnCodes.ERROR_INVALID_PARAMETER:
					throw new ApplicationException("Invalid parameter.");

				case CREDUI.ReturnCodes.ERROR_INVALID_FLAGS:
					throw new ApplicationException("Invalid flags.");

				default:
					throw new ApplicationException("Unknown credential result encountered.");
			}
			return result;
		}
	}
}