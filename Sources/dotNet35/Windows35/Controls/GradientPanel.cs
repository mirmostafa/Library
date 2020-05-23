#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Library35.Windows.Controls.GradientPanelStructs;
using LinearGradientMode = Library35.Windows.Controls.GradientPanelStructs.LinearGradientMode;

namespace Library35.Windows.Controls
{
	[ToolboxBitmap(typeof (Panel))]
	public partial class GradientPanel : Panel
	{
		// Fields
		private Color _BackColour1 = SystemColors.Window;
		private Color _BackColour2 = SystemColors.Window;
		private Color _BorderColour = SystemColors.WindowFrame;
		private BorderStyle _BorderStyle = BorderStyle.None;
		private int _BorderWidth = 1;
		private int _Curvature;
		// Properties
		//   Shadow the Backcolor property so that the base class will still render with a transparent backcolor
		private CornerCurveMode _CurveMode = CornerCurveMode.All;
		private LinearGradientMode _GradientMode = LinearGradientMode.None;

		public GradientPanel()
		{
			this.SetDefaultControlStyles();
			this.customInitialisation();
		}

		[DefaultValue(typeof (Color), "Window"), Category("Appearance"), Description("The primary background color used to display text and graphics in the control.")]
		public new Color BackColor
		{
			get { return this._BackColour1; }
			set
			{
				this._BackColour1 = value;
				if (this.DesignMode)
					this.Invalidate();
			}
		}

		[DefaultValue(typeof (Color), "Window"), Category("Appearance"), Description("The secondary background color used to paint the control.")]
		public Color BackColor2
		{
			get { return this._BackColour2; }
			set
			{
				this._BackColour2 = value;
				if (this.DesignMode)
					this.Invalidate();
			}
		}

		[DefaultValue(typeof (LinearGradientMode), "None"), Category("Appearance"), Description("The gradient direction used to paint the control.")]
		public LinearGradientMode GradientMode
		{
			get { return this._GradientMode; }
			set
			{
				this._GradientMode = value;
				if (this.DesignMode)
					this.Invalidate();
			}
		}

		[DefaultValue(typeof (BorderStyle), "None"), Category("Appearance"), Description("The border style used to paint the control.")]
		public new BorderStyle BorderStyle
		{
			get { return this._BorderStyle; }
			set
			{
				this._BorderStyle = value;
				if (this.DesignMode)
					this.Invalidate();
			}
		}

		[DefaultValue(typeof (Color), "WindowFrame"), Category("Appearance"), Description("The border color used to paint the control.")]
		public Color BorderColor
		{
			get { return this._BorderColour; }
			set
			{
				this._BorderColour = value;
				if (this.DesignMode)
					this.Invalidate();
			}
		}

		[DefaultValue(typeof (int), "1"), Category("Appearance"), Description("The width of the border used to paint the control.")]
		public int BorderWidth
		{
			get { return this._BorderWidth; }
			set
			{
				this._BorderWidth = value;
				if (this.DesignMode)
					this.Invalidate();
			}
		}

		[DefaultValue(typeof (int), "0"), Category("Appearance"), Description("The radius of the curve used to paint the corners of the control.")]
		public int Curvature
		{
			get { return this._Curvature; }
			set
			{
				this._Curvature = value;
				if (this.DesignMode)
					this.Invalidate();
			}
		}

		[DefaultValue(typeof (CornerCurveMode), "All"), Category("Appearance"), Description("The style of the curves to be drawn on the control.")]
		public CornerCurveMode CurveMode
		{
			get { return this._CurveMode; }
			set
			{
				this._CurveMode = value;
				if (this.DesignMode)
					this.Invalidate();
			}
		}

		private int _AdjustedCurve
		{
			get
			{
				var curve = 0;
				if (!(this._CurveMode == CornerCurveMode.None))
				{
					curve = this._Curvature > (this.ClientRectangle.Width / 2) ? DoubleToInt(this.ClientRectangle.Width / 2) : this._Curvature;
					if (curve > (this.ClientRectangle.Height / 2))
						curve = DoubleToInt(this.ClientRectangle.Height / 2);
				}
				return curve;
			}
		}

		private void SetDefaultControlStyles()
		{
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, false);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}

