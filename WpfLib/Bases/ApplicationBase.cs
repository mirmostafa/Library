namespace Library.Wpf.Bases;
public class ApplicationBase : Application
{
    public ApplicationBase() => 
        this.OnApplyingTheme();

    protected virtual void OnApplyingTheme() => Current.Resources.MergedDictionaries.Add(new ResourceDictionary
    {
        //Source = new Uri("/WpfLib;component/Themes/DefaultTheme.xaml", UriKind.RelativeOrAbsolute)
        Source = new Uri("/Library.Wpf;component/Themes/DefaultTheme.xaml", UriKind.RelativeOrAbsolute)
    });//Current.Resources.MergedDictionaries.Add(new ResourceDictionary//{//    Source = new Uri("/WpfLib;component/Themes/ModernUI.xaml", UriKind.RelativeOrAbsolute)//});
}
