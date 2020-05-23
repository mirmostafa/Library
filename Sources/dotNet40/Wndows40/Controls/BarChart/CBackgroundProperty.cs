#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Library40.Win.Controls.BarChart
{
	[TypeConverter(typeof (ExpandableObjectConverter))]
	public class CBackgroundProperty
	{
		#region PaintingModes enum
		public enum PaintingModes
		{
			SolidColor,
			LinearGradient,
			RadialGradient
		}
		#endregion

		[Browsable(false)]
		private readonly GraphicsPath pathGradient;

		private Brush brush;
		private Color gradientColor1 = Color.FromArgb(0xff, 140, 210, 0xf5);
		private Color gradientColor2 = Color.FromArgb(0xff, 0, 30, 90);
		private PaintingModes paintingMode = PaintingModes.RadialGradient;
		[Browsable(false)]
		private PointF radialCenterPoint;
		private RectangleF rectBound;
		[Browsable(false)]
		private RectangleF rectGradient;
		private Color solidColor;

		public CBackgroundProperty()
		{
			this.solidColor = this.gradientColor2;
			this.rectGradient = RectangleF.Empty;
			this.pathGradient = new GraphicsPath();
		}

		[Browsable(false)]
		public RectangleF BoundRect
		{
			get { return this.rectBound; }
		}

		[Browsable(true)]
		public Color GradientColor1
		{
			get { return this.gradientColor1; }
			set
			{
				this.gradientColor1 = value;
				this.ResetBrush();
			}
		}

		[Browsable(true)]
		public Color GradientColor2
		{
			get { return this.gradientColor2; }
			set
			{
				this.gradientColor2 = value;
				this.ResetBrush();
			}
		}

		[Browsable(true)]
		public PaintingModes PaintingMode
		{
			get { return this.paintingMode; }
			set
			{
				if (value != this.paintingMode)
				{
					this.paintingMode = value;
					this.ResetBrush();
				}
			}
		}

		[Browsable(true)]
		public Color SolidColor
		{
			get { return this.solidColor; }
			set
			{
				this.solidColor = value;
				this.ResetBrush();
			}
		}

		private void CreateGradientBrush()
		{
			if ((this.rectBound.Width >= 1f) && (this.rectBound.Height >= 1f))
			{
				this.rectGradient.X = this.rectBound.Left - (this.rectBound.Width / 2f);
				this.rectGradient.Y = this.rectBound.Top - (this.rectBound.Height / 3f);
				this.rectGradient.Width = this.rectBound.Width * 2f;
				this.rectGradient.Height = this.rectBound.Height + (this.rectBound.Height / 2f);
				this.radialCenterPoint.X = this.rectBound.Left + (this.rectBound.Width / 2f);
				this.radialCenterPoint.Y = this.rectBound.Top + (this.rectBound.Height / 2f);
				this.pathGradient.Reset();
				this.pathGradient.AddEllipse(this.rectGradient);
				var brush = new PathGradientBrush(this.pathGradient)
				            {
					            CenterPoint = this.radialCenterPoint,
					            CenterColor = this.gradientColor1
				            };
				var colorArray = new[]
				                 {
					                 this.gradientColor2
				                 };
				brush.SurroundColors = colorArray;
				this.brush = brush;
			}
		}

		public void Draw(Graphics gr, RectangleF rectBound)
		{
			this.SetBoundRect(rectBound);
			this.ResetBrush();
			if (this.brush != null)
				gr.FillRectangle(this.brush, rectBound);
		}

		private void ResetBrush()
		{
			if (this.brush != null)
			{
				this.brush.Dispose();
				this.brush = null;
			}
			if (this.PaintingMode == PaintingModes.LinearGradient)
			{
				if (this.BoundRect.Height > 0f)
					this.brush = new LinearGradientBrush(new Point((int)this.BoundRect.X, (int)this.BoundRect.Y),
						new Point((int)this.BoundRect.X, (int)this.BoundRect.Bottom),
						this.GradientColor1,
						this.GradientColor2);
			}
			else if (this.PaintingMode == PaintingModes.RadialGradient)
				this.CreateGradientBrush();
			else
				this.brush = new SolidBrush(this.SolidColor);
		}

		public void SetBoundRect(RectangleF boundRect)
		{
			this.rectBound.X = boundRect.X;
			this.rectBound.Y = boundRect.Y;
			this.rectBound.Width = boundRect.Width;
			this.rectBound.Height = boundRect.Height;
		}
	}
}