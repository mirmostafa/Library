using System.Windows.Media;

namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for SquareImageTile.xaml
    /// </summary>
    public partial class SquareImageTile : BaseTile
    {
        public SquareImageTile()
        {
            this.InitializeComponent();
        }

        public Brush Image
        {
            get => this.Background;
            set => this.Background = value;
        }
    }
}