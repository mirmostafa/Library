#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Library40.Win.Controls.BarChart
{
	public class HBarItem : ICloneable
	{
		private readonly Color ColorBacklightEnd;
		private readonly Color[] ColorGradientSurround;
		public RectangleF BoundRect;
		private Color ColorBorder;
		private Color ColorFillBK;
		private Color ColorGlowEnd;
		private Color ColorGlowStart;
		public RectangleF LabelRect;
		public RectangleF ValueRect;
		private SolidBrush brushFill;
		private LinearGradientBrush brushGlow;
		private PathGradientBrush brushGradient;
		protected Color colorBar;
		protected double dValue;
		private PointF gradientCenterPoint;
		private HBarItems parent;
		private GraphicsPath pathGradient;
		protected RectangleF rectBar;
		private RectangleF rectGlow;
		private RectangleF rectGradient;
		protected string strLabel;

		public HBarItem()
		{
			this.colorBar = Color.Empty;
			this.Value = 0.0;
			this.Label = string.Empty;
			this.Parent = null;
			this.ColorBacklightEnd = Color.FromArgb(80, 0, 0, 0);
			this.ColorGradientSurround = new[]
			                             {
				                             this.ColorBacklightEnd
			                             };
			this.ShowBorder = true;
			this.BarRect = RectangleF.Empty;
			this.BoundRect = new RectangleF(0f, 0f, 0f, 0f);
		}

		public HBarItem(double dValue, string strLabel, Color colorBar)
			: this()
		{
			this.Value = dValue;
			this.Label = strLabel;
			this.Color = colorBar;
		}

		public HBarItem(double dValue, string strLabel, Color colorBar, RectangleF barRect)
			: this(dValue, strLabel, colorBar)
		{
			this.rectBar = barRect;
		}

		public HBarItem(double dValue, string strLabel, Color colorBar, RectangleF rectfBar, RectangleF rectfBound)
			: this(dValue, strLabel, colorBar, rectfBar)
		{
			this.BarRect = rectfBound;
		}

		public HBarItem(double dValue, string strLabel, Color colorBar, RectangleF rectfBar, RectangleF rectfBound, HBarItems Parent)
			: this(dValue, strLabel, colorBar, rectfBar, rectfBound)
		{
			this.Parent = Parent;
		}

		public RectangleF BarRect
		{
			get { return this.rectBar; }
			set
			{
				this.rectBar = value;
				this.CreateGlowBrush();
			}
		}

		public Color Color
		{
			get { return this.colorBar; }
			set
			{
				this.colorBar = value;
				this.ColorFillBK = this.GetDarkerColor(this.Color, 0x55);
				this.ColorBorder = this.GetDarkerColor(this.Color, 100);
				if (this.brushFill != null)
				{
					this.brushFill.Dispose();
					this.brushFill = null;
				}
				this.brushFill = new SolidBrush(this.ColorFillBK);
			}
		}

		[Localizable(true)]
		public string Label
		{
			get { return this.strLabel; }
			set { this.strLabel = value; }
		}

		public HBarItems Parent
		{
			get { return this.parent; }
			set { this.parent = value; }
		}

		public bool ShowBorder { get; set; }

		public double Value
		{
			get { return this.dValue; }
			set
			{
				this.dValue = value;
				if (this.Parent != null)
					this.Parent.ShouldReCalculate = true;
			}
		}

		#region ICloneable Members
		object ICloneable.Clone()
		{
			return new HBarItem(this.Value, this.Label, this.Color, this.BarRect, this.BoundRect);
		}
		#endregion

		public object Clone()
		{
			return new HBarItem(this.Value, this.Label, this.Color, this.BarRect, this.BoundRect);
		}

		private void CreateGlowBrush()
		{
			if (this.rectBar.Height <= 0f)
				this.rectBar.Height = 1f;
			var num = (int)(185f + ((5f * this.BarRect.Width) / 24f));
			var num2 = (int)(10f + ((4f * this.BarRect.Width) / 24f));
			if (num > 0xff)
				num = 0xff;
			else if (num < 0)
				num = 0;
			if (num2 > 0xff)
				num2 = 0xff;
			else if (num2 < 0)
				num2 = 0;
			this.ColorGlowStart = Color.FromArgb(num2, 0xff, 0xff, 0xff);
			this.ColorGlowEnd = Color.FromArgb(num, 0xff, 0xff, 0xff);
			if (this.brushGlow != null)
			{
				this.brushGlow.Dispose();
				this.brushGlow = null;
			}
			this.rectGlow = new RectangleF(this.rectBar.Left, this.rectBar.Top, this.rectBar.Width / 2f, this.rectBar.Height);
			this.brushGlow = new LinearGradientBrush(new PointF(this.rectGlow.Right + 1f, this.rectGlow.Top),
				new PointF(this.rectGlow.Left - 1f, this.rectGlow.Top),
				this.ColorGlowStart,
				this.ColorGlowEnd);
		}

		private void CreateGradientBrush()
		{
			if (this.pathGradient == null)
				this.pathGradient = new GraphicsPath();
			if (this.brushGradient != null)
			{
				this.brushGradient.Dispose();
				this.brushGradient = null;
			}
			this.rectGradient.X = this.rectBar.Left - (this.rectBar.Width / 8f);
			this.rectGradient.Y = this.rectBar.Top - (this.rectBar.Height / 2f);
			this.rectGradient.Width = this.rectBar.Width * 2f;
			this.rectGradient.Height = this.rectBar.Height * 2f;
			this.gradientCenterPoint.X = this.rectBar.Right;
			this.gradientCenterPoint.Y = this.rectBar.Top + (this.rectBar.Height / 2f);
			this.pathGradient.Reset();
			this.pathGradient.AddEllipse(this.rectGradient);
			this.brushGradient = new PathGradientBrush(this.pathGradient)
			                     {
				                     CenterPoint = this.gradientCenterPoint,
				                     CenterColor = this.Color,
				                     SurroundColors = this.ColorGradientSurround
			                     };
		}

		public void Draw(Graphics gr)
		{
			if ((this.BarRect.Width > 0f) && (this.BarRect.Height > 0f))
			{
				if (this.parent.DrawingMode == HBarItems.DrawingModes.Solid)
					gr.FillRectangle(new SolidBrush(this.Color), this.BarRect);
				else
					gr.FillRectangle(this.brushFill, this.BarRect);
				if ((this.parent.DrawingMode == HBarItems.DrawingModes.Glass) || (this.parent.DrawingMode == HBarItems.DrawingModes.Rubber))
				{
					this.CreateGradientBrush();
					gr.FillRectangle(this.brushGradient, this.BarRect);
				}
				if (this.parent.DrawingMode == HBarItems.DrawingModes.Glass)
					gr.FillRectangle(this.brushGlow, this.rectGlow);
				if (this.ShowBorder)
					gr.DrawRectangle(new Pen(this.ColorBorder, 1f), this.rectBar.Left, this.rectBar.Top, this.rectBar.Width, this.rectBar.Height);
			}
		}

		private Color GetDarkerColor(Color color, byte intensity)
		{
			var num = color.R - intensity;
			var num2 = color.G - intensity;
			var num3 = color.B - intensity;
			if ((num > 0xff) || (num < 0))
				num *= -1;
			if ((num2 > 0xff) || (num2 < 0))
				num2 *= -1;
			if ((num3 > 0xff) || (num3 < 0))
				num3 *= -1;
			return Color.FromArgb(0xff, (byte)num, (byte)num2, (byte)num3);
		}
	}
}