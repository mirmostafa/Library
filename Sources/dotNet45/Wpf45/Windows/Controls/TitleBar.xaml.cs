using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Mohammad.Wpf.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for TitleBar.xaml
    /// </summary>
    [ContentProperty("ItemsBar")]
    public partial class TitleBar
    {
        public static readonly DependencyProperty ParentWindowProperty = DependencyProperty.Register("ParentWindow",
            typeof(Window),
            typeof(TitleBar),
            new PropertyMetadata(default(Window)));

        public Window ParentWindow
        {
            get { return (Window) this.GetValue(ParentWindowProperty); }
            set
            {
                this.SetValue(ParentWindowProperty, value);
                this.OnParentFormChanged();
                this.OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title",
            typeof(string),
            typeof(TitleBar),
            new PropertyMetadata(default(string)));

        public string Title
        {
            get { return (string) this.GetValue(TitleProperty); }
            set
            {
                this.SetValue(TitleProperty, value);
                this.OnPropertyChanged();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public object ItemsBar
        {
            get { return this.ItemsBarContentPresenter.Content; }
            set
            {
                this.ItemsBarContentPresenter.Content = value;
                this.OnPropertyChanged();
            }
        }

        public TitleBar() { this.InitializeComponent(); }

        private void OnParentFormChanged()
        {
            this.Title = this.ParentWindow?.Title;
            this.BindToElementPath(this.ParentWindow, Window.TitleProperty, "Title");
            this.BindToElementPath(this.ParentWindow, BackgroundProperty, "Background");
        }

        private void TitleBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            var parent = this.Parent;
            while (parent != null && !(parent is Page) && !(parent is Window))
                parent = parent.GetParent();
            this.ParentWindow = parent as Window;
        }

        private void MinimizeButton_OnClick(object sender, RoutedEventArgs e) { this.ParentWindow.WindowState = WindowState.Minimized; }

        private void MaximizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            var parentWindow = this.ParentWindow;
            if (parentWindow != null)
                parentWindow.WindowState = parentWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e) { this.ParentWindow.Close(); }

        private void TitleBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) { this.ParentWindow?.DragMove(); }
    }
}