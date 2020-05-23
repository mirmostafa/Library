#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Library35.Helpers.Win;
using Library35.Windows.Renderers.Internal;

namespace Library35.Windows.Renderers
{
	/// <summary>
	///     Renders toolstrip items using Windows Vista look and feel
	/// </summary>
	/// <remarks>
	///     2007 José Manuel Menéndez Poo
	///     Visit my blog for upgrades and other renderers - www.menendezpoo.com
	/// </remarks>
	public class WindowsVistaRenderer : ToolStripRenderer
	{
		#region Static
		/// <summary>
		///     Creates the glow of the buttons
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		private static GraphicsPath CreateBottomRadialPath(Rectangle rectangle)
		{
			var path = new GraphicsPath();
			RectangleF rect = rectangle;
			rect.X -= rect.Width * .35f;
			rect.Y -= rect.Height * .15f;
			rect.Width *= 1.7f;
			rect.Height *= 2.3f;
			path.AddEllipse(rect);
			path.CloseFigure();
			return path;
		}

		/// <summary>
		///     Creates the chevron for the overflow button
		/// </summary>
		/// <param name="overflowButtonSize"></param>
		/// <returns></returns>
		private static GraphicsPath CreateOverflowChevron(Size overflowButtonSize)
		{
			var r = new Rectangle(Point.Empty, overflowButtonSize);
			var path = new GraphicsPath();

			const int segmentWidth = 3;
			const int segmentHeight = 3;
			const int segmentSeparation = 5;
			const int chevronWidth = segmentWidth + segmentSeparation;
			const int chevronHeight = segmentHeight * 2;
			var chevronLeft = (r.Width - chevronWidth) / 2;
			var chevronTop = (r.Height - chevronHeight) / 2;

			// Segment \
			path.AddLine(new Point(chevronLeft, chevronTop), new Point(chevronLeft + segmentWidth, chevronTop + segmentHeight));

			// Segment /
			path.AddLine(new Point(chevronLeft + segmentWidth, chevronTop + segmentHeight), new Point(chevronLeft, chevronTop + segmentHeight * 2));

			path.StartFigure();

			// Segment \
			path.AddLine(new Point(segmentSeparation + chevronLeft, chevronTop), new Point(segmentSeparation + chevronLeft + segmentWidth, chevronTop + segmentHeight));

			// Segment /
			path.AddLine(new Point(segmentSeparation + chevronLeft + segmentWidth, chevronTop + segmentHeight),
				new Point(segmentSeparation + chevronLeft, chevronTop + segmentHeight * 2));

			return path;
		}
		#endregion

		#region Fields
		#endregion

		#region Ctor
		public WindowsVistaRenderer()
		{
			this.ColorTable = new WindowsVistaColorTable();

			this.GlossyEffect = true;
			this.BackgroundGlow = true;
			this.ToolStripRadius = 2;
			this.ButtonRadius = 2;
		}
		#endregion

		#region Properties
		/// <summary>
		///     Gets or sets the buttons rectangle radius
		/// </summary>
		public int ButtonRadius { get; set; }

		/// <summary>
		///     Gets or sets the radius of the rectangle of the hole ToolStrip
		/// </summary>
		public int ToolStripRadius { get; set; }

		/// <summary>
		///     Gets ors sets if background glow should be rendered
		/// </summary>
		public bool BackgroundGlow { get; set; }

		/// <summary>
		///     Gets or sets if glossy effect should be rendered
		/// </summary>
		public bool GlossyEffect { get; set; }

		/// <summary>
		///     Gets or sets the color table of the renderer
		/// </summary>
		public WindowsVistaColorTable ColorTable { get; set; }
		#endregion

		#region Methods
		/// <summary>
		///     Gets a rounded rectangle representing the hole area of the toolstrip
		/// </summary>
		/// <param name="toolStrip"></param>
		/// <returns></returns>
		private GraphicsPath GetToolStripRectangle(ToolStrip toolStrip)
		{
			return GraphicsTools.CreateRoundRectangle(new Rectangle(0, 0, toolStrip.Width - 1, toolStrip.Height - 1), this.ToolStripRadius);
		}

