#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library35.Helpers;

namespace Library35.Serialization
{
	public abstract class ApplicationSettings<TApplicationSettings>
		where TApplicationSettings : ApplicationSettings<TApplicationSettings>, new()
	{
		private static string _Directory = Environment.CurrentDirectory;

		protected static string Prefix { get; set; }

		protected static string Directory
		{
			get { return _Directory; }
			set { _Directory = value; }
		}

		public static string GetSettingsFile()
		{
			return string.Format("{0}\\{1}{2}", Directory, Prefix, "Settings.xml");
		}

		public static TApplicationSettings Load()
		{
			var defaultValue = new TApplicationSettings();
			defaultValue.OnLoading(EventArgs.Empty);
			return SerializationHelper.Load(GetSettingsFile(), defaultValue);
		}

		protected virtual void OnLoading(EventArgs e)
		{
		}

		public void Save()
		{
			SerializationHelper.Save(GetSettingsFile(), this);
			this.OnSaved(EventArgs.Empty);
		}

		protected virtual void OnSaved(EventArgs e)
		{
			this.Saved.Raise(this, e);
		}

		public event EventHandler Saved;
	}
}