		private void customInitialisation()
		{
			this.SuspendLayout();
			base.BackColor = Color.Transparent;
			this.BorderStyle = BorderStyle.None;
			this.ResumeLayout(false);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			base.OnPaintBackground(pevent);
			pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			var graphPath = this.GetPath();
			//	Create Gradient Brush (Cannot be width or height 0)
			LinearGradientBrush filler;
			var rect = this.ClientRectangle;
			if (this.ClientRectangle.Width == 0)
				rect.Width += 1;
			if (this.ClientRectangle.Height == 0)
				rect.Height += 1;
			if (this._GradientMode == LinearGradientMode.None)
				filler = new LinearGradientBrush(rect, this._BackColour1, this._BackColour1, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
			else
				filler = new LinearGradientBrush(rect, this._BackColour1, this._BackColour2, ((System.Drawing.Drawing2D.LinearGradientMode)this._GradientMode));
			pevent.Graphics.FillPath(filler, graphPath);
			filler.Dispose();
			switch (this._BorderStyle)
			{
				case BorderStyle.FixedSingle:
				{
					var borderPen = new Pen(this._BorderColour, this._BorderWidth);
					pevent.Graphics.DrawPath(borderPen, graphPath);
					borderPen.Dispose();
				}
					break;
				case BorderStyle.Fixed3D:
					DrawBorder3D(pevent.Graphics, this.ClientRectangle);
					break;
				case BorderStyle.None:
					break;
			}
			filler.Dispose();
			graphPath.Dispose();
		}

		protected GraphicsPath GetPath()
		{
			var graphPath = new GraphicsPath();
			if (this._BorderStyle == BorderStyle.Fixed3D)
				graphPath.AddRectangle(this.ClientRectangle);
			else
				try
				{
					var curve = 0;
					var rect = this.ClientRectangle;
					var offset = 0;
					switch (this._BorderStyle)
					{
						case BorderStyle.FixedSingle:
							if (this._BorderWidth > 1)
								offset = DoubleToInt(this.BorderWidth / 2);
							curve = this._AdjustedCurve;
							break;
						case BorderStyle.Fixed3D:
							break;
						case BorderStyle.None:
							curve = this._AdjustedCurve;
							break;
					}
					if (curve == 0)
						graphPath.AddRectangle(Rectangle.Inflate(rect, -offset, -offset));
					else
					{
						var rectWidth = rect.Width - 1 - offset;
						var rectHeight = rect.Height - 1 - offset;
						int curveWidth;
						if ((this._CurveMode & CornerCurveMode.TopRight) != 0)
							curveWidth = (curve * 2);
						else
							curveWidth = 1;
						graphPath.AddArc(rectWidth - curveWidth, offset, curveWidth, curveWidth, 270, 90);
						if ((this._CurveMode & CornerCurveMode.BottomRight) != 0)
							curveWidth = (curve * 2);
						else
							curveWidth = 1;
						graphPath.AddArc(rectWidth - curveWidth, rectHeight - curveWidth, curveWidth, curveWidth, 0, 90);
						if ((this._CurveMode & CornerCurveMode.BottomLeft) != 0)
							curveWidth = (curve * 2);
						else
							curveWidth = 1;
						graphPath.AddArc(offset, rectHeight - curveWidth, curveWidth, curveWidth, 90, 90);
						if ((this._CurveMode & CornerCurveMode.TopLeft) != 0)
							curveWidth = (curve * 2);
						else
							curveWidth = 1;
						graphPath.AddArc(offset, offset, curveWidth, curveWidth, 180, 90);
						graphPath.CloseFigure();
					}
				}
				catch (Exception)
				{
					graphPath.AddRectangle(this.ClientRectangle);
				}
			return graphPath;
		}

		public static void DrawBorder3D(Graphics graphics, Rectangle rectangle)
		{
			graphics.SmoothingMode = SmoothingMode.Default;
			graphics.DrawLine(SystemPens.ControlDark, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Y);
			graphics.DrawLine(SystemPens.ControlDark, rectangle.X, rectangle.Y, rectangle.X, rectangle.Height - 1);
			graphics.DrawLine(SystemPens.ControlDarkDark, rectangle.X + 1, rectangle.Y + 1, rectangle.Width - 1, rectangle.Y + 1);
			graphics.DrawLine(SystemPens.ControlDarkDark, rectangle.X + 1, rectangle.Y + 1, rectangle.X + 1, rectangle.Height - 1);
			graphics.DrawLine(SystemPens.ControlLight, rectangle.X + 1, rectangle.Height - 2, rectangle.Width - 2, rectangle.Height - 2);
			graphics.DrawLine(SystemPens.ControlLight, rectangle.Width - 2, rectangle.Y + 1, rectangle.Width - 2, rectangle.Height - 2);
			graphics.DrawLine(SystemPens.ControlLightLight, rectangle.X, rectangle.Height - 1, rectangle.Width - 1, rectangle.Height - 1);
			graphics.DrawLine(SystemPens.ControlLightLight, rectangle.Width - 1, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
		}

		public static int DoubleToInt(double value)
		{
			return Decimal.ToInt32(Decimal.Floor(Decimal.Parse((value).ToString())));
		}
	}

	namespace GradientPanelStructs
	{
		public enum LinearGradientMode
		{
			Horizontal = 0,
			Vertical = 1,
			ForwardDiagonal = 2,
			BackwardDiagonal = 3,
			None = 4
		}

		[Flags]
		public enum CornerCurveMode
		{
			None = 0,
			TopLeft = 1,
			TopRight = 2,
			TopLeftTopRight = 3,
			BottomLeft = 4,
			TopLeftBottomLeft = 5,
			TopRightBottomLeft = 6,
			TopLeftTopRightBottomLeft = 7,
			BottomRight = 8,
			BottomRightTopLeft = 9,
			BottomRightTopRight = 10,
			BottomRightTopLeftTopRight = 11,
			BottomRightBottomLeft = 12,
			BottomRightTopLeftBottomLeft = 13,
			BottomRightTopRightBottomLeft = 14,
			All = 15
		}
	}
}