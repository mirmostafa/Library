#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Drawing;

namespace Library35.Windows.Renderers
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