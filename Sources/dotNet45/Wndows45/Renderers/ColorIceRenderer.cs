using System.Drawing;

namespace Mohammad.Win.Renderers
{
    public class ColorIceRenderer : BasicRenderer<ColorIceRenderer>
    {
        protected override Color BackgroundGradientEndColor { get { return Color.White; } }
        protected override Color BackgroundGradientStartColor { get { return Color.Violet; } }
    }
}