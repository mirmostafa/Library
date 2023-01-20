namespace Library.Wpf.Pages;

/// <summary>
/// Interaction logic for HostPage.xaml
/// </summary>
public partial class HostPage
{
    public HostPage() 
        => this.InitializeComponent();

    /// <summary>
    /// Gets or sets the content of a System.Windows.Controls.Page.
    /// </summary>
    /// <remarks>An object that contains the content of a System.Windows.Controls.Page. Page supports one child only.</remarks>
    public new object? Content
    {
        get => base.Content;
        set
        {
            base.Content = value;
            this.OnContentChanged();
        }
    }

    private void OnContentChanged()
    {
        if (this.Content is FrameworkElement)
        {
            this.Width = 400;// element.Width;
            this.Height = 600;// element.Height;
        }
    }
}