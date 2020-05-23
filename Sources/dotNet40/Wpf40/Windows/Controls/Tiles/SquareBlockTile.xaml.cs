#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Linq;

namespace Library40.Wpf.Windows.Controls.Tiles
{
	/// <summary>
	///     Interaction logic for SquareBlockTile.xaml
	/// </summary>
	public partial class SquareBlockTile
	{
		public SquareBlockTile()
		{
			this.InitializeComponent();
		}

		public string Header
		{
			get { return this.textBlock1.Text; }
			set { this.textBlock1.Text = value; }
		}
		public string Body
		{
			get { return this.textBlock2.Text; }
			set { this.textBlock2.Text = value; }
		}
	}
}