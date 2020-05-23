using Mohammad.Wpf.Windows.Input.LibCommands;

namespace TestWpfApp45
{
    public class Commands
    {
        public static ShowWindowCommand ShowMainWindow = new ShowWindowCommand {Content = "Show _Main Window…", WindowType = typeof(MainWindow)};
    }
}