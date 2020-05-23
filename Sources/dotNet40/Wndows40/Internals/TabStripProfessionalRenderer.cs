#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Library40.Win.Controls;

namespace Library40.Win.Internals
{
	public class TabStripProfessionalRenderer : ToolStripProfessionalRenderer
	{
		private const int BOTTOM_LEFT = 0;
		private const int BOTTOM_RIGHT = 3;
		private const int TOP_LEFT = 1;
		private const int TOP_RIGHT = 2;

		private int D = -1;
		private int T = 2;
		private int X;

		private int X0;
		private int XF;
		private int Y;
		private int Y0;
		private int YF;
		private Color halocolor;
		private int i_Sweep = 90;
		private int i_Zero = 180;
		private GraphicsPath path;

		/// <summary>
		///     This renderer supports rendering of tabs.
		/// </summary>
		public TabStripProfessionalRenderer()
		{
			this.RoundedEdges = false; // get rid of the curves around the edges
			this.OnColor = Color.FromArgb(226, 209, 156);
			this.OnBackColor = Color.FromArgb(191, 219, 255);
		}

		public Color OnColor { get; set; }

		public Color OnBackColor { get; set; }

		public Color BaseColor { get; set; }

		public Color HaloColor
		{
			get { return this.halocolor; }
			set { this.halocolor = value; }
		}

		public Color SetTransparency(Color color, int transparency)
		{
			if (transparency >= 0 & transparency <= 255)
				return Color.FromArgb(transparency, color.R, color.G, color.B);
			if (transparency > 255)
				return Color.FromArgb(255, color.R, color.G, color.B);
			return Color.FromArgb(0, color.R, color.G, color.B);
		}

		/// <summary>
		///     Control when the background of the Tab is painting.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			var tabStrip = e.ToolStrip as TabStrip;
			var tab = e.Item as Tab;
			var i_opacity = tab.i_opacity;

			if (tab == null)
			{
				base.OnRenderButtonBackground(e);
				return;
			}
			var bounds = new Rectangle(Point.Empty, e.Item.Size);
			var g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = TextRenderingHint.AntiAlias;

			var textwidth = Convert.ToInt16(g.MeasureString(tab.Text, tab.Font).Width) - Convert.ToInt16(tab.Text.Length / 3);
			int textheight = Convert.ToInt16(g.MeasureString(tab.Text, tab.Font).Height);

