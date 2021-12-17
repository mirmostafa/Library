using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.Validations;
using Library.Wpf.Markers;

namespace Library.Wpf.Bases;

[ViewModel]
public abstract class ViewModelBase : IViewModel, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected virtual TViewModel SetProperty<TViewModel, TProperty>(ref TProperty backingField,
                                                                    TProperty value,
                                                                    [CallerMemberName] in string? propertyName = null)
        where TViewModel : ViewModelBase, INotifyPropertyChanged =>
        ControlHelper.SetProperty<TViewModel, TProperty>(this.Is<TViewModel>(), ref backingField, value, this.OnPropertyChanged, propertyName);
}
