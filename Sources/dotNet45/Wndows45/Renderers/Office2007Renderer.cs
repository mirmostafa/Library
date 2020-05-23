using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Mohammad.Win.Renderers.Internal;
using ProfessionalColorTable = System.Windows.Forms.ProfessionalColorTable;

namespace Mohammad.Win.Renderers
{
    /// <summary>
    ///     Draw ToolStrips using the Office 2007 themed appearance.
    /// </summary>
    public class Office2007Renderer : ToolStripProfessionalRenderer
    {
        public static Office2007Renderer Initialize(IContainerControl containerControl)
        {
            var result = new Office2007Renderer();

            ToolStripManager.Renderer = result;

            return result;

            //foreach (ToolStrip toolStrip in ControlHelper.GetControls<ToolStrip>(form))
            //    toolStrip.Renderer = windowsVistaRenderer;

            //foreach (ToolStripPanel toolStripPanel in ControlHelper.GetControls<ToolStripPanel>(form))
            //    toolStripPanel.Renderer = windowsVistaRenderer;

            //foreach (ToolStripContentPanel toolStripContentPanel in ControlHelper.GetControls<ToolStripContentPanel>(form))
            //    toolStripContentPanel.Renderer = windowsVistaRenderer;
        }

        #region FieldsPrivate

        private const int _MarginInset = 2;
        private static readonly Blend _StatusStripBlend;

        #endregion

        #region MethodsPublic

        static Office2007Renderer()
        {
            // One time creation of the blend for the status strip gradient brush
            _StatusStripBlend = new Blend();
            _StatusStripBlend.Positions = new[] {0.0F, 0.2F, 0.3F, 0.4F, 0.8F, 1.0F};
            _StatusStripBlend.Factors = new[] {0.3F, 0.4F, 0.5F, 1.0F, 0.8F, 0.7F};
        }

        /// <summary>
        ///     Initialize a new instance of the Office2007Renderer class.
        /// </summary>
        public Office2007Renderer()
            : base(new Office2007BlueColorTable()) {}

        /// <summary>
        ///     Initializes a new instance of the Office2007Renderer class.
        /// </summary>
        /// <param name="professionalColorTable">
        ///     A <see cref="BSE.Windows.Forms.ProfessionalColorTable" /> to be used for painting.
        /// </param>
        public Office2007Renderer(ProfessionalColorTable professionalColorTable)
            : base(professionalColorTable) {}

        #endregion

        #region MethodsProtected

        /// <summary>
        ///     Raises the RenderArrow event.
        /// </summary>
        /// <param name="e">A ToolStripArrowRenderEventArgs that contains the event data.</param>
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            var colorTable = this.ColorTable as Internal.ProfessionalColorTable;
            if (colorTable != null)
            {
                if (e.Item.Owner.GetType() == typeof(MenuStrip) && e.Item.Selected == false && e.Item.Pressed == false)
                    if (colorTable.MenuItemText != Color.Empty)
                        e.ArrowColor = colorTable.MenuItemText;
                if (e.Item.Owner.GetType() == typeof(StatusStrip) && e.Item.Selected == false && e.Item.Pressed == false)
                    if (colorTable.StatusStripText != Color.Empty)
                        e.ArrowColor = colorTable.StatusStripText;
            }
            base.OnRenderArrow(e);
        }

