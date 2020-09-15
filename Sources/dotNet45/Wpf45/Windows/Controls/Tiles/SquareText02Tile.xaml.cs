namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for SquareText02Tile.xaml
    /// </summary>
    public partial class SquareText02Tile
    {
        public SquareText02Tile()
        {
            this.InitializeComponent();
        }

        public string Header
        {
            get => this.textBlock1.Text;
            set => this.textBlock1.Text = value;
        }

        public string Body
        {
            get => this.textBlock2.Text;
            set => this.textBlock2.Text = value;
        }
    }
}