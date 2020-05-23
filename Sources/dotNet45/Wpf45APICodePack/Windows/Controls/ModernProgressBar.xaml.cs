using System.Windows;
using Mohammad.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for ModernProgressBar.xaml
    /// </summary>
    public partial class ModernProgressBar
    {
        public ModernProgressBar()
        {
            this.InitializeComponent();
            this.Style = this.FindResource("ModernProgressBar").As<Style>();
        }
    }
}