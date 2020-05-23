#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Library40.Win.Controls
{
	public class TabPanel : Panel
	{
		private readonly Timer timer1 = new Timer();
		private int B0 = 242;
		private int D = -1;
		private int G0 = 227;
		private int R0 = 215;

		private string S_TXT = "";
		private int T = 8;
		private int X;
		private int X0;
		private int XF;
		private int Y;
		private int Y0;
		private int YF;
		private Color _BaseColor = Color.FromArgb(215, 227, 242);
		private Color _BaseColorOn = Color.FromArgb(215, 227, 242);
		private int i_Op = 255;
		private int i_Sweep = 90;
		private int i_Zero = 180;
		private int i_fB = 1;
		private int i_fG = 1;
		private int i_fR = 1;
		private int i_factor = 1;
		private int i_mode; //0 Entering, 1 Leaving
		private GraphicsPath path;

		public TabPanel()
		{
			this.timer1.Interval = 1;
			this.timer1.Tick += this.timer1_Tick;
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

		public Color BaseColor
		{
			get { return this._BaseColor; }
			set
			{
				this._BaseColor = value;
				this.R0 = this._BaseColor.R;
				this.B0 = this._BaseColor.B;
				this.G0 = this._BaseColor.G;
			}
		}

		public Color BaseColorOn
		{
			get { return this._BaseColorOn; }
			set
			{
				this._BaseColorOn = value;
				this.R0 = this._BaseColor.R;
				this.B0 = this._BaseColor.B;
				this.G0 = this._BaseColor.G;
			}
		}
		[Localizable(true)]
		public string Caption
		{
			get { return this.S_TXT; }
			set
			{
				this.S_TXT = value;
				this.Refresh();
			}
		}

		public int Speed
		{
			get { return this.i_factor; }
			set { this.i_factor = value; }
		}

		public int Opacity
		{
			get { return this.i_Op; }
			set
			{
				if (value < 256 | value > -1)
					this.i_Op = value;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			this.X0 = 0;
			this.XF = this.Width + this.X0 - 3;
			this.Y0 = 0;
			this.YF = this.Height + this.Y0 - 3;

			var P0 = new Point(this.X0, this.Y0);
			var PF = new Point(this.X0, this.Y0 + this.YF);

			var b1 = new Pen(Color.FromArgb(this.i_Op, this.R0 - 18, this.G0 - 17, this.B0 - 19));
			var b2 = new Pen(Color.FromArgb(this.i_Op, this.R0 - 39, this.G0 - 24, this.B0 - 3));
			var b3 = new Pen(Color.FromArgb(this.i_Op, this.R0 + 11, this.G0 + 9, this.B0 + 3));
			var b4 = new Pen(Color.FromArgb(this.i_Op, this.R0 - 8, this.G0 - 4, this.B0 - 2));
			var b5 = new Pen(Color.FromArgb(this.i_Op, this.R0, this.G0, this.B0));
			var b6 = new Pen(Color.FromArgb(this.i_Op, this.R0 - 16, this.G0 - 11, this.B0 - 5));
			var b8 = new Pen(Color.FromArgb(this.i_Op, this.R0 + 1, this.G0 + +5, this.B0 + 3));
			var b7 = new Pen(Color.FromArgb(this.i_Op, this.R0 - 22, this.G0 - 10, this.B0));

			this.T = 1;
			this.DrawArc3(0, 20);
			e.Graphics.PageUnit = GraphicsUnit.Pixel;
			var B4 = b4.Brush;
			e.Graphics.SmoothingMode = SmoothingMode.None;
			this.X = this.X0;
			this.Y = this.Y0;
			this.i_Zero = 180;
			this.D = 0;

			e.Graphics.FillPath(b5.Brush, this.path);
			var brocha = new LinearGradientBrush(P0, PF, b6.Color, b8.Color);
			this.DrawArc2(15, this.YF - 20);
			e.Graphics.FillPath(brocha, this.path);
			this.DrawArc2(this.YF - 16, 12);
			var bdown = new Pen(Color.FromArgb(this.i_Op, this.R0 - 22, this.G0 - 11, this.B0));
			e.Graphics.FillPath(bdown.Brush, this.path);

			this.T = 6;
			this.DrawArc();
			this.DrawArc();
			e.Graphics.DrawPath(b2, this.path);
			this.DrawArc();
			e.Graphics.DrawPath(b3, this.path);

			var P_EX = Cursor.Position;
			P_EX = this.PointToClient(P_EX);

			var ix = 10 + this.Width / 2 - this.S_TXT.Length * (int)this.Font.Size / 2;
			var P_TXT = new PointF(ix, this.Height - 20);
			var pen = new Pen(this.ForeColor);
			e.Graphics.DrawString(this.S_TXT, this.Font, pen.Brush, P_TXT);

			base.OnPaint(e);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			var P_EX = Cursor.Position;
			P_EX = this.PointToClient(P_EX);
			if (P_EX.X > 0 | P_EX.X < this.Width | P_EX.Y > 0 | P_EX.Y < this.Height)
			{
				this.i_mode = 0;
				this.timer1.Start();
			}
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			var P_EX = Cursor.Position;
			P_EX = this.PointToClient(P_EX);
			if (P_EX.X < 0 | P_EX.X >= this.Width | P_EX.Y < 0 | P_EX.Y >= this.Height)
			{
				this.i_mode = 1;
				this.timer1.Start();
			}
			base.OnMouseLeave(e);
		}

		public void DrawArc()
		{
			this.X = this.X0;
			this.Y = this.Y0;
			this.i_Zero = 180;
			this.D++;
			this.path = new GraphicsPath();
			this.path.AddArc(this.X + this.D, this.Y + this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
			this.i_Zero += 90;
			this.X += this.XF - 6;
			this.path.AddArc(this.X - this.D, this.Y + this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
			this.i_Zero += 90;
			this.Y += this.YF - 6;
			this.path.AddArc(this.X - this.D, this.Y - this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
			this.i_Zero += 90;
			this.X -= this.XF - 6;
			this.path.AddArc(this.X + this.D, this.Y - this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
			this.i_Zero += 90;
			this.Y -= this.YF - 6;
			this.path.AddArc(this.X + this.D, this.Y + this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
		}

		public void DrawArc2(int OF_Y, int SW_Y)
		{
			this.X = this.X0 + 4;
			this.Y = this.Y0 + OF_Y;
			this.i_Zero = 180;
			this.path = new GraphicsPath();
			this.path.AddArc(this.X + this.D, this.Y + this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
			this.i_Zero += 90;
			this.X += this.XF - 8;
			this.path.AddArc(this.X - this.D, this.Y + this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
			this.i_Zero += 90;
			this.Y += SW_Y;
			this.path.AddArc(this.X - this.D, this.Y - this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
			this.i_Zero += 90;
			this.X -= this.XF - 8;
			this.path.AddArc(this.X + this.D, this.Y - this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
			this.i_Zero += 90;
			this.Y -= SW_Y;
			this.path.AddArc(this.X + this.D, this.Y + this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
		}

		public void DrawArc3(int OF_Y, int SW_Y)
		{
			this.X = this.X0;
			this.Y = this.Y0 + OF_Y;
			this.i_Zero = 180;
			this.path = new GraphicsPath();
			this.path.AddArc(this.X + this.D, this.Y + this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
			this.i_Zero += 90;
			this.X += this.XF;
			this.path.AddArc(this.X - this.D, this.Y + this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
			this.i_Zero += 90;
			this.Y += SW_Y;
			this.path.AddArc(this.X - this.D, this.Y - this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
			this.i_Zero += 90;
			this.X -= this.XF;
			this.path.AddArc(this.X + this.D, this.Y - this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
			this.i_Zero += 90;
			this.Y -= SW_Y;
			this.path.AddArc(this.X + this.D, this.Y + this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			#region Entering
			if (this.i_mode == 0)
			{
				if (Math.Abs(this._BaseColorOn.R - this.R0) > this.i_factor)
					this.i_fR = this.i_factor;
				else
					this.i_fR = 1;
				if (Math.Abs(this._BaseColorOn.G - this.G0) > this.i_factor)
					this.i_fG = this.i_factor;
				else
					this.i_fG = 1;
				if (Math.Abs(this._BaseColorOn.B - this.B0) > this.i_factor)
					this.i_fB = this.i_factor;
				else
					this.i_fB = 1;

				if (this._BaseColorOn.R < this.R0)
					this.R0 -= this.i_fR;
				else if (this._BaseColorOn.R > this.R0)
					this.R0 += this.i_fR;

				if (this._BaseColorOn.G < this.G0)
					this.G0 -= this.i_fG;
				else if (this._BaseColorOn.G > this.G0)
					this.G0 += this.i_fG;

				if (this._BaseColorOn.B < this.B0)
					this.B0 -= this.i_fB;
				else if (this._BaseColorOn.B > this.B0)
					this.B0 += this.i_fB;

				if (this._BaseColorOn == Color.FromArgb(this.R0, this.G0, this.B0))
					this.timer1.Stop();
				else
					this.Refresh();
			}
			#endregion

			#region Leaving
			if (this.i_mode == 1)
			{
				if (Math.Abs(this._BaseColor.R - this.R0) < this.i_factor)
					this.i_fR = 1;
				else
					this.i_fR = this.i_factor;
				if (Math.Abs(this._BaseColor.G - this.G0) < this.i_factor)
					this.i_fG = 1;
				else
					this.i_fG = this.i_factor;
				if (Math.Abs(this._BaseColor.B - this.B0) < this.i_factor)
					this.i_fB = 1;
				else
					this.i_fB = this.i_factor;

				if (this._BaseColor.R < this.R0)
					this.R0 -= this.i_fR;
				else if (this._BaseColor.R > this.R0)
					this.R0 += this.i_fR;
				if (this._BaseColor.G < this.G0)
					this.G0 -= this.i_fG;
				else if (this._BaseColor.G > this.G0)
					this.G0 += this.i_fG;
				if (this._BaseColor.B < this.B0)
					this.B0 -= this.i_fB;
				else if (this._BaseColor.B > this.B0)
					this.B0 += this.i_fB;

				if (this._BaseColor == Color.FromArgb(this.R0, this.G0, this.B0))
					this.timer1.Stop();
				else
					this.Refresh();
			}
			#endregion
		}

		protected override void OnResize(EventArgs eventargs)
		{
			this.Refresh();
			base.OnResize(eventargs);
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			this.ResumeLayout(false);
		}
	}
}