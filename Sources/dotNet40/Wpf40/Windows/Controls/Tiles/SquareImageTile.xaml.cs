#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Linq;
using System.Windows.Media;

namespace Library40.Wpf.Windows.Controls.Tiles
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
			get { return this.Background; }
			set { this.Background = value; }
		}
	}
}