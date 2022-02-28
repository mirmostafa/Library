using Library.Wpf.Dialogs;
using Library.Wpf.Helpers;
using Library.Wpf.Windows.UI;
using System.Windows;
using WpfAppTest.Pages;

namespace WpfApp1;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
    }

    private void ModalPageTestButton_Click(object sender, RoutedEventArgs e)
    {
        HostDialog
            .Create<ModalPageTestPage>()
            .SetPrompt("سلام")
            .Show();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Toast.CreateLongTitle("content", "title", ApplicationHelper.ApplicationTitle).Show();
    }
}
