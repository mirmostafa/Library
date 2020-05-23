#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Library40.Win.Internals;

namespace Library40.Win.Controls
{
	public enum proChartType
	{
		Line = 0,
		Column = 1,
		Pie = 2,
		Scatter = 3
	}

	public partial class ProChart : UserControl
	{
		private static string chartType = "Line";

		internal static Brush brush1 = Brushes.Red;
		internal static Brush brush2 = Brushes.Green;
		internal static Brush brush3 = Brushes.Blue;
		internal static Brush brush4 = Brushes.Brown;
		internal static Brush brush5 = Brushes.Orange;
		internal static Brush brush6 = Brushes.Pink;
		internal static Brush brush7 = Brushes.Violet;
		internal static Brush brush8 = Brushes.Gold;
		internal static Brush brush9 = Brushes.DarkOrchid;
		internal static Brush brush10 = Brushes.YellowGreen;

		internal static Brush chartTitleColor = Brushes.Black;
		internal static Brush xAxisTitleColor = Brushes.Black;
		internal static Brush yAxisTitleColor = Brushes.Black;
		private readonly ArrayList pieNames = new ArrayList();
		private readonly ArrayList pieValues = new ArrayList();
		private readonly ArrayList values1 = new ArrayList();
		private readonly ArrayList values2 = new ArrayList();
		private readonly ArrayList values3 = new ArrayList();
		private readonly ArrayList values4 = new ArrayList();
		private readonly ArrayList values5 = new ArrayList();
		private float H;

		private bool HLines;
		private Collection<ProChartLine> ProCahrtLines = new Collection<ProChartLine>();
		private bool VLines;
		private float W;
		private string chartTitle = "";
		private float max_val;
		private float min_val;

		private Point mouseOffset;
		private float n;
		private string name1;
		private string name2;
		private string name3;
		private string name4;
		private string name5;

		private float stepx = 1;
		private float stepy = 1;

		private int verticalLineMax;
		private string xAxisTitle = "";
		private string yAxisTitle = "";

		public ProChart()
		{
			this.InitializeComponent();
		}

		[DefaultValue(proChartType.Line)]
		public proChartType ProChartType { get; set; }

		// Properties for data members of this class

		public bool VerticalLines
		{
			get { return this.VLines; }
			set { this.VLines = value; }
		}

		public bool HorizontalLines
		{
			get { return this.HLines; }
			set { this.HLines = value; }
		}

		public string ChartType
		{
			get { return chartType; }
			set { chartType = value; }
		}
		public string ChartTitle
		{
			get { return this.chartTitle; }
			set { this.chartTitle = value; }
		}
		public string XAxisTitle
		{
			get { return this.xAxisTitle; }
			set { this.xAxisTitle = value; }
		}
		public string YAxisTitle
		{
			get { return this.yAxisTitle; }
			set { this.yAxisTitle = value; }
		}

		private void MyChart_Load(object sender, EventArgs e)
		{
			this.HLines = true;
			this.VLines = false;
			switch (this.ProChartType)
			{
				case proChartType.Line:
					this.PaintLineChart();
					break;
				case proChartType.Column:
					this.PaintColumnChart();
					break;
				default:
					break;
			}

			//if (chartType == "Line")
			//    PaintLineChart();
			//else if (chartType == "Column")
			//    PaintColumnChart();
			//else if (chartType == "Pie")
			//    PaintPieChart();
			//else if (chartType == "Scatter")
			//    PaintScatterChart();

			//trackBarWidth.Value = this.Width;
			//trackBarWidth.Value = panelChart.Width;
			//trackBarHeight.Value = this.Height;
			//trackBarHeight.Value = panelChart.Height;
			this.label_Resize.MouseDown += this.label_Resize_MouseDown;
			this.label_Resize.MouseMove += this.label_Resize_MouseMove;
			this.Parent.Paint += this.Parent_Paint;
		}

		private void Parent_Paint(object sender, PaintEventArgs e)
		{
			switch (this.ProChartType)
			{
				case proChartType.Line:
					this.PaintLineChart();
					break;
				case proChartType.Column:
					this.PaintColumnChart();
					break;
				default:
					break;
			}
		}

