using System.Windows;

namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for WideText01Tile.xaml
    /// </summary>
    public partial class WideText14Tile
    {
        public static readonly DependencyProperty BodyProperty = DependencyProperty.Register("Body",
            typeof(string),
            typeof(WideText14Tile),
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

        public string Header { get { return this.textBlock1.Text; } set { this.textBlock1.Text = value; } }
        public string Badge { get { return this.textBlockBadge.Text; } set { this.textBlockBadge.Text = value; } }
        public WideText14Tile() { this.InitializeComponent(); }

        protected override void HookCommand()
        {
            var libCommand = this.Command;
            if (libCommand == null)
                return;
            var header = libCommand.Header;
            if (header != null)
            {
                this.Header = header.StartsWith("_") ? header.Substring(1) : header;
            }
            else if (libCommand.Content is string)
            {
                var s = this.Command.Content.ToString();
                this.Header = s.StartsWith("_") ? s.Substring(1) : s;
            }
            this.Body = libCommand.Body;
        }
    }
}