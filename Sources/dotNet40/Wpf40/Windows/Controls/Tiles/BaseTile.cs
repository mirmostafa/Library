#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Linq;
using System.Windows.Controls;

namespace Library40.Wpf.Windows.Controls.Tiles
{
	public class BaseTile : UserControl
	{
		private TileScaleType _ScaleType;

		protected TileScaleType ScaleType
		{
			get { return this._ScaleType; }
			set
			{
				this._ScaleType = value;
				switch (value)
				{
					case TileScaleType.Square:
						this.Width = 150;
						this.Height = 150;
						break;
					case TileScaleType.SquareWide:
						break;
					case TileScaleType.SquareLong:
						break;
					default:
						throw new ArgumentOutOfRangeException("value");
				}
			}
		}

		protected override void OnInitialized(EventArgs e)
		{
			//this.FontFamily = new FontFamily("Segoe UI Light");
			//this.FontSize = 14;
			//this.Background = new LinearGradientBrush(Colors.RoyalBlue, Colors.Blue, 45);
			//this.Foreground = Brushes.Snow;
			base.OnInitialized(e);
		}
	}

	public enum TileScaleType
	{
		Square,
		SquareWide,
		SquareLong,
	}
}