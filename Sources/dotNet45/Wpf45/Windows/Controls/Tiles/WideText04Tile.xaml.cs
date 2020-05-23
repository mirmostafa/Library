namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for WideText01Tile.xaml
    /// </summary>
    public partial class WideText04Tile
    {
        public string Header { get { return this.textBlock1.Text; } set { this.textBlock1.Text = value; } }
        public WideText04Tile() { this.InitializeComponent(); }
    }
}