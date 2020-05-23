using System.ComponentModel;
using System.Windows;
using Mohammad.EventsArgs;
using Mohammad.Wpf.Windows.Dialogs;

namespace TestWpfApp45.Pages
{
    /// <summary>
    ///     Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void SettingsPage_OnCommonDialogClosing(object sender, CancelEventArgs e)
        {
        }

        private void SettingsPage_OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void SettingsPage_OnOkButtonClicked(object sender, ActingEventArgs e)
        {
            MsgBoxEx.Inform("Test");
            e.Handled = true;
        }
    }
}