#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.ComponentModel;
using System.Drawing;

namespace Library35.Windows.Controls.BarChart
{
	[TypeConverter(typeof (ExpandableObjectConverter))]
	public class CBorderProperty
	{
		private bool bVisible;
		private Color color;
		private int nSize;
		private Pen pen;
		private RectangleF rectBound;

		public CBorderProperty()
		{
			this.BoundRect = new RectangleF(0f, 0f, 0f, 0f);
			this.Visible = true;
			this.Color = Color.White;
			this.Width = 1;
		}

		[Browsable(false)]
		public RectangleF BoundRect
		{
			get { return this.rectBound; }
			set { this.rectBound = value; }
		}

		[Browsable(true)]
		public Color Color
		{
			get { return this.color; }
			set
			{
				this.color = value;
				this.ResetPen();
			}
		}

		[Browsable(true)]
		public bool Visible
		{
			get { return this.bVisible; }
			set { this.bVisible = value; }
		}

		[Browsable(true)]
		public int Width
		{
			get { return this.nSize; }
			set
			{
				this.nSize = value;
				this.ResetPen();
			}
		}

		public void Draw(Graphics gr)
		{
			if (this.rectBound == RectangleF.Empty)
				this.rectBound = gr.VisibleClipBounds;
			if (this.bVisible)
			{
				if (this.pen == null)
					this.ResetPen();
				if (this.pen != null)
					gr.DrawRectangle(this.pen, this.rectBound.X + (this.Width / 2), this.rectBound.Y + (this.Width / 2), this.rectBound.Width - this.Width, this.rectBound.Height - this.Width);
			}
		}

		private void ResetPen()
		{
			if (this.pen != null)
			{
				this.pen.Dispose();
				this.pen = null;
			}
			if (this.nSize > 0)
				this.pen = new Pen(this.color, this.nSize);
		}

		internal void SetRect(Rectangle rect)
		{
			this.rectBound.X = rect.X;
			this.rectBound.Y = rect.Y;
			this.rectBound.Width = rect.Width;
			this.rectBound.Height = rect.Height;
		}
	}
}