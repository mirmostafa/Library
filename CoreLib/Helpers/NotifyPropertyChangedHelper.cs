using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Helpers;
public static class NotifyPropertyChangedHelper
{
    public static TNotifyPropertyChanged HandlePropertyChanges<TNotifyPropertyChanged>(this TNotifyPropertyChanged o, in PropertyChangedEventHandler? propertyChangedHandler)
        where TNotifyPropertyChanged: INotifyPropertyChanged
    {
        o.PropertyChanged -= propertyChangedHandler;
        o.PropertyChanged += propertyChangedHandler;
        return o;
    }
}
