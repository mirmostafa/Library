using System.Windows;

namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for WideText01Tile.xaml
    /// </summary>
    public partial class WideText13Tile
    {
        public static readonly DependencyProperty BodyProperty = DependencyProperty.Register("Body",
            typeof(string),
            typeof(WideText13Tile),
            new PropertyMetadata(default(string)));

        public string Body
        {
            get { return (string) this.GetValue(BodyProperty); }
            set
            {
                this.SetValue(BodyProperty, value);
                this.textBlock2.Text = value;
            }
        }

        public string Badge { get { return this.textBlockBadge.Text; } set { this.textBlockBadge.Text = value; } }
        public WideText13Tile() { this.InitializeComponent(); }

        protected override void HookCommand()
        {
            var libCommand = this.Command;
            if (libCommand == null)
                return;
            this.Body = libCommand.Body ?? (libCommand.Content ?? string.Empty).ToString();
        }
    }
}