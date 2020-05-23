#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Linq;

namespace Library40.Wpf.Windows.Controls.Tiles
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
			get { return this.textBlock1.Text; }
			set { this.textBlock1.Text = value; }
		}

		public string Body2
		{
			get { return this.textBlock2.Text; }
			set { this.textBlock2.Text = value; }
		}

		public string Body3
		{
			get { return this.textBlock3.Text; }
			set { this.textBlock3.Text = value; }
		}

		public string Body4
		{
			get { return this.textBlock4.Text; }
			set { this.textBlock4.Text = value; }
		}
	}
}