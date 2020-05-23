using System.Windows;

namespace Mohammad.Wpf.Windows.Controls
{
    public interface ILibControl {}

    public interface IBindable
    {
        DependencyProperty BindingFieldProperty { get; }
    }

    public interface ILibSecured
    {
        string SecurityCode { get; set; }
        void SetControlUnSecured();
    }
}