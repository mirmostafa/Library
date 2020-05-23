using System.Drawing;

namespace Mohammad.Win.Renderers
{
    public class ColorIceRenderer : BasicRenderer<ColorIceRenderer>
    {
        protected override Color BackgroundGradientEndColor => Color.White;
        protected override Color BackgroundGradientStartColor => Color.Violet;
    }
}