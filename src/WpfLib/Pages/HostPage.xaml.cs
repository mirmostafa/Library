namespace Library.Wpf.Pages;

/// <summary>
/// Interaction logic for HostPage.xaml
/// </summary>
public partial class HostPage
{
    public HostPage() => this.InitializeComponent();

    /// <summary>
    /// Content of the Page
    /// </summary>
    /// <remarks>Page only supports one child</remarks>
    public new object Content
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
        if (this.Content is FrameworkElement element)
        {
            this.Width = 400;// element.Width;
            this.Height = 600;// element.Height;
        }
    }
}