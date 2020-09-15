namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for SquareText03Tile.xaml
    /// </summary>
    public partial class SquareText03Tile
    {
        public SquareText03Tile()
        {
            this.InitializeComponent();
        }

        public string Body1
        {
            get => this.textBlock1.Text;
            set => this.textBlock1.Text = value;
        }

        public string Body2
        {
            get => this.textBlock2.Text;
            set => this.textBlock2.Text = value;
        }

        public string Body3
        {
            get => this.textBlock3.Text;
            set => this.textBlock3.Text = value;
        }

        public string Body4
        {
            get => this.textBlock4.Text;
            set => this.textBlock4.Text = value;
        }
    }
}