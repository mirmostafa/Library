using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Mohammad.Win.Forms.Internals;

namespace Mohammad.Win.Controls
{
    public class RibbonButton : Button
    {
        private readonly int t_end = 100;
        //Timer

        private readonly Timer timer1 = new Timer();
        private readonly Timer timer2 = new Timer();
        private Image _img;
        private Image _img_Enable;
        private Image _img_fad;
        private Color _TextColor = Color.Black;
        private Image _toshow;

        [Localizable(true)]
        private string _Word1 = "";

        [Localizable(true)]
        private string _Word2 = "";

        //Fading
        private bool b_fad;
        private Image currentImage;
        private int i_fad; //0 nothing, 1 entering, 2 leaving
        private int i_value = 255; //Level of transparency
        //InfoForm
        private InfoForm info;
        private string s_filename;
        private string s_folder;
        private int t;
        public bool ShowInfoForm { get; set; } = true;
        //Properties
        public Image img_on { get; set; }

        public Image img_Enable
        {
            get { return this._img_Enable; }
            set
            {
                this._img_Enable = value;
                this.Image = this._img_Enable;
            }
        }

        public Image img_click { get; set; }
        public Image img_back { get; set; }

        public Image img
        {
            get { return this._img; }
            set
            {
                this._img = value;
                this.Image = this._img;
                this.currentImage = value;
            }
        }

        public string folder
        {
            get { return this.s_folder; }
            set
            {
                if (value != null)
                    if (value[value.Length - 1] != '\\')
                        this.s_folder = value + "\\";
                    else
                        this.s_folder = value;
            }
        }

        public string filename
        {
            get { return this.s_filename; }
            set
            {
                this.s_filename = value;

                if ((this.s_folder != null) & (this.s_filename != null))
                {
                    this._img = Image.FromFile(this.s_folder + this.s_filename);
                    this.Image = this._img;
                }
            }
        }

        public Font InfoFont { get; set; } = new Font("tahoma", 9);
        public string InfoTitle { get; set; } = "";
        public string InfoComment { get; set; } = "";
        public string InfoImage { get; set; } = "";
        public Color InfoColor { get; set; } = Color.FromArgb(201, 217, 239);
        public bool isChecked { get; set; }
        public bool CheckedButton { get; set; }
        public Image CheckedButtonImage { get; set; }
        public bool TwoWord { get; set; }

        [Localizable(true)]
        public string Word1 { get { return this._Word1; } set { this._Word1 = value; } }

        [Localizable(true)]
        public string Word2 { get { return this._Word2; } set { this._Word2 = value; } }

