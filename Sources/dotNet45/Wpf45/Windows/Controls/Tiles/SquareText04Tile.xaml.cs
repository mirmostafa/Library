namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for SquareText04Tile.xaml
    /// </summary>
    public partial class SquareText04Tile
    {
        public SquareText04Tile()
        {
            this.InitializeComponent();
        }

        public string Body
        {
            get => this.textBlock2.Text;
            set => this.textBlock2.Text = value;
        }
    }
}