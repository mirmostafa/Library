namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for WideText01Tile.xaml
    /// </summary>
    public partial class WideText04Tile
    {
        public WideText04Tile()
        {
            this.InitializeComponent();
        }

        public string Header
        {
            get => this.textBlock1.Text;
            set => this.textBlock1.Text = value;
        }
    }
}