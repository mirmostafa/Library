using System.Windows.Media;

namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for SquareImageTile.xaml
    /// </summary>
    public partial class SquareImageTile : BaseTile
    {
        public Brush Image { get { return this.Background; } set { this.Background = value; } }
        public SquareImageTile() { this.InitializeComponent(); }
    }
}