			if (tabStrip != null)
			{
				#region Tab Selected and NOT active
				if (tab.b_selected & !tab.b_active)
				{
					#region Show Lateral Gradients
					var ri = bounds;
					ri.Width = (tab.Width - textwidth) / 2;
					ri.X += 2;
					ri.Width -= 2;
					var C0i = this.BaseColor;
					var c_origin = 255;
					C0i = this.SetTransparency(C0i, 0);
					var CFi = this.HaloColor;
					c_origin = 255;
					CFi = this.SetTransparency(CFi, c_origin - i_opacity);
					var b = new LinearGradientBrush(ri, C0i, CFi, LinearGradientMode.BackwardDiagonal);
					g.FillRectangle(b, ri.X, ri.Y, ri.Width, ri.Height);
					var offset = bounds.Width - ri.Width - 1;
					ri.Location = new Point(offset, 1);
					b = new LinearGradientBrush(ri, C0i, CFi, LinearGradientMode.ForwardDiagonal);
					g.FillRectangle(b, offset, ri.Y, ri.Width, ri.Height);
					#endregion

					#region Show Central Gradient
					var r = bounds;
					r.X += 2;
					r.Width -= 2;
					r.Height = r.Height / 2;
					var C0 = this.BaseColor;
					c_origin = 255;
					C0 = this.SetTransparency(C0, c_origin - i_opacity);
					var CF = this.HaloColor;
					c_origin = 255;
					CF = this.SetTransparency(CF, c_origin - i_opacity);
					b = new LinearGradientBrush(r, C0, CF, LinearGradientMode.Vertical);
					g.FillRectangle(b, r.X, r.Y + r.Height + 1, r.Width - r.X, r.Height);
					#endregion

					#region Show Vertical Side Lines
					var Offs = 0;
					this.X0 = 0;
					this.XF = tab.Width + this.X0;
					this.YF = tab.Height + this.Y0;
					this.Y0 = 0;
					var P0 = new Point(this.X0, this.Y0);
					var PF = new Point(this.X0, this.YF / 2);
					var C0sl = this.OnBackColor;
					C0sl = this.SetTransparency(C0sl, 0);
					var CFsl = Color.White;
					var rsl = new Rectangle(this.X0, this.Y0 + Offs, this.X0 + 1, this.YF / 2);
					b = new LinearGradientBrush(rsl, C0sl, CFsl, LinearGradientMode.Vertical);
					g.FillRectangle(b, this.X0, this.Y0 + Offs, rsl.Width, rsl.Height);
					rsl = new Rectangle(this.X0, this.YF / 2 - 1, this.X0 + 1, this.YF - Offs);
					b = new LinearGradientBrush(rsl, CFsl, C0sl, LinearGradientMode.Vertical);
					g.FillRectangle(b, this.X0, this.YF / 2 - 1, rsl.Width, rsl.Height);

					var rsr = new Rectangle(this.XF - 2, this.Y0 + Offs, this.XF - 1, this.YF / 2);
					b = new LinearGradientBrush(rsr, C0sl, CFsl, LinearGradientMode.Vertical);
					g.FillRectangle(b, this.XF - 1, this.Y0 + Offs, rsr.Width, rsr.Height);
					rsr = new Rectangle(this.XF - 2, this.YF / 2 - 1, this.XF - 1, this.YF - Offs);
					b = new LinearGradientBrush(rsr, CFsl, C0sl, LinearGradientMode.Vertical);
					g.FillRectangle(b, this.XF - 1, this.YF / 2 - 1, rsr.Width, rsr.Height);
					#endregion

					#region Show Border
					this.X0 = 0;
					this.XF = tab.Width + this.X0;
					this.Y0 = 0;
					this.YF = tab.Height + this.Y0;
					P0 = new Point(this.X0, this.Y0);
					PF = new Point(this.X0, this.Y0 + this.YF - 15);
					var BorderColor = this.BaseColor;
					c_origin = 255;
					BorderColor = this.SetTransparency(BorderColor, c_origin - i_opacity);
					var PBorder = new Pen(BorderColor);
					this.X = this.X0;
					this.Y = this.Y0;
					this.i_Zero = 270;
					this.D = 1;
					this.T = 5;
					this.DrawArc(0);
					g.DrawPath(PBorder, this.path);
					this.X = this.X0;
					this.Y = this.Y0;
					this.i_Zero = 270;
					this.D = 1;
					this.T = 5;
					this.DrawArc(1);
					var IBC = Color.White;
					c_origin = 164;
					IBC = this.SetTransparency(IBC, c_origin - i_opacity);
					PBorder = new Pen(IBC);
					g.DrawPath(PBorder, this.path);
					#endregion
				}
					#endregion

				else if (tab.b_active & !tab.b_selected)
				{
					#region Show Upper Rectangle
					var upblock = new Rectangle(8, 3, bounds.Width - 16, 4);
					g.FillRectangle(new SolidBrush(Color.FromArgb(245, 250, 255)), upblock);
					#endregion

					#region Show Bottom Rectangle
					var CFsl = this.BaseColor;
					var C0sl = Color.FromArgb(CFsl.R + 19, CFsl.G + 15, CFsl.B + 10);
					var doblock = new Rectangle(6, 3, bounds.Width - 12, bounds.Height);
					Brush b = new LinearGradientBrush(doblock, C0sl, CFsl, LinearGradientMode.Vertical);
					g.FillRectangle(b, doblock);
					#endregion

					#region Show Line Borders
					var b2 = Pens.White;
					var b3 = Pens.White;
					var b4 = Pens.White;
					int R0 = this.BaseColor.R;
					int G0 = this.BaseColor.G;
					int B0 = this.BaseColor.B;
					var TX0 = 0;
					var TY0 = 0;
					var TXF = bounds.Width;
					var TYF = bounds.Height;
					if (R0 != 0 & G0 != 0 & B0 != 0)
					{
						if (this.BaseColor.GetBrightness() < 0.5) //Dark Colors
						{
							b2 = new Pen(Color.FromArgb(R0 - 22, G0 - 11, B0));
							b3 = new Pen(Color.FromArgb(R0 + 18, G0 + 25, B0));
							b4 = new Pen(Color.FromArgb(R0, G0, B0));
						}
						else
						{
							b2 = new Pen(Color.FromArgb(R0 - 74, G0 - 49, B0 - 15));
							b3 = new Pen(Color.FromArgb(R0 - 8, G0 - 24, B0 + 10));
							b4 = new Pen(Color.FromArgb(R0, G0, B0));
						}

						this.DrawTab(TX0, TY0, TXF, TYF);
						g.DrawPath(b2, this.path);
						this.DrawTab(TX0 + 1, TY0 + 1, TXF - 1, TYF);
						g.DrawPath(b3, this.path);
						this.DrawTab(TX0 + 2, TY0 + 2, TXF - 2, TYF);
						g.DrawPath(b4, this.path);
					}
					#endregion

					#region Show Shadow
					var P0 = new Point(bounds.Right - 5, 3);
					var PF = new Point(bounds.Right - 5, bounds.Height - 2);
					var ps = new Pen(this.SetTransparency(Color.Black, 20));
					g.DrawLine(ps, P0, PF);
					P0 = new Point(bounds.Right - 4, 4);
					PF = new Point(bounds.Right - 4, bounds.Height - 1);
					ps = new Pen(this.SetTransparency(Color.Black, 10));
					g.DrawLine(ps, P0, PF);
					#endregion
				}
				else if (tab.b_active & tab.b_selected)
				{
					#region Show Upper Rectangle
					var upblock = new Rectangle(8, 3, bounds.Width - 16, 4);
					g.FillRectangle(new SolidBrush(Color.FromArgb(245, 250, 255)), upblock);
					#endregion

					#region Show Bottom Rectangle
					var CFsl = this.BaseColor;
					var C0sl = Color.FromArgb(CFsl.R + 19, CFsl.G + 15, CFsl.B + 10);
					var doblock = new Rectangle(6, 3, bounds.Width - 12, bounds.Height);
					Brush b = new LinearGradientBrush(doblock, C0sl, CFsl, LinearGradientMode.Vertical);
					g.FillRectangle(b, doblock);
					#endregion

					#region Show Line Borders
					var b2 = Pens.White;
					var b3 = Pens.White;
					var b4 = Pens.White;
					int R0 = this.BaseColor.R;
					int G0 = this.BaseColor.G;
					int B0 = this.BaseColor.B;
					var TX0 = 0;
					var TY0 = 0;
					var TXF = bounds.Width;
					var TYF = bounds.Height;
					if (R0 != 0 & G0 != 0 & B0 != 0)
					{
						if (this.BaseColor.GetBrightness() < 0.5) //Dark Colors
						{
							b2 = new Pen(Color.FromArgb(R0 - 26, G0 - 14, B0 - 3));
							b3 = new Pen(Color.FromArgb(R0 + 18, G0 + 25, B0));
							b4 = new Pen(Color.FromArgb(R0, G0, B0));
						}
						else
						{
							b2 = new Pen(Color.FromArgb(R0 - 74, G0 - 49, B0 - 15));
							b3 = new Pen(Color.FromArgb(R0 - 8, G0 - 24, B0 + 10));
							b4 = new Pen(Color.FromArgb(R0, G0, B0));
						}

						this.DrawTab(TX0, TY0, TXF, TYF);
						g.DrawPath(b2, this.path);
						this.DrawTab(TX0 + 1, TY0 + 1, TXF - 1, TYF);
						g.DrawPath(b3, this.path);
						this.DrawTab(TX0 + 2, TY0 + 2, TXF - 2, TYF);
						g.DrawPath(b4, this.path);
					}
					#endregion

					#region Show Shadow
					var P0 = new Point(bounds.Right - 5, 3);
					var PF = new Point(bounds.Right - 5, bounds.Height - 2);
					var ps = new Pen(this.SetTransparency(Color.Black, 20));
					g.DrawLine(ps, P0, PF);
					P0 = new Point(bounds.Right - 4, 4);
					PF = new Point(bounds.Right - 4, bounds.Height - 1);
					ps = new Pen(this.SetTransparency(Color.Black, 10));
					g.DrawLine(ps, P0, PF);
					#endregion

					#region Show Halo
					var CH = this.halocolor;
					var c_origin = 255;
					CH = this.SetTransparency(CH, c_origin - i_opacity);
					this.DrawHalo(TX0, TY0, TXF, TYF);
					g.DrawPath(new Pen(CH), this.path);
					this.DrawHalo(TX0 + 1, TY0 + 1, TXF + 1, TYF);
					g.DrawPath(new Pen(CH), this.path);
					#endregion
				}
			}
			else
				base.OnRenderButtonBackground(e);
		}

