namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for WideText01Tile.xaml
    /// </summary>
    public partial class WideText03Tile
    {
        public WideText03Tile()
        {
            this.InitializeComponent();
        }

        public string Header
        {
            get => this.textBlock1.Text;
            set => this.textBlock1.Text = value;
        }

        protected override void HookCommand()
        {
            var header = this.Command.Header;
            if (header != null)
            {
                this.Header = header;
            }
            else if (this.Command.Content is string)
            {
                this.Header = this.Command.Content.ToString();
            }
        }
    }
}