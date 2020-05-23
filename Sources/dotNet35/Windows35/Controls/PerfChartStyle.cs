#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Library35.Windows.Controls
{
	[TypeConverter(typeof (ExpandableObjectConverter))]
	public class PerfChartStyle
	{
		private bool antiAliasing = true;
		private Color backgroundColorBottom = Color.DarkGreen;
		private Color backgroundColorTop = Color.DarkGreen;

		private bool showAverageLine = true;
		private bool showHorizontalGridLines = true;
		private bool showVerticalGridLines = true;

		public PerfChartStyle()
		{
			this.VerticalGridPen = new ChartPen();
			this.HorizontalGridPen = new ChartPen();
			this.AvgLinePen = new ChartPen();
			this.ChartLinePen = new ChartPen();
			this.CrisisLine = new ChartPen();
		}

		public bool ShowCrisisLine { get; set; }

		public bool ShowVerticalGridLines
		{
			get { return this.showVerticalGridLines; }
			set { this.showVerticalGridLines = value; }
		}

		public bool ShowHorizontalGridLines
		{
			get { return this.showHorizontalGridLines; }
			set { this.showHorizontalGridLines = value; }
		}

		public bool ShowAverageLine
		{
			get { return this.showAverageLine; }
			set { this.showAverageLine = value; }
		}

		public ChartPen VerticalGridPen { get; set; }

		public ChartPen CrisisLine { get; set; }

		public ChartPen HorizontalGridPen { get; set; }

		public ChartPen AvgLinePen { get; set; }

		public ChartPen ChartLinePen { get; set; }

		public bool AntiAliasing
		{
			get { return this.antiAliasing; }
			set { this.antiAliasing = value; }
		}

		public Color BackgroundColorTop
		{
			get { return this.backgroundColorTop; }
			set { this.backgroundColorTop = value; }
		}

		public Color BackgroundColorBottom
		{
			get { return this.backgroundColorBottom; }
			set { this.backgroundColorBottom = value; }
		}
	}

	[TypeConverter(typeof (ExpandableObjectConverter))]
	public class ChartPen
	{
		private readonly Pen pen;

		public ChartPen()
		{
			this.pen = new Pen(Color.Black);
		}

		public Color Color
		{
			get { return this.pen.Color; }
			set { this.pen.Color = value; }
		}

		public DashStyle DashStyle
		{
			get { return this.pen.DashStyle; }
			set { this.pen.DashStyle = value; }
		}

		public float Width
		{
			get { return this.pen.Width; }
			set { this.pen.Width = value; }
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Pen Pen
		{
			get { return this.pen; }
		}
	}
}