namespace Library.Wpf.Bases;

public class ApplicationBase : Application
{
    public ApplicationBase() =>
        this.ApplyTheme();

    protected virtual void OnApplyingTheme() =>
        Current.Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri("/Library.Wpf;component/Themes/DefaultTheme.xaml", UriKind.RelativeOrAbsolute)
        });

    private void ApplyTheme() =>
        this.OnApplyingTheme();
}