		private void label_Resize_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				var mousePos = MousePosition;

				mousePos.Offset(this.mouseOffset.X, this.mouseOffset.Y);

				this.Width = mousePos.X - this.Parent.Location.X - this.Left - this.mouseOffset.X;
				this.Height = mousePos.Y - this.Parent.Location.Y - this.Top + this.mouseOffset.Y;

				if (chartType == "Line")
					this.PaintLineChart();
				else if (chartType == "Column")
					this.PaintColumnChart();
			}
		}

		private void label_Resize_MouseDown(object sender, MouseEventArgs e)
		{
			this.mouseOffset = new Point(e.X, -e.Y);
		}

		// Method to draw a column chart
		// Input : void
		// Output : void
		public void PaintColumnChart()
		{
			var column = 0;

			/*if (this.Width < 300)
                this.Width = 300;
            if (this.Height < 200)
                this.Height = 200;*/

			this.W = this.Width - (20 + 40 + 20 + 120);
			this.H = this.Height - 50;

			var graph = this.CreateGraphics();
			graph.Clear(Color.White);
			var penCurrent = new Pen(Color.Black, 2);
			var Rect = new Rectangle(0, 0, this.Width, this.Height);

			graph.DrawRectangle(penCurrent, Rect);

			penCurrent = new Pen(Color.Black);

			graph.DrawLine(penCurrent, new Point(65, 20), new Point(65, (int)(this.H + 20)));
			graph.DrawLine(penCurrent, new Point(65, (int)(this.H + 20)), new Point((int)(this.W + 60), (int)(this.H + 20)));

			this.stepx = 1;
			this.stepy = 1;

			this.verticalLineMax = 0;

			this.min_val = 0;
			this.max_val = 0;

			float stepValue = 1;

			foreach (var line in this.ProCahrtLines)
				foreach (var value in line.Values)
				{
					if (this.min_val > value)
						this.min_val = value;
					if (this.max_val < value)
						this.max_val = value;
				}

			stepValue = this.max_val / 10;

			this.stepy = (this.H) / 10;
			this.verticalLineMax = (int)(this.H + 20 - (10 * this.stepy));

			for (var i = 1; i <= 10; i++)
			{
				graph.DrawLine(penCurrent, new Point(63, (int)(this.H + 20 - i * this.stepy)), new Point(67, (int)(this.H + 20 - i * this.stepy)));
				if (this.max_val < 1)
					graph.DrawString(((stepValue * i)).ToString(), new Font(FontFamily.GenericSansSerif, 7), Brushes.Black, 20, (this.H + 20 - i * this.stepy) - 6);
				else
					graph.DrawString(((int)(stepValue * i)).ToString(), new Font(FontFamily.GenericSansSerif, 7), Brushes.Black, 20, (this.H + 20 - i * this.stepy) - 6);
				if (this.HLines)
				{
					var pl = new Pen(Color.LightGray);
					pl.DashStyle = DashStyle.Dash;
					graph.DrawLine(pl, new Point(65, (int)(this.H + 20 - i * this.stepy)), new Point((int)(this.W + 60), (int)(this.H + 20 - i * this.stepy)));
				}
			}

			var existDataToDisplay = false;
			foreach (var line in this.ProCahrtLines)
			{
				if (line.Values.Length <= 0)
					continue;
				this.stepx = (int)(this.W) / line.Values.Length;
				existDataToDisplay = true;
				break;
			}
			if (!existDataToDisplay)
			{
				graph.DrawString("No Data to Display", new Font(FontFamily.GenericSansSerif, 12), chartTitleColor, 60 + 20, 20 + 40);
				return;
			}

			var index = 0;
			var written = false;
			var legendTop = 20 + 20;
			foreach (var line in this.ProCahrtLines)
			{
				if (index == 0)
				{
					for (var i = 1; i <= line.Values.Length; i++)
					{
						graph.DrawLine(penCurrent, new Point((int)(i * this.stepx + 65), (int)(this.H + 20 - 2)), new Point((int)(i * this.stepx + 65), (int)(this.H + 20 + 2)));
						graph.DrawString(i.ToString(), new Font(FontFamily.GenericSansSerif, 7), Brushes.Black, (i * this.stepx + 65 - 7), (this.H + 20 + 5));
						if (this.VLines)
						{
							var pl = new Pen(Brushes.Gray, 1)
							         {
								         DashStyle = DashStyle.Dash
							         };
							graph.DrawLine(pl, new Point((int)(i * this.stepx + 65), (int)(this.H + 20)), new Point((int)(i * this.stepx + 65), this.verticalLineMax));
						}
					}

					index = -1;
				}
				if (line.Values.Length > 1)
				{
					var arrayList = new ArrayList();
					for (var i = 0; i < line.Values.Length; i++)
						arrayList.Add(line.Values[i]);
					var brush = new SolidBrush(line.Color);
					this.DrawColumn(arrayList, line.Name, brush, graph, column, ref legendTop);
					column++;
					written = true;
				}
			}
			if (written == false)
			{
				graph.DrawString("Insufficient Data to Display", new Font(FontFamily.GenericSansSerif, 12), chartTitleColor, 60 + 20, 20 + 40);
				return;
			}

			if (this.chartTitle != "")
				graph.DrawString(this.chartTitle, new Font(FontFamily.GenericSansSerif, 9), chartTitleColor, (int)((60 + this.W + 20 + 60) / 2), 2);
			if (this.xAxisTitle != "")
				graph.DrawString(this.xAxisTitle, new Font(FontFamily.GenericSansSerif, 7), xAxisTitleColor, (int)((60 + this.W + 20 + 60) / 2), 20 + this.H + 13);
			if (this.yAxisTitle != "")
			{
				var format = new StringFormat();
				format.FormatFlags = StringFormatFlags.DirectionVertical;
				graph.DrawString(this.yAxisTitle, new Font(FontFamily.GenericSansSerif, 7), yAxisTitleColor, 4, 20, format);
			}
			this.label_Resize.Left = this.Width - this.label_Resize.Width;
			this.label_Resize.Top = this.Height - this.label_Resize.Height;
		}

		// Methpod to draw columns on fixed points on a column chart
		// Input : graphics object on which we want to write, an array of values, a name for this series , a brush represents the color of the points to be drawn 
		//         and the integer value represents the height that is to be used while drawing legends of the chart and an integer value represents the width that is to be considered when drawing chrt without overlaping columns. 
		// Output : void
		public void DrawColumn(ArrayList values, string name, Brush brush, Graphics graph, int column, ref int legendTop)
		{
			var WriteLegend = false;
			if (values.Count > 1)
			{
				var v = new Point[values.Count];

				for (var i = 0; i < values.Count; i++)
				{
					v[i].X = (int)((i + 1) * this.stepx + 65);

					var proc = (100 * (float)values[i]) / (this.max_val);
					v[i].Y = (int)(this.verticalLineMax + (((this.H) * (100 - proc)) / 100));

					var start = v[i].X + (column * 7);

					graph.FillRectangle(brush, start, v[i].Y, 7, ((this.H + 20) - (this.verticalLineMax + (((this.H) * (100 - proc)) / 100))));

					if (WriteLegend == false)
					{
						var rect = new Rectangle((int)this.W + 60 + 20, legendTop, 10, 10);
						graph.FillRectangle(brush, rect);
						graph.DrawString(name, new Font(FontFamily.GenericSansSerif, 7), Brushes.Black, (this.W + 60 + 20 + 10 + 5), legendTop);
						legendTop += 15;
						WriteLegend = true;
					}
				}
			}
		}

		// Method to draw a pie chart
		// Input : void
		// Output : void

		// Method to draw a Line chart
		// Input : void
		// Output : void
		public void PaintLineChart()
		{
			this.W = this.Width - (20 + 40 + 20 + 120);
			//W = panelChart.Width - (20 + 40 + 20 + 120);
			this.H = this.Height - 50;
			//H = panelChart.Height - 50;

			var graph = this.CreateGraphics();
			//Graphics graph = panelChart.CreateGraphics();

			graph.Clear(Color.White);
			var penCurrent = new Pen(Color.Black, 2);
			var Rect = new Rectangle(0, 0, this.Width, this.Height);
			//Rectangle Rect = new Rectangle(0, 0, (int)panelChart.Width, (int)panelChart.Height);

			graph.DrawRectangle(penCurrent, Rect);

			penCurrent = new Pen(Color.Black);

			graph.DrawLine(penCurrent, new Point(65, 20), new Point(65, (int)(this.H + 20)));
			graph.DrawLine(penCurrent, new Point(65, (int)(this.H + 20)), new Point((int)(this.W + 60), (int)(this.H + 20)));

			this.stepx = 1;
			this.stepy = 1;

			this.verticalLineMax = 0;

			this.min_val = 0;
			this.max_val = 0;

			float stepValue = 1;

			foreach (var line in this.ProCahrtLines)
				foreach (var value in line.Values)
				{
					if (this.min_val > value)
						this.min_val = value;
					if (this.max_val < value)
						this.max_val = value;
				}

			stepValue = this.max_val / 10;

			this.stepy = (this.H) / 10;

			this.verticalLineMax = (int)(this.H + 20 - (10 * this.stepy));

			for (var i = 1; i <= 10; i++)
			{
				graph.DrawLine(penCurrent, new Point(63, (int)(this.H + 20 - i * this.stepy)), new Point(67, (int)(this.H + 20 - i * this.stepy)));
				if (this.max_val < 1)
					graph.DrawString(((stepValue * i)).ToString(), new Font(FontFamily.GenericSansSerif, 7), Brushes.Black, 20, (this.H + 20 - i * this.stepy) - 6);
				else
					graph.DrawString(((int)(stepValue * i)).ToString(), new Font(FontFamily.GenericSansSerif, 7), Brushes.Black, 20, (this.H + 20 - i * this.stepy) - 6);
				if (this.HLines)
				{
					var pl = new Pen(Color.LightGray);
					pl.DashStyle = DashStyle.Dash;
					graph.DrawLine(pl, new Point(65, (int)(this.H + 20 - i * this.stepy)), new Point((int)(this.W + 60), (int)(this.H + 20 - i * this.stepy)));
				}
			}

			var existDataToDisplay = false;
			foreach (var line in this.ProCahrtLines)
			{
				if (line.Values.Length <= 0)
					continue;
				this.stepx = (int)(this.W) / line.Values.Length;
				existDataToDisplay = true;
				break;
			}
			if (!existDataToDisplay)
			{
				graph.DrawString("No Data to Display", new Font(FontFamily.GenericSansSerif, 12), chartTitleColor, 60 + 20, 20 + 40);
				return;
			}

			var index = 0;
			var written = false;
			var legendTop = 20 + 20;
			foreach (var line in this.ProCahrtLines)
			{
				if (index == 0)
				{
					for (var i = 1; i <= line.Values.Length; i++)
					{
						graph.DrawLine(penCurrent, new Point((int)(i * this.stepx + 65), (int)(this.H + 20 - 2)), new Point((int)(i * this.stepx + 65), (int)(this.H + 20 + 2)));
						graph.DrawString(i.ToString(), new Font(FontFamily.GenericSansSerif, 7), Brushes.Black, (i * this.stepx + 65 - 7), (this.H + 20 + 5));
						if (this.VLines)
						{
							var pl = new Pen(Brushes.Gray, 1)
							         {
								         DashStyle = DashStyle.Dash
							         };
							graph.DrawLine(pl, new Point((int)(i * this.stepx + 65), (int)(this.H + 20)), new Point((int)(i * this.stepx + 65), this.verticalLineMax));
						}
					}

					index = -1;
				}
				if (line.Values.Length > 1)
				{
					var arrayList = new ArrayList();
					for (var i = 0; i < line.Values.Length; i++)
						arrayList.Add(line.Values[i]);
					var brush = new SolidBrush(line.Color);
					this.DrawLine(graph, arrayList, line.Name, line.Avrage.ToString(), brush, ref legendTop);
					written = true;
				}
			}
			if (written == false)
			{
				graph.DrawString("Insufficient Data to Display", new Font(FontFamily.GenericSansSerif, 12), chartTitleColor, 60 + 20, 20 + 40);
				return;
			}

			if (this.chartTitle != "")
				graph.DrawString(this.chartTitle, new Font(FontFamily.GenericSansSerif, 9), chartTitleColor, (int)((60 + this.W + 20 + 60) / 2), 2);
			if (this.xAxisTitle != "")
				graph.DrawString(this.xAxisTitle, new Font(FontFamily.GenericSansSerif, 7), xAxisTitleColor, (int)((60 + this.W + 20 + 60) / 2), 20 + this.H + 13);
			if (this.yAxisTitle != "")
			{
				var format = new StringFormat();
				format.FormatFlags = StringFormatFlags.DirectionVertical;
				graph.DrawString(this.yAxisTitle, new Font(FontFamily.GenericSansSerif, 7), yAxisTitleColor, 4, 20, format);
			}
			this.label_Resize.Left = this.Width - this.label_Resize.Width;
			this.label_Resize.Top = this.Height - this.label_Resize.Height;
		}

		// Methpod to draw lines on fixed points on a Line chart
		// Input : graphics object on which we want to write, an array of values, a name for this series , a brush represents the color of the points to be drawn 
		//         and the integer value represents the height that is to be used while drawing legends of the chart.
		// Output : void
		public void DrawLine(Graphics graph, ArrayList values, string name, Brush brush, ref int legendTop)
		{
			//graph = this.CreateGraphics();
			var pp = new Pen(brush, 1);
			var v = new Point[values.Count];
			var WriteLegend = false;

			for (var i = 0; i < values.Count; i++)
			{
				v[i].X = (int)((i + 1) * this.stepx + 65);

				var proc = (100 * (float)values[i]) / (this.max_val);
				v[i].Y = (int)(this.verticalLineMax + (((this.H) * (100 - proc)) / 100));

				//v[i].Y=(int)(H+20-proc);

				graph.FillRectangle(brush, v[i].X - 2, v[i].Y - 2, 5, 5);
				if (WriteLegend == false)
				{
					var rect = new Rectangle((int)this.W + 60 + 20, legendTop, 10, 10);
					graph.FillRectangle(brush, rect);

					graph.DrawString(name, new Font(FontFamily.GenericSansSerif, 7), Brushes.Black, (this.W + 60 + 20 + 10 + 5), legendTop);
					legendTop += 15;
					WriteLegend = true;
				}
			}

			graph.DrawLines(pp, v);
		}

		public void DrawLine(Graphics graph, ArrayList values, string name, string avrage, Brush brush, ref int legendTop)
		{
			//graph = this.CreateGraphics();
			var pp = new Pen(brush, 1);
			var v = new Point[values.Count];
			var WriteLegend = false;

			for (var i = 0; i < values.Count; i++)
			{
				v[i].X = (int)((i + 1) * this.stepx + 65);

				var proc = (100 * (float)values[i]) / (this.max_val);
				v[i].Y = (int)(this.verticalLineMax + (((this.H) * (100 - proc)) / 100));

				//v[i].Y=(int)(H+20-proc);

				graph.FillRectangle(brush, v[i].X - 2, v[i].Y - 2, 5, 5);
				if (WriteLegend == false)
				{
					var rect = new Rectangle((int)this.W + 60 + 20, legendTop, 10, 10);
					graph.FillRectangle(brush, rect);

					graph.DrawString(name + " میانگین:" + avrage, new Font(FontFamily.GenericSansSerif, 7), Brushes.Black, (this.W + 60 + 20 + 10 + 5), legendTop);
					legendTop += 15;
					WriteLegend = true;
				}
			}

			graph.DrawLines(pp, v);
		}

		// Method to draw a Scatter chart
		// Input : void
		// Output : void

		// Methpod to draw points on fixed points on a scatter chart
		// Input : graphics object on which we want to write, an array of values, a name for this series , a brush represents the color of the points to be drawn 
		//         and the integer value represents the height that is to be used while drawing legends of the chart.
		// Output : void
		public void DrawPoints(Graphics graph, ArrayList values, string name, Brush brush, ref int legendTop)
		{
			var pp = new Pen(brush, 1);
			var v = new Point[values.Count];
			var WriteLegend = false;

			for (var i = 0; i < values.Count; i++)
			{
				v[i].X = (int)((i + 1) * this.stepx + 65);

				var proc = (100 * (float)values[i]) / (this.max_val);
				v[i].Y = (int)(this.verticalLineMax + (((this.H) * (100 - proc)) / 100));

				graph.FillRectangle(brush, v[i].X - 2, v[i].Y - 2, 5, 5);
				if (WriteLegend == false)
				{
					var rect = new Rectangle((int)this.W + 60 + 20, legendTop, 10, 10);
					graph.FillRectangle(brush, rect);
					graph.DrawString(name, new Font(FontFamily.GenericSansSerif, 7), Brushes.Black, (this.W + 60 + 20 + 10 + 5), legendTop);
					legendTop += 15;
					WriteLegend = true;
				}
			}
		}

		public void ResizeToOriginal()
		{
			//if (this.Width > 320)
			//    this.Width = 320;
			//if (this.Height > 240)
			//    this.Height = 240;
		}

		// Method to load values of ArrayLists 
		// Note : These values are used while drawing charts.
		//        5 arrays of values is because the maximum number of fields used to draw the chart is 5.  
		// Input : 5 arrays of type float and 5 names for these 5 array values 
		// Output : void

		public void LoadValues(Collection<ProChartLine> proCahrtLines)
		{
			this.ProCahrtLines.Clear();
			if (proCahrtLines.Count != 0)
				this.ProCahrtLines = proCahrtLines;
		}

		public void LoadValues(float[] v1, string n1, float[] v2, string n2, float[] v3, string n3, float[] v4, string n4, float[] v5, string n5)
		{
			this.values1.Clear();
			this.values2.Clear();
			this.values3.Clear();
			this.values4.Clear();
			this.values5.Clear();

			if (v1 != null && n1 != null)
			{
				foreach (var f in v1)
					this.values1.Add(f);
				this.name1 = n1;
			}
			if (v2 != null && n2 != null)
			{
				foreach (var f in v2)
					this.values2.Add(f);
				this.name2 = n2;
			}
			if (v3 != null && n3 != null)
			{
				foreach (var f in v3)
					this.values3.Add(f);
				this.name3 = n3;
			}
			if (v4 != null && n4 != null)
			{
				foreach (var f in v4)
					this.values4.Add(f);
				this.name4 = n4;
			}
			if (v5 != null && n5 != null)
			{
				foreach (var f in v5)
					this.values5.Add(f);
				this.name5 = n5;
			}
		}

		// Method to load values for drawing pie chart
		// Note : Method LoadValues(...) will load data for Line chart , column chart and scatter chart
		//        These charts require more than 1 array of values to be drawn.
		//        But pie chart requires only 1 array of values and 1 array of names for these values.
		//        So the signature differs for loading values for pie chart and other charts.
		// Input : an array of values and an array of names for these values
		// Output : void
		public void LoadValuesForPie(float[] values, string[] names)
		{
			this.pieValues.Clear();
			this.pieNames.Clear();

			if (values != null)
				foreach (var f in values)
					this.pieValues.Add(f);
			if (names != null)
				foreach (var s in names)
					this.pieNames.Add(s);
		}

		private void trackBarWeith_Scroll(object sender, EventArgs e)
		{
			//this.Width = trackBarWidth.Value;
			//panelChart.Width = trackBarWidth.Value;            
		}

		private void trackBarHeight_Scroll(object sender, EventArgs e)
		{
			//this.Height = trackBarHeight.Value;
			//panelChart.Height = trackBarHeight.Value;
		}
	}
}