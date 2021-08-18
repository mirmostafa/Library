﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfLibrary.Markers;

namespace WpfLibrary.Bases
{
    [ViewModel]
    public abstract class ViewModelBase : IViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}