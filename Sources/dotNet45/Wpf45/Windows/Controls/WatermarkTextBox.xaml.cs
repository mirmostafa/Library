using System.Windows;
using System.Windows.Controls;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for WatermarkTextBox.xaml
    /// </summary>
    public partial class WatermarkTextBox : TextBox
    {
        public string Watermark { get; set; }
        public Style GotFocusStyle { get; set; }
        public Style LostFocusStyle { get; set; }
        public WatermarkTextBox() { this.InitializeComponent(); }
    }
}