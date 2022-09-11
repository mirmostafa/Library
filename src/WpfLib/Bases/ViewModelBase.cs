using Library.ComponentModel;
using Library.Wpf.Markers;

namespace Library.Wpf.Bases;

[Markers.ViewModel]
public abstract class ViewModelBase : NotifyPropertyChanged, IViewModel
{

}