		/// <summary>
		///     Draws the glossy effect on the toolbar
		/// </summary>
		/// <param name="g"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		private void DrawGlossyEffect(Graphics g, ToolStrip t)
		{
			this.DrawGlossyEffect(g, t, 0);
		}

		/// <summary>
		///     Draws the glossy effect on the toolbar
		/// </summary>
		/// <param name="g"></param>
		/// <param name="t"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		private void DrawGlossyEffect(Graphics g, ToolStrip t, int offset)
		{
			var glossyRect = new Rectangle(0, offset, t.Width - 1, (t.Height - 1) / 2);

			using (var b = new LinearGradientBrush(glossyRect.Location, new PointF(0, glossyRect.Bottom), this.ColorTable.GlossyEffectNorth, this.ColorTable.GlossyEffectSouth))
			using (var border = GraphicsTools.CreateTopRoundRectangle(glossyRect, this.ToolStripRadius))
				g.FillPath(b, border);
		}

		/// <summary>
		///     Renders the background of a button
		/// </summary>
		/// <param name="e"></param>
		private void DrawVistaButtonBackground(ToolStripItemRenderEventArgs e)
		{
			var chk = false;

			if (e.Item is ToolStripButton)
				chk = (e.Item as ToolStripButton).Checked;

			this.DrawVistaButtonBackground(e.Graphics, new Rectangle(Point.Empty, e.Item.Size), e.Item.Selected, e.Item.Pressed, chk);
		}

		/// <summary>
		///     Renders the background of a button on the specified rectangle using the specified device
		/// </summary>
		private void DrawVistaButtonBackground(Graphics g, Rectangle r, bool selected, bool pressed, bool checkd)
		{
			g.SmoothingMode = SmoothingMode.AntiAlias;

			var outerBorder = new Rectangle(r.Left, r.Top, r.Width - 1, r.Height - 1);
			var border = outerBorder;
			border.Inflate(-1, -1);
			var innerBorder = border;
			innerBorder.Inflate(-1, -1);
			var glossy = outerBorder;
			glossy.Height /= 2;
			var fill = innerBorder;
			fill.Height /= 2;
			var glow = Rectangle.FromLTRB(outerBorder.Left, outerBorder.Top + Convert.ToInt32(Convert.ToSingle(outerBorder.Height) * .5f), outerBorder.Right, outerBorder.Bottom);

			if ((!selected && !pressed) && !checkd)
				return;

			#region Layers
			//Outer border
			using (var path = GraphicsTools.CreateRoundRectangle(outerBorder, this.ButtonRadius))
			using (var p = new Pen(this.ColorTable.ButtonOuterBorder))
				g.DrawPath(p, path);

			//Checked fill
			if (checkd)
				using (var path = GraphicsTools.CreateRoundRectangle(innerBorder, 2))
				using (Brush b = new SolidBrush(selected ? this.ColorTable.CheckedButtonFillHot : this.ColorTable.CheckedButtonFill))
					g.FillPath(b, path);

			//Glossy effefct
			using (var path = GraphicsTools.CreateTopRoundRectangle(glossy, this.ButtonRadius))
			using (Brush b = new LinearGradientBrush(new Point(0, glossy.Top), new Point(0, glossy.Bottom), this.ColorTable.GlossyEffectNorth, this.ColorTable.GlossyEffectSouth))
				g.FillPath(b, path);

			//Border
			using (var path = GraphicsTools.CreateRoundRectangle(border, this.ButtonRadius))
			using (var p = new Pen(this.ColorTable.ButtonBorder))
				g.DrawPath(p, path);

			var fillNorth = pressed ? this.ColorTable.ButtonFillNorthPressed : this.ColorTable.ButtonFillNorth;
			var fillSouth = pressed ? this.ColorTable.ButtonFillSouthPressed : this.ColorTable.ButtonFillSouth;

			//Fill
			using (var path = GraphicsTools.CreateTopRoundRectangle(fill, this.ButtonRadius))
			using (Brush b = new LinearGradientBrush(new Point(0, fill.Top), new Point(0, fill.Bottom), fillNorth, fillSouth))
				g.FillPath(b, path);

			var innerBorderColor = pressed || checkd ? this.ColorTable.ButtonInnerBorderPressed : this.ColorTable.ButtonInnerBorder;

			//Inner border
			using (var path = GraphicsTools.CreateRoundRectangle(innerBorder, this.ButtonRadius))
			using (var p = new Pen(innerBorderColor))
				g.DrawPath(p, path);

			//Glow
			using (var clip = GraphicsTools.CreateRoundRectangle(glow, 2))
			{
				g.SetClip(clip, CombineMode.Intersect);

				var glowColor = this.ColorTable.Glow;

				if (checkd)
					glowColor = selected ? this.ColorTable.CheckedGlowHot : this.ColorTable.CheckedGlow;

				using (var brad = CreateBottomRadialPath(glow))
				using (var pgr = new PathGradientBrush(brad))
				{
					unchecked
					{
						const int opacity = 255;
						var bounds = brad.GetBounds();
						pgr.CenterPoint = new PointF((bounds.Left + bounds.Right) / 2f, (bounds.Top + bounds.Bottom) / 2f);
						pgr.CenterColor = Color.FromArgb(opacity, glowColor);
						pgr.SurroundColors = new[]
						                     {
							                     Color.FromArgb(0, glowColor)
						                     };
					}
					g.FillPath(pgr, brad);
				}
				g.ResetClip();
			}
			#endregion
		}

