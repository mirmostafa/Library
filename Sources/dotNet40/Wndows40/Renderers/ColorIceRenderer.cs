#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Drawing;

namespace Library40.Win.Renderers
{
	public class ColorIceRenderer : BasicRenderer<ColorIceRenderer>
	{
		protected override Color BackgroundGradientEndColor
		{
			get { return Color.White; }
		}

		protected override Color BackgroundGradientStartColor
		{
			get { return Color.Violet; }
		}
	}
}