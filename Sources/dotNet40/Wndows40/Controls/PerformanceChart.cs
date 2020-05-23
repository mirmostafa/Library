#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Library40.Helpers;
using Library40.Win.EventsArgs;

namespace Library40.Win.Controls
{
	public enum ScaleMode
	{
		/// <summary>
		///     Absolute Scale Mode: مقادیر بین صفر تا صد را قبول می کند و نمایش می دهد
		/// </summary>
		Absolute,
		/// <summary>
		///     Relative Scale Mode: تمام مقادیر را قبول می کند و نمایش می دهد
		/// </summary>
		Relative
	}

	public enum TimerMode
	{
		/// <summary>
		///     Chart is refreshed when a value is added
		/// </summary>
		Disabled,
		/// <summary>
		///     Chart is refreshed every <c>TimerInterval</c> milliseconds, adding all values
		///     in the queue to the chart. If there are no values in the queue, a 0 (zero) is added
		/// </summary>
		Simple,
		/// <summary>
		///     Chart is refreshed every <c>TimerInterval</c> milliseconds, adding an average of
		///     all values in the queue to the chart. If there are no values in the queue,
		///     0 (zero) is added
		/// </summary>
		SynchronizedAverage,
		/// <summary>
		///     Chart is refreshed every <c>TimerInterval</c> milliseconds, adding the sum of
		///     all values in the queue to the chart. If there are no values in the queue,
		///     0 (zero) is added
		/// </summary>
		SynchronizedSum
	}

	public sealed partial class PerformanceChart : UserControl
	{
		#region *** Constants ***
		// Keep only a maximum MAX_VALUE_COUNT amount of values; This will allow
		// Draw a background grid with a fixed line spacing
		private const int GRID_SPACING = 16;
		private const int MAX_VALUE_COUNT = 512;
		#endregion

		public PerformanceChart()
		{
			this.InitializeComponent();
			this._PerfChartStyle = new PerfChartStyle();

			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			// Redraw when resized
			this.SetStyle(ControlStyles.ResizeRedraw, true);

			this.Font = SystemInformation.MenuFont;
		}

		#region *** Properties ***
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("Appearance and Style")]
		public PerfChartStyle PerfChartStyle
		{
			get { return this._PerfChartStyle; }
			set { this._PerfChartStyle = value; }
		}

		[DefaultValue(typeof (Border3DStyle), "Sunken"), Description("BorderStyle"), Category("Appearance")]
		public new Border3DStyle BorderStyle
		{
			get { return this._B3Dstyle; }
			set
			{
				this._B3Dstyle = value;
				this.Invalidate();
			}
		}

		[DefaultValue(true)]
		public Boolean RaiseCrisisEvent { get; set; }

		public ScaleMode ScaleMode
		{
			get { return this._ScaleMode; }
			set { this._ScaleMode = value; }
		}

		public TimerMode TimerMode
		{
			get { return this._TimerMode; }
			set
			{
				if (value == TimerMode.Disabled)
				{
					// Stop and append only when changed
					if (this._TimerMode != TimerMode.Disabled)
					{
						this._TimerMode = value;

						this.tmrRefresh.Stop();
						// If there are any values in the queue, append them
						this.ChartAppendFromQueue();
					}
				}
				else
				{
					this._TimerMode = value;
					this.tmrRefresh.Start();
				}
			}
		}

		public int TimerInterval
		{
			get { return this.tmrRefresh.Interval; }
			set
			{
				if (value < 15)
					throw new ArgumentOutOfRangeException("TimerInterval", value, "The Timer interval must be greater then 15");
				this.tmrRefresh.Interval = value;
			}
		}
		#endregion

		#region *** Public Methods ***
		private decimal _MaxValue;

		[DefaultValue(0)]
		public decimal Value { get; private set; }

		public decimal MaxValue
		{
			get { return this._MaxValue > 0 ? this._MaxValue : 100; }
			set { this._MaxValue = value > 0 ? value : 100; }
		}

		public string AverageValue
		{
			get { return this._AverageValue.ToString(); }
		}

		[DefaultValue(false)]
		public Boolean RaiseException { get; set; }

		public void Clear()
		{
			this._DrawValues.Clear();
			this.Invalidate();
		}

		public event EventHandler<AddedValueEventsArgs> AddedValue;
		public event EventHandler<CrossingCriticalValueEventsArgs> CrossingCriticalValue;

		public event EventHandler<OccurExceptionEventsArgs> OccurException;

