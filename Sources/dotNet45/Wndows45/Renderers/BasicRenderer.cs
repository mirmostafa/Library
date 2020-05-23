using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Mohammad.Win.Renderers
{
    public class BasicRenderer<TBasicRenderer> : ToolStripRenderer
        where TBasicRenderer : BasicRenderer<TBasicRenderer>, new()
    {
        private Brush backgroundBrush;
        public Color ArrowColor { get { return Color.White; } }
        protected virtual Color BackgroundGradientEndColor { get { return SystemColors.ControlDark; } }
        protected virtual Color BackgroundGradientStartColor { get { return SystemColors.ControlDark; } }
        protected virtual Color FontColor { get { return SystemColors.ControlText; } }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            base.OnRenderArrow(e);
            e.ArrowColor = this.ArrowColor;
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderButtonBackground(e);
            var g = e.Graphics;
            if (e.Item.Selected)
            {
                LinearGradientBrush b;
                if (e.Item.Pressed)
                    using (
                        b =
                            new LinearGradientBrush(new Rectangle(0, 0, e.Item.Bounds.Width, e.Item.Bounds.Height),
                                Color.FromArgb(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B + 50),
                                Color.DarkGreen,
                                90f))
                        g.FillRectangle(b, new Rectangle(0, 0, e.Item.Bounds.Width, e.Item.Bounds.Height));
                else
                    using (b = new LinearGradientBrush(new Rectangle(0, 0, e.Item.Bounds.Width, e.Item.Bounds.Height), Color.Yellow, Color.Green, 90f))
                        g.FillRectangle(b, new Rectangle(0, 0, e.Item.Bounds.Width, e.Item.Bounds.Height));
            }
        }

        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e) { base.OnRenderDropDownButtonBackground(e); }
        protected override void OnRenderGrip(ToolStripGripRenderEventArgs e) { base.OnRenderGrip(e); }
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e) { base.OnRenderImageMargin(e); }
        protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e) { base.OnRenderItemBackground(e); }
        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e) { base.OnRenderItemCheck(e); }
        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e) { base.OnRenderItemImage(e); }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            base.OnRenderItemText(e);
            e.TextColor = this.FontColor;
        }

        protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e) { base.OnRenderLabelBackground(e); }
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) { base.OnRenderMenuItemBackground(e); }
        protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e) { base.OnRenderOverflowButtonBackground(e); }
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e) { base.OnRenderSeparator(e); }
        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e) { base.OnRenderSplitButtonBackground(e); }
        protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e) { base.OnRenderStatusStripSizingGrip(e); }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBackground(e);
            if (this.backgroundBrush == null)
                this.backgroundBrush = new LinearGradientBrush(e.ToolStrip.ClientRectangle,
                    this.BackgroundGradientStartColor,
                    this.BackgroundGradientEndColor,
                    90f,
                    true);
            e.Graphics.FillRectangle(this.backgroundBrush, e.AffectedBounds);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) { base.OnRenderToolStripBorder(e); }
        protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e) { base.OnRenderToolStripContentPanelBackground(e); }
        protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e) { base.OnRenderToolStripPanelBackground(e); }
        protected override void OnRenderToolStripStatusLabelBackground(ToolStripItemRenderEventArgs e) { base.OnRenderToolStripStatusLabelBackground(e); }
        protected void Reset() { this.backgroundBrush = null; }

        public static TBasicRenderer Initialize(IContainerControl containerControl)
        {
            var result = new TBasicRenderer();
            ToolStripManager.Renderer = result;
            return result;
        }
    }
}