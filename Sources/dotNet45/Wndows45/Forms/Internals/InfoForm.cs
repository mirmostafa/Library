using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Mohammad.Win.Forms.Internals
{
    internal class InfoForm : Form
    {
        private readonly int i_Sweep = 90;
        private readonly int ms = 300;
        private readonly Timer timer = new Timer();
        private readonly Timer timerClos = new Timer();
        private string _comment = "";
        private Color _fillcolor;
        private string _picture = "";
        private string _title = "";
        private bool appearing = true;
        private Brush b = new SolidBrush(Color.FromArgb(160, 0, 255, 0));
        private int D = -1;
        private int HC = 90;
        private int i_Zero = 180;
        private Image img;
        private int j = 10;
        private Pen p = new Pen(Color.Black, 8.0f);
        private GraphicsPath path;
        private OffInfoShadow shadow;
        private int T = 2;
        private int WC = 220;
        private int X;
        private int X0;
        private int XC = 8;
        private int XF;
        private int Y;
        private int Y0;
        private int YC = 20;
        private int YF;

        public string Title
        {
            get { return this._title; }
            set
            {
                this.Size = new Size(170, 30);
                this._title = value;
            }
        }

        public string Comment
        {
            get { return this._comment; }
            set
            {
                if (value != "")
                {
                    this.Size = new Size(250, 100);
                    this._comment = value;
                    this.timerClos.Interval = (this._comment.Length + 30) * 1000;
                }
            }
        }

        public string Picture
        {
            get { return this._picture; }
            set
            {
                if (value != "")
                {
                    this.Size = new Size(390, 180);
                    this.XC = 122;
                    this.YC = 30;
                    this.WC = 240;
                    this.HC = 120;
                    this._picture = value;
                    try
                    {
                        this.img = Image.FromFile(this._picture);
                    }
                    catch {}
                }
            }
        }

        public Color FillColor { get { return this._fillcolor; } set { this._fillcolor = value; } }

        public InfoForm()
        {
            //Transparency and Alpha Channel Enabled
            this.BackColor = Color.Fuchsia;
            this.TransparencyKey = Color.Fuchsia;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.ShowInTaskbar = false;
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Width = 10;
            this.Height = 10;
            this.Opacity = 0.8;
            this.StartPosition = FormStartPosition.Manual;

            this.timer.Interval = 5;

            this.timer.Tick += this.timer_Tick;
            this.timerClos.Tick += this.timerClos_Tick;
        }

        private void timerClos_Tick(object sender, EventArgs e) { this.Close(); }

        protected override void OnLoad(EventArgs e)
        {
            this.shadow = new OffInfoShadow();
            this.shadow.Location = new Point(this.Location.X + 8, this.Location.Y + 12);
            this.shadow.Size = this.Size;
            this.shadow.Show();
            this.timer.Start();
            base.OnLoad(e);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.appearing)
                if (this.Opacity == 1)
                    if (this.j < this.ms)
                        this.j++;
                    else
                        this.appearing = !this.appearing;
                else
                    this.Opacity += 0.1;
            if (!this.appearing)
                if (this.Opacity == 0)
                {
                    this.Close();
                }
                else
                {
                    this.Opacity -= 0.2;
                    this.shadow.Close();
                }
            this.timerClos.Start();
        }

        public new void Close()
        {
            this.appearing = false;
            this.timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            //Paint the Background             
            this.T = 6;
            this.D = -1;
            this.X0 = 0;
            this.Y0 = 0;
            this.XF = e.ClipRectangle.Width - 10;
            this.YF = e.ClipRectangle.Height - 7;
            var rpath = new Rectangle(this.X0, this.Y0, this.XF, this.YF + 14);
            this.DrawArc();
            try
            {
                LinearGradientBrush b;
                if (this._fillcolor.GetBrightness() > 0.5)
                {
                    this.ForeColor = Color.FromArgb(0, 0, 0);
                    b = new LinearGradientBrush(rpath, Color.White, this._fillcolor, LinearGradientMode.Vertical);
                }
                else
                {
                    this.ForeColor = Color.FromArgb(220, 220, 220);
                    b = new LinearGradientBrush(rpath, this._fillcolor, Color.FromArgb(60, 60, 60), LinearGradientMode.Vertical);
                }
                e.Graphics.FillPath(b, this.path);
            }
            catch {}
            this.T = 6;
            this.D = -1;
            this.X0 = 0;
            this.Y0 = 0;
            this.XF = e.ClipRectangle.Width - 10;
            this.YF = e.ClipRectangle.Height - 7;
            rpath = new Rectangle(this.X0, this.Y0, this.XF, this.YF + 14);
            this.DrawArc();

            e.Graphics.DrawPath(new Pen(Color.FromArgb(118, 118, 118)), this.path);

            //Title
            var TitlePos = new Point(5, 3);
            var TitleFont = new Font(this.Font, FontStyle.Bold);
            var ForePen = new Pen(this.ForeColor);
            g.DrawString(this._title, TitleFont, ForePen.Brush, TitlePos);
            //Comment
            var rect = new RectangleF(this.XC, this.YC, this.WC, this.HC);
            g.DrawString(this._comment, this.Font, ForePen.Brush, rect);

            if (this.img != null)
            {
                g.DrawImage(this.img, 12, 30, this.img.Width, this.img.Height);
                var pline = new Pen(this._fillcolor);
                g.DrawLine(pline, 5, 166, 372, 166);
                g.DrawLine(new Pen(Color.White), 5, 167, 372, 167);
            }

            base.OnPaint(e);
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
            this.X += this.XF;
            this.path.AddArc(this.X - this.D, this.Y + this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
            this.i_Zero += 90;
            this.Y += this.YF;
            this.path.AddArc(this.X - this.D, this.Y - this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
            this.i_Zero += 90;
            this.X -= this.XF;
            this.path.AddArc(this.X + this.D, this.Y - this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
            this.i_Zero += 90;
            this.Y -= this.YF;
            this.path.AddArc(this.X + this.D, this.Y + this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
        }
    }

    public class OffInfoShadow : Form
    {
        private readonly int i_Sweep = 90;
        private int D = -1;
        private int i_Zero = 180;
        private GraphicsPath path;
        private int T = 2;
        private int X;
        private int X0, XF;
        private int Y;
        private int Y0, YF;

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        public OffInfoShadow()
        {
            this.ShowInTaskbar = false;
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.TransparencyKey = Color.FromArgb(255, 255, 255);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.Opacity = 0.5;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            this.T = 12;
            this.D = -1;
            this.X0 = -2;
            this.Y0 = -2;
            this.XF = e.ClipRectangle.Width - 14;
            this.YF = e.ClipRectangle.Height - 14;
            var rpath = new Rectangle(this.X0, this.Y0, this.XF, this.YF);
            this.DrawArc();
            var HBlack = Color.FromArgb(120, 100, 100, 100);
            var pgb = new PathGradientBrush(this.path);
            var SBlack = Color.FromArgb(255, 100, 100, 100);
            pgb.CenterColor = SBlack;
            pgb.SurroundColors = new[] {HBlack};
            pgb.FocusScales = new PointF(0.96f, 0.92f);
            g.FillPath(pgb, this.path);
        }

        protected override void OnPaintBackground(PaintEventArgs e) { }

        public void DrawArc()
        {
            this.X = this.X0;
            this.Y = this.Y0;
            this.i_Zero = 180;
            this.D++;
            this.path = new GraphicsPath();
            this.path.AddArc(this.X + this.D, this.Y + this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
            this.i_Zero += 90;
            this.X += this.XF;
            this.path.AddArc(this.X - this.D, this.Y + this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
            this.i_Zero += 90;
            this.Y += this.YF;
            this.path.AddArc(this.X - this.D, this.Y - this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
            this.i_Zero += 90;
            this.X -= this.XF;
            this.path.AddArc(this.X + this.D, this.Y - this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
            this.i_Zero += 90;
            this.Y -= this.YF;
            this.path.AddArc(this.X + this.D, this.Y + this.D, this.T, this.T, this.i_Zero, this.i_Sweep);
        }
    }
}