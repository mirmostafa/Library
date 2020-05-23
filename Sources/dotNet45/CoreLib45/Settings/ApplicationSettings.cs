#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using Mohammad.Dynamic;
using Mohammad.Helpers;
using Mohammad.Serialization;

namespace Mohammad.Settings
{
    public class ApplicationSettings<TApplicationSettings> : IAppSettings
        where TApplicationSettings : ApplicationSettings<TApplicationSettings>, new()
    {
        private static string _Prefix;
        private string _SettingsFile;
        private Expando _Windows;

        protected static string Prefix
        {
            get => _Prefix ?? $"{ApplicationHelper.ApplicationTitle}_";
            set => _Prefix = value;
        }

        protected static string Directory { get; set; } = Environment.CurrentDirectory;

        private string SettingsFile
        {
            get => this._SettingsFile ?? GetSettingsFile();
            set => this._SettingsFile = value;
        }

        public Expando Windows
        {
            get => this._Windows ?? (this._Windows = new Expando());
            set => this._Windows = value;
        }

        public void Save(bool encryptAfterSave = false, string password = null)
        {
            if (encryptAfterSave && password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            SerializationHelper.Save(this.SettingsFile, this, encryptAfterSave, password);
            this.OnSaved(EventArgs.Empty);
        }

        public static string GetSettingsFile() => $"{Directory}\\{Prefix}Settings.xml";

        public static TApplicationSettings Load(string settingsFile = null, bool isEncrypted = false, string password = null)
        {
            var defaultValue = new TApplicationSettings();
            defaultValue.OnLoading(EventArgs.Empty);
            var result = SerializationHelper.Load(settingsFile ?? GetSettingsFile(), defaultValue, isEncrypted, password);
            if (!settingsFile.IsNullOrEmpty())
            {
                result.SettingsFile = settingsFile;
            }

            return result;
        }

        protected virtual void OnLoading(EventArgs e)
        {
        }

        protected virtual void OnSaved(EventArgs e)
        {
            this.Saved.Raise(this, e);
        }

        public event EventHandler Saved;
    }

    internal class ApplicationSettings : ApplicationSettings<ApplicationSettings>
    {
    }
}