﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using Mohammad.Settings;

namespace Mohammad.Wpf.Windows.Settings
{
    public abstract class AppSettings<TAppSettings> : ApplicationSettings<TAppSettings>, INotifyPropertyChanged
        where TAppSettings : ApplicationSettings<TAppSettings>, new()
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}