using System.Windows;

using Library.EventsArgs;
using Library.Wpf.Windows.Input.Commands;
using Library.Wpf.Windows.UI;

namespace WpfApp1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
        => this.InitializeComponent();

    private void Button_Click(object sender, RoutedEventArgs e) => 
        Toast2.ShowText("Hi");

    private void GoToYahooCommand_Navigating(object sender, ItemActedEventArgs<NavigationUICommand.NavigatingEventArgs> e)
    {
    }
}