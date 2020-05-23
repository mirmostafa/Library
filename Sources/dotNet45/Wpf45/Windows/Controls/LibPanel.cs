using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Mohammad.Wpf.Windows.Controls
{
    public class LibPanel : Panel, INotifyPropertyChanged
    {
        public void Connect(int connectionId, object target) { throw new NotSupportedException(); }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}