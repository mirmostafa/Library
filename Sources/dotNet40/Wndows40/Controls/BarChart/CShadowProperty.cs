#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.ComponentModel;
using System.Drawing;

namespace Library40.Win.Controls.BarChart
{
	[TypeConverter(typeof (ExpandableObjectConverter))]
	public class CShadowProperty
	{
		#region Modes enum
		public enum Modes
		{
			None,
			Inner,
			Outer,
			Both
		}
		#endregion

		private Color colorInner = Color.FromArgb(100, 0, 0, 0);
		private Color colorOuter = Color.FromArgb(100, 0, 0, 0);
		private Modes mode = Modes.Inner;
		private int nSizeInner = 5;
		private int nSizeOuter = 5;
		private Pen pen;
		private Pen penBack;
		private RectangleF rectInner = RectangleF.Empty;
		private RectangleF rectOuter = RectangleF.Empty;

		[Browsable(true)]
		public Color ColorInner
		{
			get { return this.colorInner; }
			set { this.colorInner = value; }
		}

		[Browsable(true)]
		public Color ColorOuter
		{
			get { return this.colorOuter; }
			set { this.colorOuter = value; }
		}

		[Browsable(true)]
		public Modes Mode
		{
			get { return this.mode; }
			set { this.mode = value; }
		}

		[Browsable(true)]
		public int WidthInner
		{
			get { return this.nSizeInner; }
			set { this.nSizeInner = value; }
		}

		[Browsable(true)]
		public int WidthOuter
		{
			get { return this.nSizeOuter; }
			set { this.nSizeOuter = value; }
		}

		public void Draw(Graphics gr, Color colorBK)
		{
			if (this.mode != Modes.None)
			{
				if ((this.mode == Modes.Outer) || (this.mode == Modes.Both))
				{
					if ((this.nSizeOuter <= 0) || (this.nSizeOuter > this.colorOuter.A))
						return;
					this.DrawOuterShadow(gr, colorBK);
				}
				if (((this.mode == Modes.Inner) || (this.mode == Modes.Both)) && ((this.nSizeInner > 0) && (this.nSizeInner <= this.colorInner.A)))
					this.DrawInnerShadow(gr);
			}
		}

		private void DrawInnerShadow(Graphics gr)
		{
			if (this.rectInner != Rectangle.Empty)
			{
				if (this.pen == null)
					this.pen = new Pen(this.colorInner);
				if (this.pen.Color != this.colorInner)
					this.pen.Color = this.colorInner;
				var rect = new Rectangle((int)(this.rectInner.X + (this.pen.Width / 2f)),
					(int)(this.rectInner.Y + (this.pen.Width / 2f)),
					(int)(this.rectInner.Width - this.pen.Width),
					(int)(this.rectInner.Height - this.pen.Width));
				var num = this.colorInner.A / this.nSizeInner;
				if (num <= 0)
					num = 1;
				for (int i = this.colorInner.A; i > 0; i -= num)
				{
					this.pen.Color = Color.FromArgb(i, this.pen.Color);
					gr.DrawRectangle(this.pen, rect);
					rect.Inflate(-1, -1);
				}
			}
		}

		private void DrawOuterShadow(Graphics gr, Color colorBK)
		{
			if (this.rectOuter != Rectangle.Empty)
			{
				if (((this.penBack == null) || (this.penBack.Width != this.nSizeOuter)) || (this.penBack.Color != colorBK))
					this.penBack = new Pen(colorBK, this.nSizeOuter);
				gr.DrawRectangle(this.penBack,
					new Rectangle((int)(this.rectOuter.X + (this.penBack.Width / 2f)),
						(int)(this.rectOuter.Y + (this.penBack.Width / 2f)),
						(int)(this.rectOuter.Width - this.penBack.Width),
						(int)(this.rectOuter.Height - this.penBack.Width)));
				if (this.pen == null)
					this.pen = new Pen(this.colorOuter, 1f);
				if (this.pen.Color != this.colorOuter)
					this.pen.Color = this.colorOuter;
				var rect = new Rectangle((int)(this.rectOuter.X + (this.pen.Width / 2f)),
					(int)(this.rectOuter.Y + (this.pen.Width / 2f)),
					(int)(this.rectOuter.Width - this.pen.Width),
					(int)(this.rectOuter.Height - this.pen.Width));
				var num = this.colorOuter.A / this.nSizeOuter;
				if (num <= 0)
					num = 1;
				for (var i = 0; i < this.colorOuter.A; i += num)
				{
					this.pen.Color = Color.FromArgb(i, this.pen.Color);
					gr.DrawRectangle(this.pen, rect);
					rect.Inflate(-1, -1);
				}
			}
		}

		public void SetRect(RectangleF rect, int nIndex)
		{
			this.SetRect(rect.X, rect.Y, rect.Width, rect.Height, nIndex);
		}

		public void SetRect(float x, float y, float width, float height, int nIndex)
		{
			if (nIndex == 0)
			{
				this.rectInner.X = x;
				this.rectInner.Y = y;
				this.rectInner.Width = width;
				this.rectInner.Height = height;
			}
			else
			{
				this.rectOuter.X = x;
				this.rectOuter.Y = y;
				this.rectOuter.Width = width;
				this.rectOuter.Height = height;
			}
		}
	}
}