		public void AddValue(decimal value, string description)
		{
			if (this._ScaleMode == ScaleMode.Absolute && value > this.MaxValue)
			{
				if (this.RaiseException)
					throw new Exception(String.Format("Values greater then {0} not allowed in ScaleMode: Absolute ({1})", this.MaxValue, value));
				this.OccurException.Raise(this, new OccurExceptionEventsArgs("Values greater then {0} not allowed in ScaleMode: Absolute ({1})"));
				return;
			}

			this.AddedValue.Raise(this, new AddedValueEventsArgs(value, this.Value));
			this.Value = value;
			if (this.Value > this.CrisisValue && this.RaiseCrisisEvent)
				this.CrossingCriticalValue.Raise(this, new CrossingCriticalValueEventsArgs(this.CrisisValue, this.Value));
			switch (this._TimerMode)
			{
				case TimerMode.Disabled:
					this.ChartAppend(value);
					this.Invalidate();
					break;
				case TimerMode.Simple:
				case TimerMode.SynchronizedAverage:
				case TimerMode.SynchronizedSum:
					this.AddValueToQueue(value);
					break;
				default:
					throw new Exception(String.Format("Unsupported TimerMode: {0}", this._TimerMode));
			}
		}

		public void AddValue(decimal value)
		{
			if (this._ScaleMode == ScaleMode.Absolute && value > this.MaxValue)
			{
				if (this.RaiseException)
					throw new Exception(String.Format("Values greater then {0} not allowed in ScaleMode: Absolute ({1})", this.MaxValue, value));
				this.OccurException.Raise(this, new OccurExceptionEventsArgs("Values greater then {0} not allowed in ScaleMode: Absolute ({1})"));
				return;
			}
			this.AddedValue.Raise(this, new AddedValueEventsArgs(value, this.Value));
			this.Value = value;
			if (this.Value > this.CrisisValue && this.RaiseCrisisEvent)
				this.CrossingCriticalValue.Raise(this, new CrossingCriticalValueEventsArgs(this.CrisisValue, this.Value));
			switch (this._TimerMode)
			{
				case TimerMode.Disabled:
					this.ChartAppend(value);
					this.Invalidate();
					break;
				case TimerMode.Simple:
				case TimerMode.SynchronizedAverage:
				case TimerMode.SynchronizedSum:
					this.AddValueToQueue(value);
					break;
				default:
					throw new Exception(String.Format("Unsupported TimerMode: {0}", this._TimerMode));
			}
		}
		#endregion

		#region *** Private Methods: Common ***
		private void AddValueToQueue(decimal value)
		{
			this._WaitingValues.Enqueue(value);
		}

		private void ChartAppend(decimal value)
		{
			this._DrawValues.Insert(0, Math.Max(value, 0));

			if (this._DrawValues.Count > MAX_VALUE_COUNT)
				this._DrawValues.RemoveAt(MAX_VALUE_COUNT);

			this._GridScrollOffset += valueSpacing;
			if (this._GridScrollOffset > GRID_SPACING)
				this._GridScrollOffset = this._GridScrollOffset % GRID_SPACING;
		}

		private void ChartAppendFromQueue()
		{
			if (this._WaitingValues.Count > 0)
			{
				if (this._TimerMode == TimerMode.Simple)
					while (this._WaitingValues.Count > 0)
						this.ChartAppend(this._WaitingValues.Dequeue());
				else if (this._TimerMode == TimerMode.SynchronizedAverage || this._TimerMode == TimerMode.SynchronizedSum)
				{
					var appendValue = Decimal.Zero;
					var valueCount = this._WaitingValues.Count;

					while (this._WaitingValues.Count > 0)
						appendValue += this._WaitingValues.Dequeue();

					if (this._TimerMode == TimerMode.SynchronizedAverage)
						appendValue = appendValue / valueCount;
					this.ChartAppend(appendValue);
				}
			}
			else
				this.ChartAppend(Decimal.Zero);

			this.Invalidate();
		}

		private int CalcVerticalPosition(decimal value)
		{
			var result = Decimal.Zero;

			if (this._ScaleMode == ScaleMode.Absolute)
				result = value * this.Height / this._MaxValue;
			else if (this._ScaleMode == ScaleMode.Relative)
				result = (this._CurrentMaxValue > 0) ? (value * this.Height / this._CurrentMaxValue) : 0;

			result = this.Height - result;

			return Convert.ToInt32(Math.Round(result));
		}

		private decimal GetHighestValueForRelativeMode()
		{
			decimal maxValue = 0;

			for (var i = 0; i < this._VisibleValues; i++)
				if (this._DrawValues[i] > maxValue)
					maxValue = this._DrawValues[i];

			return maxValue;
		}
		#endregion