        /// <summary>
        ///     Raises the RenderItemText event.
        /// </summary>
        /// <param name="e">A ToolStripItemTextRenderEventArgs that contains the event data.</param>
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            var colorTable = this.ColorTable as Internal.ProfessionalColorTable;
            if (colorTable != null)
            {
                if (e.ToolStrip is MenuStrip && e.Item.Selected == false && e.Item.Pressed == false)
                    if (colorTable.MenuItemText != Color.Empty)
                        e.TextColor = colorTable.MenuItemText;
                if (e.ToolStrip is StatusStrip && e.Item.Selected == false && e.Item.Pressed == false)
                    if (colorTable.StatusStripText != Color.Empty)
                        e.TextColor = colorTable.StatusStripText;
            }
            base.OnRenderItemText(e);
        }

        /// <summary>
        ///     Raises the RenderToolStripContentPanelBackground event.
        /// </summary>
        /// <param name="e">An ToolStripContentPanelRenderEventArgs containing the event data.</param>
        protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e)
        {
            // Must call base class, otherwise the subsequent drawing does not appear!
            base.OnRenderToolStripContentPanelBackground(e);

            // Cannot paint a zero sized area
            if (e.ToolStripContentPanel.Width > 0 && e.ToolStripContentPanel.Height > 0)
                using (
                    var backBrush = new LinearGradientBrush(e.ToolStripContentPanel.ClientRectangle,
                        this.ColorTable.ToolStripContentPanelGradientBegin,
                        this.ColorTable.ToolStripContentPanelGradientEnd,
                        LinearGradientMode.Vertical))
                    e.Graphics.FillRectangle(backBrush, e.ToolStripContentPanel.ClientRectangle);
        }

        /// <summary>
        ///     Raises the RenderSeparator event.
        /// </summary>
        /// <param name="e">An ToolStripSeparatorRenderEventArgs containing the event data.</param>
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            e.Item.ForeColor = this.ColorTable.RaftingContainerGradientBegin;
            base.OnRenderSeparator(e);
        }

        /// <summary>
        ///     Raises the RenderToolStripBackground event.
        /// </summary>
        /// <param name="e">An ToolStripRenderEventArgs containing the event data.</param>
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is StatusStrip)
            {
                // We do not paint the top two pixel lines, so are drawn by the status strip border render method
                //RectangleF backRectangle = new RectangleF(0, 1.5f, e.ToolStrip.Width, e.ToolStrip.Height - 2);
                var backRectangle = new RectangleF(0, 0, e.ToolStrip.Width, e.ToolStrip.Height);

                // Cannot paint a zero sized area
                if (backRectangle.Width > 0 && backRectangle.Height > 0)
                    using (
                        var backBrush = new LinearGradientBrush(backRectangle,
                            this.ColorTable.StatusStripGradientBegin,
                            this.ColorTable.StatusStripGradientEnd,
                            LinearGradientMode.Vertical))
                    {
                        backBrush.Blend = _StatusStripBlend;
                        e.Graphics.FillRectangle(backBrush, backRectangle);
                    }
            }
            else
            {
                base.OnRenderToolStripBackground(e);
            }
        }

        /// <summary>
        ///     Raises the RenderImageMargin event.
        /// </summary>
        /// <param name="e">An ToolStripRenderEventArgs containing the event data.</param>
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is ContextMenuStrip || e.ToolStrip is ToolStripDropDownMenu)
            {
                // First with the total margin area
                var marginRectangle = e.AffectedBounds;

                // Do we need to draw with separator on the opposite edge?
                var bIsRightToLeft = e.ToolStrip.RightToLeft == RightToLeft.Yes;

                marginRectangle.Y += _MarginInset;
                marginRectangle.Height -= _MarginInset * 2;

                // Reduce so it is inside the border
                if (bIsRightToLeft == false)
                    marginRectangle.X += _MarginInset;
                else
                    marginRectangle.X += _MarginInset / 2;

                // Draw the entire margine area in a solid color
                using (var backBrush = new SolidBrush(this.ColorTable.ImageMarginGradientBegin))
                    e.Graphics.FillRectangle(backBrush, marginRectangle);
            }
            else
            {
                base.OnRenderImageMargin(e);
            }
        }

        #endregion

        #region MethodsPrivate

        private static GraphicsPath CreateBorderPath(Rectangle rectangle, float cut)
        {
            // Drawing lines requires we draw inside the area we want
            rectangle.Width--;
            rectangle.Height--;

            // Create path using a simple set of lines that cut the corner
            var path = new GraphicsPath();
            path.AddLine(rectangle.Left + cut, rectangle.Top, rectangle.Right - cut, rectangle.Top);
            path.AddLine(rectangle.Right - cut, rectangle.Top, rectangle.Right, rectangle.Top + cut);
            path.AddLine(rectangle.Right, rectangle.Top + cut, rectangle.Right, rectangle.Bottom - cut);
            path.AddLine(rectangle.Right, rectangle.Bottom - cut, rectangle.Right - cut, rectangle.Bottom);
            path.AddLine(rectangle.Right - cut, rectangle.Bottom, rectangle.Left + cut, rectangle.Bottom);
            path.AddLine(rectangle.Left + cut, rectangle.Bottom, rectangle.Left, rectangle.Bottom - cut);
            path.AddLine(rectangle.Left, rectangle.Bottom - cut, rectangle.Left, rectangle.Top + cut);
            path.AddLine(rectangle.Left, rectangle.Top + cut, rectangle.Left + cut, rectangle.Top);
            return path;
        }

        private static GraphicsPath CreateClipBorderPath(Rectangle rectangle, float cut)
        {
            // Clipping happens inside the rect, so make 1 wider and taller
            rectangle.Width++;
            rectangle.Height++;

            // Now create a path based on this inner rectangle
            return CreateBorderPath(rectangle, cut);
        }

        #endregion
    }
}