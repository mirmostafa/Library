using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Mohammad.Win.Controls
{
    [DefaultProperty("BlockSize")]
    public partial class ProgressDisk : UserControl
    {
        private readonly GraphicsPath _BackgroundPath1 = new GraphicsPath();
        private readonly GraphicsPath _BackgroundPath2 = new GraphicsPath();
        private readonly GraphicsPath _FreGroundPath = new GraphicsPath();
        private readonly GraphicsPath _ValuePath = new GraphicsPath();
        private Timer _Timer;
        private int _Value;
        private Color activeForeColor1 = Color.Blue;
        private Color activeForeColor2 = Color.LightBlue;
        private Color backgroundColor = Color.White;
        private float blockRatio = .4f;
        private ProgressDiskBlockSize bs = ProgressDiskBlockSize.Small;
        private Color inactiveForeColor1 = Color.Green;
        private Color inactiveForeColor2 = Color.LightGreen;
        private Region region = new Region();
        private int size = 50;

        /// <summary>
        ///     Current progress value
        /// </summary>
        [DefaultValue(0)]
        public int Value
        {
            get { return this._Value; }
            set
            {
                this._Value = value;
                this.Render();
            }
        }

        /// <summary>
        ///     The color of background
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        public Color BackgroundColor
        {
            get { return this.backgroundColor; }
            set
            {
                this.backgroundColor = value;
                this.Render();
            }
        }

        /// <summary>
        ///     The color of the start of active foreground.
        /// </summary>
        [DefaultValue(typeof(Color), "Blue")]
        public Color ActiveForeColor1
        {
            get { return this.activeForeColor1; }
            set
            {
                this.activeForeColor1 = value;
                this.Render();
            }
        }

        /// <summary>
        ///     The color of the end of active foreground.
        /// </summary>
        [DefaultValue(typeof(Color), "LightBlue")]
        public Color ActiveForeColor2
        {
            get { return this.activeForeColor2; }
            set
            {
                this.activeForeColor2 = value;
                this.Render();
            }
        }

        /// <summary>
        ///     The color of the start of inactive foreground.
        /// </summary>
        [DefaultValue(typeof(Color), "Green")]
        public Color InactiveForeColor1
        {
            get { return this.inactiveForeColor1; }
            set
            {
                this.inactiveForeColor1 = value;
                this.Render();
            }
        }

        /// <summary>
        ///     The color of the end of inactive foreground.
        /// </summary>
        [DefaultValue(typeof(Color), "LightGreen")]
        public Color InactiveForeColor2
        {
            get { return this.inactiveForeColor2; }
            set
            {
                this.inactiveForeColor2 = value;
                this.Render();
            }
        }

        /// <summary>
        ///     The size of square
        /// </summary>
        [DefaultValue(50)]
        public int SquareSize
        {
            get { return this.size; }
            set
            {
                this.size = value;
                this.Size = new Size(this.size, this.size);
            }
        }

        /// <summary>
        ///     The size of each block
        /// </summary>
        [DefaultValue(typeof(ProgressDiskBlockSize), "Small")]
        public ProgressDiskBlockSize BlockSize
        {
            get { return this.bs; }
            set
            {
                this.bs = value;
                switch (this.bs)
                {
                    case ProgressDiskBlockSize.XSmall:
                        this.blockRatio = 0.49f;
                        break;
                    case ProgressDiskBlockSize.Small:
                        this.blockRatio = 0.4f;
                        break;
                    case ProgressDiskBlockSize.Medium:
                        this.blockRatio = 0.3f;
                        break;
                    case ProgressDiskBlockSize.Large:
                        this.blockRatio = 0.2f;
                        break;
                    case ProgressDiskBlockSize.XLarge:
                        this.blockRatio = 0.1f;
                        break;
                    case ProgressDiskBlockSize.XXLarge:
                        this.blockRatio = 0.01f;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        ///     The count of slices
        /// </summary>
        [DefaultValue(12)]
        public int SliceCount { get; set; }

        /// <summary>
        ///     Creates a new instance
        /// </summary>
        public ProgressDisk()
        {
            this.InitializeComponent();
            this.Render();
        }

        private void timer_Tick(object sender, EventArgs e) { this.Value = (this.Value + 1) % this.SliceCount; }

        /// <summary>
        ///     Draws the progress disk
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            this.region = new Region(this.ClientRectangle);
            if (this.backgroundColor == Color.Transparent)
            {
                this.region.Exclude(this._BackgroundPath2);
                this.Region = this.region;
            }

            e.Graphics.FillPath(new SolidBrush(this.backgroundColor), this._BackgroundPath1);
            e.Graphics.FillPath(
                new LinearGradientBrush(new Rectangle(0, 0, this.size, this.size), this.inactiveForeColor1, this.inactiveForeColor2, this._Value * 360 / 12, true),
                this._ValuePath);
            e.Graphics.FillPath(
                new LinearGradientBrush(new Rectangle(0, 0, this.size, this.size), this.activeForeColor1, this.activeForeColor2, this._Value * 360 / 12, true),
                this._FreGroundPath);
            e.Graphics.FillPath(new SolidBrush(this.backgroundColor), this._BackgroundPath2);

            base.OnPaint(e);
        }

        private void Render()
        {
            this._BackgroundPath1.Reset();
            this._BackgroundPath2.Reset();
            this._ValuePath.Reset();
            this._FreGroundPath.Reset();
            this._BackgroundPath1.AddPie(new Rectangle(0, 0, this.size, this.size), 0, 360);

            //just in case...
            if (this.SliceCount == 0)
                this.SliceCount = 12;

            float sliceAngle = 360 / this.SliceCount;
            var sweepAngle = sliceAngle - 5;
            for (var i = 0; i < this.SliceCount; i++)
                if (this._Value != i)
                    this._ValuePath.AddPie(0, 0, this.size, this.size, i * sliceAngle, sweepAngle);
            this._BackgroundPath2.AddPie(this.size / 2 - this.size * this.blockRatio,
                this.size / 2 - this.size * this.blockRatio,
                this.blockRatio * 2 * this.size,
                this.blockRatio * 2 * this.size,
                0,
                360);
            this._FreGroundPath.AddPie(new Rectangle(0, 0, this.size, this.size), this._Value * sliceAngle, sweepAngle);
            this.Invalidate();
        }

        /// <summary>
        ///     Re-calculates the boarders.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            this.size = Math.Max(this.Width, this.Height);
            this.Size = new Size(this.size, this.size);
            this.Render();
            base.OnSizeChanged(e);
        }

        /// <summary>
        ///     Re-calculates the boarders.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            this.size = Math.Max(this.Width, this.Height);
            this.Size = new Size(this.size, this.size);
            this.Render();
            base.OnResize(e);
        }

        #region Interval

        private int _Interval = 100;

        /// <summary>
        ///     Auto-Progressive interval in mili-second
        /// </summary>
        [DefaultValue(100)]
        public int Interval
        {
            get { return this._Interval; }
            set
            {
                this._Interval = value;
                if (this._Timer != null)
                    this._Timer.Interval = value;
            }
        }

        #endregion

        #region AutoProgress

        private bool _AutoProgress;

        /// <summary>
        ///     Moves on, automatically.
        /// </summary>
        [DefaultValue(false)]
        public bool AutoProgress
        {
            get { return this._AutoProgress; }
            set
            {
                if (this._AutoProgress == value)
                    return;
                this._AutoProgress = value;
                if (this._AutoProgress)
                {
                    if (this._Timer == null)
                    {
                        this._Timer = new Timer();
                        this._Timer.Interval = this.Interval;
                        this._Timer.Tick += this.timer_Tick;
                    }
                    this._Timer.Start();
                }
                else
                {
                    this._Timer.Stop();
                }
            }
        }

        #endregion
    }
}