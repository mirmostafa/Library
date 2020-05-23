namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for SquareText01Tile.xaml
    /// </summary>
    public partial class SquareText01Tile
    {
        public string Header { get { return this.textBlock1.Text; } set { this.textBlock1.Text = value; } }
        public string Body1 { get { return this.textBlock2.Text; } set { this.textBlock2.Text = value; } }
        public string Body2 { get { return this.textBlock3.Text; } set { this.textBlock3.Text = value; } }
        public string Body3 { get { return this.textBlock4.Text; } set { this.textBlock4.Text = value; } }
        public SquareText01Tile() { this.InitializeComponent(); }
    }
}