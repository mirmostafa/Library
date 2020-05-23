using System.ComponentModel;

namespace Mohammad.Wpf.Windows.Controls
{
    public interface IDescriptiveObject : INotifyPropertyChanged
    {
        string Title { get; set; }
        string Description { get; set; }
        string AppTitle { get; set; }
    }
}