        //Constructor
        public RibbonButton()
        {
            this.SetStyle(
                ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer,
                true);

            this.BackColor = Color.Transparent;
            this.FlatStyle = FlatStyle.Flat;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.FlatAppearance.BorderSize = 0;
            this.TextAlign = ContentAlignment.BottomCenter;
            this.ImageAlign = ContentAlignment.TopCenter;
            this.FlatAppearance.BorderColor = Color.FromArgb(100, 255, 255, 255);
            this.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 255, 255, 255);
            this.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 255, 255, 255);
            this._toshow = this.img_back;
            this.timer1.Interval = 10;
            this.timer1.Tick += this.timer1_Tick;
            this.timer2.Interval = 10;
            this.timer2.Tick += this.timer2_Tick;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (this._img_Enable != null)
                this.currentImage = this.Enabled ? this._img : this._img_Enable;
            base.OnEnabledChanged(e);
        }

        //Methods
        public void PaintBackground()
        {
            if (this.b_fad)
            {
                var _img_temp = new object();
                if (this.i_fad == 1)
                    _img_temp = this.img_on.Clone();
                else if (this.i_fad == 2)
                    _img_temp = this.img_back.Clone();
                this._img_fad = (Image) _img_temp;
                var _grf = Graphics.FromImage(this._img_fad);
                var brocha = new SolidBrush(Color.FromArgb(this.i_value, 255, 255, 255));
                _grf.FillRectangle(brocha, 0, 0, this._img_fad.Width, this._img_fad.Height);
                this.BackgroundImage = this._img_fad;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (this.t < this.t_end)
            {
                this.t++;
            }
            else
            {
                this.timer2.Stop();
                this.t = 0;
                if (this.ShowInfoForm)
                    this.ShowInfo();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (this.i_fad)
            {
                case 1:
                {
                    if (this.i_value == 0)
                        this.i_value = 255;
                    if (this.i_value > -1)
                    {
                        this.PaintBackground();
                        this.i_value -= 10;
                    }
                    else
                    {
                        this.i_value = 0;
                        this.PaintBackground();
                        this.timer1.Stop();
                    }
                    break;
                }
                case 2:
                {
                    if (this.i_value == 0)
                        this.i_value = 255;
                    if (this.i_value > -1)
                    {
                        this.PaintBackground();
                        this.i_value -= 10;
                    }
                    else
                    {
                        this.i_value = 0;
                        this.PaintBackground();
                        this.timer1.Stop();
                    }
                    break;
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (this.isChecked)
            {
                if (!this.CheckedButton)
                    if (this.b_fad)
                    {
                        this.i_fad = 1;
                        this.timer1.Start();
                    }
                    else
                    {
                        this.BackgroundImage = this.img_on;
                        this._toshow = this.img_on;
                    }
            }
            else if (this.b_fad)
            {
                this.i_fad = 1;
                this.timer1.Start();
            }
            else
            {
                this.BackgroundImage = this.img_on;
                this._toshow = this.img_on;
            }
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (this.isChecked)
            {
                if (!this.CheckedButton)
                {
                    if (this.b_fad)
                    {
                        this.i_fad = 2;
                        this.timer1.Start();
                    }
                    else
                    {
                        this.BackgroundImage = this.img_back;
                        this._toshow = this.img_back;
                    }

                    //Close the info form
                    if (this.info != null)
                        this.info.Close();
                    this.timer2.Stop();
                }
            }
            else
            {
                if (this.b_fad)
                {
                    this.i_fad = 2;
                    this.timer1.Start();
                }
                else
                {
                    this.BackgroundImage = this.img_back;
                    this._toshow = this.img_back;
                }

                //Close the info form
                if (this.info != null)
                    this.info.Close();
                this.timer2.Stop();
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (this.isChecked)
            {
                if (!this.CheckedButton)
                {
                    this.BackgroundImage = this.img_click;
                    this._toshow = this.img_click;
                }
            }
            else
            {
                this.BackgroundImage = this.img_click;
                this._toshow = this.img_click;
            }

            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (this.isChecked)
            {
                if (!this.CheckedButton)
                {
                    this.BackgroundImage = this.img_on;
                    this._toshow = this.img_on;
                }
            }
            else
            {
                this.BackgroundImage = this.img_on;
                this._toshow = this.img_on;
            }
            base.OnMouseUp(mevent);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            if (this.isChecked)
            {
                if (!this.CheckedButton)
                    this.timer2.Start();
            }
            else
            {
                this.timer2.Start();
            }
            base.OnMouseHover(e);
        }

        public void ShowInfo()
        {
            if (this.isChecked)
                if (this.CheckedButton)
                    return;
            this.info = new InfoForm();
            this.info.Title = this.InfoTitle;
            this.info.Comment = this.InfoComment;
            this.info.Picture = this.InfoImage;
            this.info.FillColor = this.InfoColor;
            this.info.Font = this.InfoFont;
            if (this.GetInfoLocation() == Side.UpLeft)
                this.info.Location = new Point(Cursor.Position.X, Application.OpenForms[0].Location.Y + this.Bottom + 80);
            else
                this.info.Location = new Point(Cursor.Position.X - this.info.Width, Application.OpenForms[0].Location.Y + this.Bottom + 80);

            this.info.Show();
        }

        public Side GetInfoLocation()
        {
            var CPX = Cursor.Position.X - Application.OpenForms[0].Location.X;
            var HSX = Application.OpenForms[0].Width / 2;
            if (CPX < HSX)
                return Side.UpLeft;
            return Side.UpRight;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //para pintar el fondo hay que tener en cuenta el 
            //desplazamiento con el control contenedor
            if (this.Parent != null)
            {
                var cstate = pevent.Graphics.BeginContainer();
                pevent.Graphics.TranslateTransform(-this.Left, -this.Top);
                var clip = pevent.ClipRectangle;
                clip.Offset(this.Left, this.Top);
                var pe = new PaintEventArgs(pevent.Graphics, clip);
                //pinta el fondo del contenedor
                this.InvokePaintBackground(this.Parent, pe);
                //pinta el resto del contenedor
                this.InvokePaint(this.Parent, pe);
                //restaura el Graphics a su estado original
                pevent.Graphics.EndContainer(cstate);
            }
            else
            {
                base.OnPaintBackground(pevent);
            }
        }

        /// 
        protected override void OnClick(EventArgs e)
        {
            if (this.isChecked)
            {
                this.CheckedButton = !this.CheckedButton;
                this.BackgroundImage = this.CheckedButton ? this.CheckedButtonImage : this.img_back;
                this._toshow = this.BackgroundImage;
            }
            base.OnClick(e);
        }

        public Bitmap SetGrayscale()
        {
            var temp = (Bitmap) this._img;
            var bmap = (Bitmap) temp.Clone();
            Color c;
            for (var i = 0; i < bmap.Width; i++)
            for (var j = 0; j < bmap.Height; j++)
            {
                c = bmap.GetPixel(i, j);
                var gray = (byte) (.299 * c.R + .587 * c.G + .114 * c.B);

                bmap.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
            }
            return (Bitmap) bmap.Clone();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            if (this.Parent != null)
            {
                var cstate = pevent.Graphics.BeginContainer();
                pevent.Graphics.TranslateTransform(-this.Left, -this.Top);
                var clip = pevent.ClipRectangle;
                clip.Offset(this.Left, this.Top);
                var pe = new PaintEventArgs(pevent.Graphics, clip);
                //pinta el fondo del contenedor
                //  InvokePaintBackground(this.Parent, pe);
                //pinta el resto del contenedor
                this.InvokePaint(this.Parent, pe);
                //restaura el Graphics a su estado original
                pevent.Graphics.EndContainer(cstate);

                var g = pevent.Graphics;

                try
                {
                    g.DrawImage(this._toshow, pevent.ClipRectangle);
                }
                catch {}

                var rect = pevent.ClipRectangle;
                var X = 4;
                try
                {
                    var newwidth = rect.Width - 8;

                    var newheigth = newwidth * this.currentImage.Height / this.currentImage.Width;
                    //var _imgpos = new Point(X, 4);
                    var _Width = this.Width / 2 - 16;
                    if (_Width < 0)
                        _Width = 0;
                    var _imgpos = new Point(_Width, 10);
                    var r = new Rectangle(_imgpos, new Size(32, 32)); //new Size(newwidth, newheigth));
                    //g.DrawImageUnscaled(_img, _imgpos);
                    g.DrawImage(this.currentImage, r);
                }
                catch {}
                var _height = this.Height / 2 + 10;
                if (this.TwoWord)
                {
                    var TwoWordPForeColor = new Pen(this.ForeColor);

                    var Word1size = g.MeasureString(this.Word1, this.Font);
                    X = rect.X + rect.Width;
                    X = (X - (int) Word1size.Width) / 2;
                    var Word1Y = rect.Y + rect.Height;
                    Word1Y = Word1Y - (int) Word1size.Height - 2;
                    var Word1pos = new Point(X, Word1Y) {Y = _height};
                    g.DrawString(this.Word1, this.Font, TwoWordPForeColor.Brush, Word1pos);

                    var Word2size = g.MeasureString(this.Word2, this.Font);
                    X = rect.X + rect.Width;
                    X = (X - (int) Word2size.Width) / 2;
                    var Word2Y = rect.Y + rect.Height;
                    Word2Y = Word2Y - (int) Word2size.Height - 2;
                    var Word2pos = new Point(X, Word2Y) {Y = _height + 10};
                    g.DrawString(this.Word2, this.Font, TwoWordPForeColor.Brush, Word2pos);
                }
                else
                {
                    var textsize = g.MeasureString(this.Text, this.Font);
                    X = rect.X + rect.Width;
                    X = (X - (int) textsize.Width) / 2;
                    var Y = rect.Y + rect.Height;
                    Y = Y - (int) textsize.Height - 2;
                    var _textpos = new Point(X, Y);
                    var PForeColor = new Pen(this.ForeColor);
                    //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    _textpos.Y = _height;
                    g.DrawString(this.Text, this.Font, PForeColor.Brush, _textpos);
                }
            }
            else
            {
                base.OnPaint(pevent);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }

        #region Side enum

        public enum Side
        {
            UpLeft,
            UpRight,
            DownLeft,
            DownRight
        }

        #endregion
    }
}