		/// <summary>
		///     Draws the background of a menu, vista style
		/// </summary>
		/// <param name="e"></param>
		private void DrawVistaMenuBackground(ToolStripItemRenderEventArgs e)
		{
			this.DrawVistaMenuBackground(e.Graphics, new Rectangle(Point.Empty, e.Item.Size), e.Item.Selected);
		}

		/// <summary>
		///     Draws the background of a menu, vista style
		/// </summary>
		/// <param name="g"></param>
		/// <param name="r"></param>
		/// <param name="highlighted"></param>
		private void DrawVistaMenuBackground(Graphics g, Rectangle r, bool highlighted)
		{
			//g.Clear(ColorTable.MenuBackground);

			const int margin = 2;

			#region IconSeparator
			const int left = 22;

			using (var p = new Pen(this.ColorTable.MenuDark))
				g.DrawLine(p, new Point(r.Left + left, r.Top), new Point(r.Left + left, r.Height - margin));

			using (var p = new Pen(this.ColorTable.MenuLight))
				g.DrawLine(p, new Point(r.Left + left + 1, r.Top), new Point(r.Left + left + 1, r.Height - margin));
			#endregion

			if (highlighted)

				#region Draw Rectangle
				using (var path = GraphicsTools.CreateRoundRectangle(new Rectangle(r.X + margin, r.Y + margin, r.Width - margin * 2, r.Height - margin * 2), 3))
				{
					using (Brush b = new LinearGradientBrush(new Point(0, 2), new Point(0, r.Height - 2), this.ColorTable.MenuHighlightNorth, this.ColorTable.MenuHighlightSouth))
						g.FillPath(b, path);

					using (var p = new Pen(this.ColorTable.MenuHighlight))
						g.DrawPath(p, path);
				}
			#endregion
		}

		/// <summary>
		///     Draws the border of the vista menu window
		/// </summary>
		/// <param name="g"></param>
		/// <param name="r"></param>
		private void DrawVistaMenuBorder(Graphics g, Rectangle r)
		{
			using (var p = new Pen(this.ColorTable.BackgroundBorder))
				g.DrawRectangle(p, new Rectangle(r.Left, r.Top, r.Width - 1, r.Height - 1));
		}
		#endregion

		protected override void Initialize(ToolStrip toolStrip)
		{
			base.Initialize(toolStrip);

			toolStrip.AutoSize = false;
			toolStrip.Height = 35;
			toolStrip.ForeColor = this.ColorTable.Text;
			toolStrip.GripStyle = ToolStripGripStyle.Hidden;
		}