		/// <summary>
		///     Control how the border of the toolstrip draws - keep track of if there's
		///     a selected tab and make that look "connected" by skipping drawing a line through it.
		/// </summary>
		/// <param name="e"></param>
		/*  protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {
             TabStrip tabStrip = e.ToolStrip as TabStrip;
           
              using (Pen outerBlueBorderPen = new Pen(Color.Red)) {
                  using (Pen innerWhiteBorderPen = new Pen(Color.Red))
                  {
                      if (tabStrip != null) {
                          if (tabStrip.SelectedTab != null) {
                              // left border coords
                              Point borderStart1 = new Point(0, tabStrip.SelectedTab.Bounds.Bottom);
                              Point borderStop1 = new Point(tabStrip.SelectedTab.Bounds.Left, tabStrip.SelectedTab.Bounds.Bottom);

                              // right border coords
                              Point borderStart2 = new Point(tabStrip.SelectedTab.Bounds.Right - 1, tabStrip.SelectedTab.Bounds.Bottom);
                              Point borderStop2 = new Point(tabStrip.ClientRectangle.Right, tabStrip.SelectedTab.Bounds.Bottom);

                              e.Graphics.DrawLine(outerBlueBorderPen, borderStart1, borderStop1);
                              e.Graphics.DrawLine(outerBlueBorderPen, borderStart2, borderStop2);

                              // shift all points down one to draw the white line
                              borderStop1.Offset(0, 1);
                              borderStart1.Offset(0, 1);
                              borderStart2.Offset(0, 1);
                              borderStop2.Offset(0, 1);
                              e.Graphics.DrawLine(innerWhiteBorderPen, borderStart1, borderStop1);
                              e.Graphics.DrawLine(innerWhiteBorderPen, borderStart2, borderStop2);

                          }
                          else {
                              e.Graphics.DrawLine(outerBlueBorderPen, new Point(0, tabStrip.DisplayRectangle.Bottom), new Point(tabStrip.Width, tabStrip.DisplayRectangle.Bottom));
                              e.Graphics.DrawLine(innerWhiteBorderPen, new Point(0, tabStrip.DisplayRectangle.Bottom + 1), new Point(tabStrip.Width, tabStrip.DisplayRectangle.Bottom + 1));
                            

                          }   

                      }
                  }
              }
            
          }
       */
		public void DrawArc(int VOff)
		{
			this.i_Zero = 180;
			this.X0 = this.X0 + this.D;
			this.XF = this.XF - this.D;
			this.Y0 = this.Y0 + VOff;
			this.path = new GraphicsPath();
			var P0 = new Point(this.X0, this.YF);
			var PF = new Point(this.X0, this.Y0 + this.T);
			this.path.AddLine(P0, PF);
			this.path.AddArc(this.X0, this.Y0, this.T, this.T, this.i_Zero, this.i_Sweep);
			this.i_Zero += 90;
			P0 = new Point(this.X0 + this.T, this.Y0);
			PF = new Point(this.XF - this.T, this.Y0);
			this.path.AddLine(P0, PF);
			this.path.AddArc(this.XF - this.T - 1, this.Y0, this.T, this.T, this.i_Zero, this.i_Sweep);
			P0 = new Point(this.XF - 1, this.Y0 + this.T);
			PF = new Point(this.XF - 1, this.YF);
			this.path.AddLine(P0, PF);
		}

