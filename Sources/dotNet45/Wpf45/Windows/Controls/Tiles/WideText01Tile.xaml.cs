namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for WideText01Tile.xaml
    /// </summary>
    public partial class WideText01Tile
    {
        public string Header { get { return this.textBlock1.Text; } set { this.textBlock1.Text = value; } }
        public string Body1 { get { return this.textBlock2.Text; } set { this.textBlock2.Text = value; } }
        public string Body2 { get { return this.textBlock3.Text; } set { this.textBlock3.Text = value; } }
        public string Body3 { get { return this.textBlock4.Text; } set { this.textBlock4.Text = value; } }
        public WideText01Tile() { this.InitializeComponent(); }

        protected override void HookCommand()
        {
            var header = this.Command.Header;
            if (header != null)
            {
                this.Header = header.StartsWith("_") ? header.Substring(1) : header;
            }
            else if (this.Command.Content is string)
            {
                var s = this.Command.Content.ToString();
                this.Header = s.StartsWith("_") ? s.Substring(1) : s;
            }
        }
    }
}