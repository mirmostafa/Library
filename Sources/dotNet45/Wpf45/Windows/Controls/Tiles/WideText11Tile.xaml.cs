namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for WideText01Tile.xaml
    /// </summary>
    public partial class WideText11Tile
    {
        public string Body { get { return this.textBlock1.Text; } set { this.textBlock1.Text = value; } }
        public string Badge { get { return this.textBlockBadge.Text; } set { this.textBlockBadge.Text = value; } }
        public WideText11Tile() { this.InitializeComponent(); }

        protected override void HookCommand()
        {
            var libCommand = this.Command;
            if (libCommand == null)
                return;
            var header = libCommand.Header;
            if (header != null)
            {
                this.Body = header.StartsWith("_") ? header.Substring(1) : header;
            }
            else if (libCommand.Content is string)
            {
                var s = this.Command.Content.ToString();
                this.Body = s.StartsWith("_") ? s.Substring(1) : s;
            }
        }
    }
}