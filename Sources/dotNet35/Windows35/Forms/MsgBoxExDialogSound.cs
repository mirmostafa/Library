#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.IO;
using System.Media;
using System.Security;
using Microsoft.Win32;

namespace Library35.Windows.Forms
{
	public sealed class MsgBoxExDialogSound : ISound
	{
		#region Fields

		#region _default
		private static ISound _default;
		#endregion

		#region _error
		private static ISound _error;
		#endregion

		#region _information
		private static ISound _information;
		#endregion

		#region _question
		private static ISound _question;
		#endregion

		#region _security
		private static ISound _security;
		#endregion

		#region _warning
		private static ISound _warning;
		#endregion

		#region mediaPath
		private static readonly string mediaPath = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\Media\");
		#endregion

		#region name
		private readonly string name;
		#endregion

		#endregion

		#region Properties

		#region Default
		public static ISound Default
		{
			get
			{
				if (_default == null)
					_default = new MsgBoxExDialogSound(".Default");
				return _default;
			}
		}
		#endregion

		#region Error
		public static ISound Error
		{
			get
			{
				if (_error == null)
					_error = new MsgBoxExDialogSound("SystemHand");
				return _error;
			}
		}
		#endregion

		#region Information
		public static ISound Information
		{
			get
			{
				if (_information == null)
					_information = new MsgBoxExDialogSound("SystemAsterisk");
				return _information;
			}
		}
		#endregion

		#region Question
		public static ISound Question
		{
			get
			{
				if (_question == null)
					_question = new MsgBoxExDialogSound("SystemQuestion");
				return _question;
			}
		}
		#endregion

		#region Security
		public static ISound Security
		{
			get
			{
				if (_security == null)
					if (Environment.OSVersion.Version.Major >= 6)
						_security = new MsgBoxExDialogSound("WindowsUAC");
					else
						_security = new MsgBoxExDialogSound("SystemHand");
				return _security;
			}
		}
		#endregion

		#region Warning
		public static ISound Warning
		{
			get
			{
				if (_warning == null)
					_warning = new MsgBoxExDialogSound("SystemExclamation");
				return _warning;
			}
		}
		#endregion

		#endregion

		#region Methods

		#region MsgBoxExDialogSound
		internal MsgBoxExDialogSound(string name)
		{
			this.name = name;
		}
		#endregion

		#region Play
		public void Play()
		{
			try
			{
				var path = Registry.GetValue(@"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\" + this.name + @"\.Current", null, null) as string;
				if (!File.Exists(path) && File.Exists(Path.Combine(mediaPath, path)))
					path = Path.Combine(mediaPath, path);
				using (var player = new SoundPlayer(path))
					player.Play();
			}
			catch (IOException)
			{
			}
			catch (TimeoutException)
			{
			}
			catch (ArgumentException)
			{
			}
			catch (SecurityException)
			{
			}
			catch (InvalidOperationException)
			{
			}
		}
		#endregion

		#endregion
	}
}