		public void DrawTab(int _X0, int _Y0, int _XF, int _YF)
		{
			this.T = 6;
			this.i_Zero = 90;
			this.path = new GraphicsPath();
			var P0 = new Point(_X0, _YF);
			var PF = new Point(_X0 + this.T, _YF - this.T);
			this.path.AddArc(_X0, _YF - this.T, this.T, this.T, this.i_Zero, -this.i_Sweep);
			this.i_Zero = 180; //path.AddLine(P0, PF); // _/
			this.path.AddArc(_X0 + this.T, _Y0, this.T, this.T, this.i_Zero, this.i_Sweep);
			this.i_Zero = 270; // path.AddLine(P0, PF); // /-
			this.path.AddArc(_XF - 2 * this.T, _Y0, this.T, this.T, this.i_Zero, this.i_Sweep);
			this.i_Zero = 180; // path.AddLine(P0, PF); // /-
			this.path.AddArc(_XF - this.T, _YF - this.T, this.T, this.T, this.i_Zero, -this.i_Sweep);
			// path.AddLine(P0, PF); // /-
		}

		public void DrawHalo(int _X0, int _Y0, int _XF, int _YF)
		{
			this.path = new GraphicsPath();
			var P0 = new Point(_X0 + 5, _YF - 3);
			var PF = new Point(_X0 + 6, _Y0 + 3);
			this.path.AddLine(P0, PF);
			P0 = new Point(_X0 + 7, _Y0);
			PF = new Point(_XF - 8, _Y0);
			this.path.AddLine(P0, PF);
			P0 = new Point(_XF - 7, _Y0 + 3);
			PF = new Point(_XF - 6, _YF - 3);
			this.path.AddLine(P0, PF);
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			//base.OnRenderToolStripBorder(e);
		}
	}
}