		protected override void InitializeItem(ToolStripItem item)
		{
			base.InitializeItem(item);
			Console.WriteLine(item.GetType().Name);
			item.ForeColor = this.ColorTable.Text;

			item.Padding = new Padding(5);

			if (item is ToolStripSplitButton)
			{
				var btn = item as ToolStripSplitButton;
				btn.DropDownButtonWidth = 18;

				foreach (ToolStripItem subitem in btn.DropDownItems)
					if (subitem is ToolStripMenuItem)
					{
						var mnu = subitem as ToolStripMenuItem;
						mnu.AutoSize = false;
						mnu.Height = 26;
						mnu.TextAlign = ContentAlignment.MiddleLeft;
					}
			}

			if (item is ToolStripDropDownButton)
			{
				var btn = item as ToolStripDropDownButton;
				btn.ShowDropDownArrow = false;

				foreach (ToolStripItem subitem in btn.DropDownItems)
					if (subitem is ToolStripMenuItem)
					{
						var mnu = subitem as ToolStripMenuItem;
						mnu.AutoSize = false;
						mnu.Height = 26;
						mnu.TextAlign = ContentAlignment.MiddleLeft;
					}
			}
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			if (e.ToolStrip is ToolStripDropDownMenu)

				#region Draw Rectangled Border
				this.DrawVistaMenuBorder(e.Graphics, new Rectangle(Point.Empty, e.ToolStrip.Size));

				#endregion

			else
			{
				#region Draw Rounded Border
				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

				using (var path = this.GetToolStripRectangle(e.ToolStrip))
				using (var p = new Pen(this.ColorTable.BackgroundBorder))
					e.Graphics.DrawPath(p, path);
				#endregion
			}
		}

		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			if (e.ToolStrip is ToolStripDropDownMenu)
				return;

			#region Background
			using (var b = new LinearGradientBrush(Point.Empty, new PointF(0, e.ToolStrip.Height), this.ColorTable.BackgroundNorth, this.ColorTable.BackgroundSouth))
			using (var border = this.GetToolStripRectangle(e.ToolStrip))
				e.Graphics.FillPath(b, border);
			#endregion

			if (this.GlossyEffect)

				#region Glossy Effect
				this.DrawGlossyEffect(e.Graphics, e.ToolStrip, 1);
			#endregion

			if (!this.BackgroundGlow)
				return;

			#region BackroundGlow
			var glowSize = Convert.ToInt32(Convert.ToSingle(e.ToolStrip.Height) * 0.15f);
			var glow = new Rectangle(0, e.ToolStrip.Height - glowSize - 1, e.ToolStrip.Width - 1, glowSize);

			using (
				var b = new LinearGradientBrush(new Point(0, glow.Top - 1), new PointF(0, glow.Bottom), Color.FromArgb(0, this.ColorTable.BackgroundGlow), this.ColorTable.BackgroundGlow))
			using (var border = GraphicsTools.CreateBottomRoundRectangle(glow, this.ToolStripRadius))
				e.Graphics.FillPath(b, border);
			#endregion
		}

		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			if (e.Item.GetCurrentParent() is ContextMenuStrip || e.Item.GetCurrentParent() is ToolStripDropDownMenu)
				e.TextColor = this.ColorTable.ToolStripDropDownMenuText;
			else
				e.TextColor = this.ColorTable.MenuText;

			if (e.Item is ToolStripButton)
				e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

			if (e.Item is ToolStripMenuItem)
			{
				var r = new Rectangle(e.TextRectangle.Location, new Size(e.TextRectangle.Width, 24));
				e.TextRectangle = r;
			}