		#region *** Private Methods: Drawing ***
		private void DrawChart(Graphics g)
		{
			this._VisibleValues = Math.Min(this.Width / valueSpacing, this._DrawValues.Count);

			if (this._ScaleMode == ScaleMode.Relative)
				this._CurrentMaxValue = this.GetHighestValueForRelativeMode();

			var previousPoint = new Point(this.Width + valueSpacing, this.Height);
			var currentPoint = new Point();

			if (this._VisibleValues > 0 && this._PerfChartStyle.ShowAverageLine)
			{
				this._AverageValue = 0;
				this.DrawAverageLine(g);
			}

			for (var i = 0; i < this._VisibleValues; i++)
			{
				currentPoint.X = previousPoint.X - valueSpacing;
				currentPoint.Y = this.CalcVerticalPosition(this._DrawValues[i]);
				g.DrawLine(this._PerfChartStyle.ChartLinePen.Pen, previousPoint, currentPoint);
				previousPoint = currentPoint;
			}

			if (this._ScaleMode == ScaleMode.Relative)
			{
				var sb = new SolidBrush(this._PerfChartStyle.ChartLinePen.Color);
				g.DrawString(this._CurrentMaxValue.ToString(), this.Font, sb, 4.0f, 2.0f);
			}

			if (this._PerfChartStyle.ShowCrisisLine)
				g.DrawLine(this._PerfChartStyle.CrisisLine.Pen, 0, this.CalcVerticalPosition(this.CrisisValue), this.Width, this.CalcVerticalPosition(this.CrisisValue));
			ControlPaint.DrawBorder3D(g, 0, 0, this.Width, this.Height, this._B3Dstyle);
		}

		private void DrawAverageLine(Graphics g)
		{
			for (var i = 0; i < this._VisibleValues; i++)
				this._AverageValue += this._DrawValues[i];

			this._AverageValue = this._AverageValue / this._VisibleValues;

			var verticalPosition = this.CalcVerticalPosition(this._AverageValue);
			g.DrawLine(this._PerfChartStyle.AvgLinePen.Pen, 0, verticalPosition, this.Width, verticalPosition);
		}

		private void DrawBackgroundAndGrid(Graphics g)
		{
			var baseRectangle = new Rectangle(0, 0, this.Width, this.Height);
			using (
				Brush gradientBrush = new LinearGradientBrush(baseRectangle,
					this._PerfChartStyle.BackgroundColorTop,
					this._PerfChartStyle.BackgroundColorBottom,
					LinearGradientMode.Vertical))
				g.FillRectangle(gradientBrush, baseRectangle);

			if (this._PerfChartStyle.ShowVerticalGridLines)
				for (var i = this.Width - this._GridScrollOffset; i >= 0; i -= GRID_SPACING)
					g.DrawLine(this._PerfChartStyle.VerticalGridPen.Pen, i, 0, i, this.Height);

			if (this._PerfChartStyle.ShowHorizontalGridLines)
				for (var i = 0; i < this.Height; i += GRID_SPACING)
					g.DrawLine(this._PerfChartStyle.HorizontalGridPen.Pen, 0, i, this.Width, i);
		}
		#endregion

		#region *** Overrides ***
		/// Override OnPaint method
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			// Enable AntiAliasing, if needed
			if (this._PerfChartStyle.AntiAliasing)
				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			this.DrawBackgroundAndGrid(e.Graphics);
			this.DrawChart(e.Graphics);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			this.Invalidate();
		}
		#endregion

		#region *** Event Handlers ***
		private void tmrRefresh_Tick(object sender, EventArgs e)
		{
			if (this.DesignMode)
				return;

			this.ChartAppendFromQueue();
		}
		#endregion

		[DefaultValue(50)]
		public decimal CrisisValue { get; set; }

		#region *** Member Variables ***
		// Amount of currently visible values (calculated from control width and value spacing)
		// Horizontal value space in Pixels
		private const int valueSpacing = 5;
		private readonly List<decimal> _DrawValues = new List<decimal>(MAX_VALUE_COUNT);
		// Value queue for Timer Modes

		private readonly Queue<decimal> _WaitingValues = new Queue<decimal>();
		// The currently highest displayed value, required for Relative Scale Mode
		// The current average value
		private decimal _AverageValue;
		// Border Style
		private Border3DStyle _B3Dstyle = Border3DStyle.Sunken;
		private decimal _CurrentMaxValue;
		// Offset value for the scrolling grid
		private int _GridScrollOffset;
		private PerfChartStyle _PerfChartStyle;
		// Scale mode for value aspect ratio
		private ScaleMode _ScaleMode = ScaleMode.Absolute;
		// Timer Mode
		private TimerMode _TimerMode;
		private int _VisibleValues;
		// List of stored values
		#endregion
	}
}