			base.OnRenderItemText(e);
		}

		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			this.DrawVistaButtonBackground(e);
		}

		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
		{
			this.DrawVistaButtonBackground(e);

			var item = e.Item as ToolStripDropDownButton;
			if (item == null)
				return;

			var arrowBounds = new Rectangle(item.Width - 18, 0, 18, item.Height);

			this.DrawArrow(new ToolStripArrowRenderEventArgs(e.Graphics, e.Item, arrowBounds, this.ColorTable.DropDownArrow, ArrowDirection.Down));
		}

		protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
		{
			this.DrawVistaButtonBackground(e);

			var item = e.Item as ToolStripSplitButton;
			if (item == null)
				return;

			var arrowBounds = item.DropDownButtonBounds;
			var buttonBounds = new Rectangle(item.ButtonBounds.Location, new Size(item.ButtonBounds.Width + 2, item.ButtonBounds.Height));
			var dropDownBounds = item.DropDownButtonBounds;

			this.DrawVistaButtonBackground(e.Graphics, buttonBounds, item.ButtonSelected, item.ButtonPressed, false);

			this.DrawVistaButtonBackground(e.Graphics, dropDownBounds, item.DropDownButtonSelected, item.DropDownButtonPressed, false);

			this.DrawArrow(new ToolStripArrowRenderEventArgs(e.Graphics, e.Item, arrowBounds, this.ColorTable.DropDownArrow, ArrowDirection.Down));
		}

		protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
		{
			if (!e.Item.Enabled)
				base.OnRenderItemImage(e);
			else if (e.Image != null)
				e.Graphics.DrawImage(e.Image, e.ImageRectangle);
		}

		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			this.DrawVistaMenuBackground(e.Graphics, new Rectangle(Point.Empty, e.Item.Size), e.Item.Selected);
		}

		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			if (e.Item.IsOnDropDown)
			{
				const int left = 20;
				var right = e.Item.Width - 3;
				var top = e.Item.Height / 2;
				top--;

				//e.Graphics.Clear(ColorTable.MenuBackground);

				using (var p = new Pen(this.ColorTable.MenuDark))
					e.Graphics.DrawLine(p, new Point(left, top), new Point(right, top));

				using (var p = new Pen(this.ColorTable.MenuLight))
					e.Graphics.DrawLine(p, new Point(left, top + 1), new Point(right, top + 1));
			}
			else
			{
				const int top = 3;
				var left = e.Item.Width / 2;
				left--;
				var height = e.Item.Height - top * 2;
				var separator = new RectangleF(left, top, 0.5f, height);

				using (
					Brush b = new LinearGradientBrush(separator.Location,
						new Point(Convert.ToInt32(separator.Left), Convert.ToInt32(separator.Bottom)),
						this.ColorTable.SeparatorNorth,
						this.ColorTable.SeparatorSouth))
					e.Graphics.FillRectangle(b, separator);
			}
		}

		protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
		{
			this.DrawVistaButtonBackground(e);

			using (var path = CreateOverflowChevron(e.Item.Size))
			using (var p = new Pen(Color.FromArgb(255, this.ColorTable.Text), 2f))
			{
				p.StartCap = LineCap.Round;
				p.EndCap = LineCap.Round;
				e.Graphics.DrawPath(p, path);
			}
		}

		public static void Render(ToolStripContainer toolStripContainer)
		{
			var windowsVistaRenderer = new WindowsVistaRenderer();

			foreach (var toolStripContentPanel in toolStripContainer.GetControls<ToolStripContentPanel>())
				windowsVistaRenderer.InitializeContentPanel(toolStripContentPanel);

			foreach (var toolStripPanel in toolStripContainer.GetControls<ToolStripPanel>())
				foreach (var toolStrip in toolStripPanel.GetControls<ToolStrip>())
				{
					windowsVistaRenderer.Initialize(toolStrip);
					toolStrip.Renderer = windowsVistaRenderer;
				}
		}

		public static void Initialize(IContainerControl containerControl)
		{
			var windowsVistaRenderer = new WindowsVistaRenderer();

			ToolStripManager.Renderer = windowsVistaRenderer;

			//foreach (ToolStrip toolStrip in ControlHelper.GetControls<ToolStrip>(form))
			//    toolStrip.Renderer = windowsVistaRenderer;

			//foreach (ToolStripPanel toolStripPanel in ControlHelper.GetControls<ToolStripPanel>(form))
			//    toolStripPanel.Renderer = windowsVistaRenderer;

			//foreach (ToolStripContentPanel toolStripContentPanel in ControlHelper.GetControls<ToolStripContentPanel>(form))
			//    toolStripContentPanel.Renderer = windowsVistaRenderer;
		}
	}
}