#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using Library35.Windows.Controls.BarChart;

namespace Library35.Windows.Controls
{
	[ToolboxBitmap(typeof (HBarChart), "BarChart.bmp"), ComplexBindingProperties("DataSource", "DataMember")]
	public partial class HBarChart : UserControl
	{
		#region Delegates
		public delegate void OnBarEvent(object sender, BarEventArgs e);
		#endregion

		#region BarSizingMode enum
		public enum BarSizingMode
		{
			Normal,
			AutoScale
		}
		#endregion

		protected HBarItems bars;
		private Bitmap bmpBackBuffer;
		private CBorderProperty border = new CBorderProperty();
		private Rectangle bounds = new Rectangle(0, 0, 0, 0);
		private CDataSourceManager dataSourceManager;
		private CDescriptionProperty description;
		private CLabelProperty label;
		private int nBarWidth;
		private int nBarsGap;
		[Browsable(false)]
		private int nLastVisitedBarIndex;
		[Browsable(false)]
		private Point ptLastTooltipMouseLoction;
		private Rectangle rectBK;
		private RectangleF rectDesc = new RectangleF(0f, 0f, 0f, 0f);
		private CShadowProperty shadow = new CShadowProperty();
		protected ToolTip tooltip;
		private CValueProperty values;

		public HBarChart()
		{
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.InitializeComponent();
			this.description = new CDescriptionProperty();
			this.label = new CLabelProperty();
			this.values = new CValueProperty();
			this.Background = new CBackgroundProperty();
			this.nBarWidth = 0x18;
			this.nBarsGap = 4;
			this.SizingMode = BarSizingMode.Normal;
			this.bars = new HBarItems();
			this.bmpBackBuffer = null;
			this.ptLastTooltipMouseLoction = new Point(0, 0);
			this.tooltip = new ToolTip();
			this.tooltip.IsBalloon = true;
			this.tooltip.InitialDelay = 0;
			this.tooltip.ReshowDelay = 0;
			this.nLastVisitedBarIndex = -1;
		}

		[Category("Bar Chart"), Browsable(true), Description("Chart background style and colors."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public CBackgroundProperty Background { get; set; }

		[Browsable(true), Category("Bar Chart")]
		public int BarsGap
		{
			get { return this.nBarsGap; }
			set { this.nBarsGap = value; }
		}

		[Category("Bar Chart"), Browsable(true)]
		public ToolTip BarTooltip
		{
			get { return this.tooltip; }
			set { this.tooltip = value; }
		}

		[Category("Bar Chart"), Browsable(true)]
		public int BarWidth
		{
			get { return this.nBarWidth; }
			set { this.nBarWidth = value; }
		}

		[Description("Settings of the border around the chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(true), Category("Bar Chart")]
		public CBorderProperty Border
		{
			get { return this.border; }
			set { this.border = value; }
		}

		[Browsable(false), Category("Bar Chart")]
		public int Count
		{
			get { return this.bars.Count; }
		}

		[Description("Defines data member of the connected data source. Chart reads data of this data member."), DefaultValue(""),
		 Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing"), Category("Bar Chart")]
		public string DataMember
		{
			get
			{
				if (this.dataSourceManager == null)
					return string.Empty;
				return this.dataSourceManager.DataMember;
			}
			set
			{
				if (value != this.DataMember)
				{
					if (this.dataSourceManager == null)
					{
						var row = new CreateChartForEachRow();
						this.dataSourceManager = new CDataSourceManager(this)
						                         {
							                         DataEventHandler = row
						                         };
					}
					this.dataSourceManager.ConnectTo(this.DataSource, value);
				}
			}
		}

		[RefreshProperties(RefreshProperties.Repaint), Category("Bar Chart"), Description("Defines Data Source to connected to."),
		 TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design"), DefaultValue((string)null), AttributeProvider(typeof (IListSource))]
		public object DataSource
		{
			get
			{
				if (this.dataSourceManager == null)
					return null;
				return this.dataSourceManager.DataSource;
			}
			set
			{
				if (value != this.DataSource)
					if (this.dataSourceManager == null)
					{
						var row = new CreateChartForEachRow();
						this.dataSourceManager = new CDataSourceManager(this)
						                         {
							                         DataEventHandler = row
						                         };
						this.dataSourceManager.ConnectTo(value, this.DataMember);
					}
					else
					{
						this.dataSourceManager.ConnectTo(value, this.DataMember);
						if (value == null)
							this.dataSourceManager = null;
					}
			}
		}

		[Browsable(false)]
		public CDataSourceManager DataSourceManager
		{
			get { return this.dataSourceManager; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(true), Category("Bar Chart"),
		 Description("Look and feel of the description line at the bottom of the chart.")]
		public CDescriptionProperty Description
		{
			get { return this.description; }
			set { this.description = value; }
		}

		[Description("A collection of chart items. A bar for each item will be drawn."), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		 Category("Bar Chart")]
		public HBarItems Items
		{
			get { return this.bars; }
			set { this.bars = value; }
		}

		[Description("Look and feel of the label at the bottom of each bar."), Browsable(true), Category("Bar Chart"),
		 DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public CLabelProperty Label
		{
			get { return this.label; }
			set { this.label = value; }
		}

		[Description("Settings of the shadows around the chart."), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Bar Chart")]
		public CShadowProperty Shadow
		{
			get { return this.shadow; }
			set { this.shadow = value; }
		}

		[Browsable(true), Category("Bar Chart")]
		public BarSizingMode SizingMode { get; set; }

		[Category("Bar Chart"), Description("Look and feel of the Value/Percent presented at the top of each bar."), Browsable(true),
		 DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public CValueProperty Values
		{
			get { return this.values; }
			set { this.values = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(true), Category("Bar Chart"),
		 Description("Mouse click event occurd while mouse is over a bar.")]
		public event OnBarEvent BarClicked;

		[Description("Mouse double click event occurd while mouse is over a bar"), Category("Bar Chart"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		 Browsable(true)]
		public event OnBarEvent BarDoubleClicked;

		[Description("Mouse is now over a bar rectangle starting from top of the chart, left of the bar and ending right of the bar and bottom of the chart."), Category("Bar Chart"),
		 Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public event OnBarEvent BarMouseEnter;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Bar Chart"), Description("Mouse just hovered out a bar."), Browsable(true)]
		public event OnBarEvent BarMouseLeave;

		internal void Add(object nullObject)
		{
			this.Add(0.0, "N/A", Color.Black);
		}

		public void Add(double dValue, string strLabel, Color colorBar)
		{
			this.bars.Add(new HBarItem(dValue, strLabel, colorBar));
		}

		private void CalculateBound(Size sizeClient)
		{
			this.bounds = new Rectangle(0, 0, sizeClient.Width, sizeClient.Height);
			if ((this.shadow.Mode == CShadowProperty.Modes.Outer) || (this.shadow.Mode == CShadowProperty.Modes.Both))
			{
				this.shadow.SetRect(this.bounds, 1);
				this.bounds.X += this.shadow.WidthOuter;
				this.bounds.Y += this.shadow.WidthOuter;
				this.bounds.Width -= 2 * this.shadow.WidthOuter;
				this.bounds.Height -= 2 * this.shadow.WidthOuter;
			}
			this.rectBK = new Rectangle(this.bounds.X, this.bounds.Y, this.bounds.Width, this.bounds.Height);
			if ((this.border != null) && this.Border.Visible)
			{
				this.border.SetRect(this.bounds);
				this.bounds.X += this.Border.Width;
				this.bounds.Y += this.Border.Width;
				this.bounds.Width -= 2 * this.Border.Width;
				this.bounds.Height -= 2 * this.Border.Width;
			}
			if ((this.shadow.Mode == CShadowProperty.Modes.Inner) || (this.shadow.Mode == CShadowProperty.Modes.Both))
				this.shadow.SetRect(this.bounds, 0);
		}

		private void CalculatePositions(Graphics gr)
		{
			var num = 0;
			var flag = (this.bars.Maximum < 0.0) || (this.bars.Minimum < 0.0);
			var flag2 = (this.bars.Maximum < 0.0) && (this.bars.Minimum < 0.0);
			var num2 = 0f;
			var height = 0f;
			var num4 = this.bounds.X + (((this.bounds.Width - (this.bars.Count * this.nBarWidth)) - ((this.bars.Count + 1) * this.nBarsGap)) / 2);
			if (((this.Description != null) && this.Description.Visible) && ((this.Description.Font != null) && (gr != null)))
			{
				this.rectDesc.X = this.bounds.X + this.nBarsGap;
				this.rectDesc.Y = (this.bounds.Bottom - (2 * this.nBarsGap)) - this.Description.Font.GetHeight(gr);
				this.rectDesc.Width = this.bounds.Size.Width - (2 * this.nBarsGap);
				this.rectDesc.Height = this.description.Font.GetHeight(gr) + (2 * this.nBarsGap);
			}
			else
				this.rectDesc = RectangleF.Empty;
			foreach (var item in this.bars)
			{
				if (flag)
				{
					item.BoundRect.X = (num4 + (num * this.nBarWidth)) + ((num + 1) * this.nBarsGap);
					item.BoundRect.Width = this.nBarWidth;
					if (flag2)
					{
						item.BoundRect.Height = this.bounds.Height - this.rectDesc.Height;
						item.BoundRect.Y = this.bounds.Y + this.nBarsGap;
					}
					else
					{
						item.BoundRect.Height = (((this.bounds.Height - this.rectDesc.Height) / 2f) + this.Label.Font.GetHeight(gr)) + (this.nBarsGap / 2);
						if (item.Value > 0.0)
							item.BoundRect.Y = this.bounds.Y + this.nBarsGap;
						else
							item.BoundRect.Y = (((this.bounds.Height - this.rectDesc.Height) / 2f) - this.Label.Font.GetHeight(gr)) - (this.nBarsGap / 2);
					}
					item.LabelRect.X = item.BoundRect.X;
					item.LabelRect.Width = item.BoundRect.Width + this.nBarsGap;
					item.LabelRect.Height = this.Label.Font.GetHeight(gr);
					if (flag2)
						item.LabelRect.Y = this.nBarsGap;
					else if (item.Value >= 0.0)
						item.LabelRect.Y = (item.BoundRect.Bottom - (this.nBarsGap / 2)) - item.LabelRect.Height;
					else
						item.LabelRect.Y = this.bounds.Y + item.BoundRect.Top;
					num2 = ((item.BoundRect.Height - (2 * this.nBarsGap)) - item.LabelRect.Height) - this.values.Font.GetHeight(gr);
					height = (float)((Math.Abs(item.Value) * num2) / this.bars.ABSMaximum);
					if (height < 0f)
						height = 0f;
					if (flag2)
					{
						item.BarRect = new RectangleF(item.BoundRect.X, item.LabelRect.Bottom + this.nBarsGap, item.BoundRect.Width, height);
						item.ValueRect.X = item.BoundRect.X;
						item.ValueRect.Y = item.BarRect.Bottom + this.nBarsGap;
						item.ValueRect.Width = item.BoundRect.Width;
						item.ValueRect.Height = this.values.Font.GetHeight(gr);
					}
					else
					{
						item.BarRect = new RectangleF(item.BoundRect.X,
							this.bounds.Y + ((item.Value > 0.0) ? (((this.bounds.Height - this.rectDesc.Height) / 2f) - height) : ((this.bounds.Height - this.rectDesc.Height) / 2f)),
							item.BoundRect.Width,
							height);
						item.ValueRect.X = item.BoundRect.X;
						item.ValueRect.Y = (item.Value > 0.0) ? (item.BarRect.Top - this.values.Font.GetHeight(gr)) : (item.BarRect.Bottom + (this.nBarsGap / 2));
						item.ValueRect.Width = item.BoundRect.Width + this.nBarsGap;
						item.ValueRect.Height = this.values.Font.GetHeight(gr);
					}
				}
				else
				{
					item.BoundRect.X = (num4 + (num * this.nBarWidth)) + ((num + 1) * this.nBarsGap);
					item.BoundRect.Y = this.bounds.Y + this.nBarsGap;
					item.BoundRect.Width = this.nBarWidth;
					item.BoundRect.Height = this.bounds.Height - this.rectDesc.Height;
					if (this.Label.Visible)
					{
						item.LabelRect.X = item.BoundRect.X;
						item.LabelRect.Y = (this.bounds.Bottom - this.rectDesc.Height) - this.Label.Font.GetHeight(gr);
						item.LabelRect.Width = item.BoundRect.Width + this.nBarsGap;
						item.LabelRect.Height = this.Label.Font.GetHeight(gr);
					}
					else
						item.LabelRect = RectangleF.Empty;
					num2 = ((item.BoundRect.Height - (2 * this.nBarsGap)) - item.LabelRect.Height) - (this.values.Visible ? this.values.Font.GetHeight(gr) : 0f);
					height = (float)((Math.Abs(item.Value) * num2) / this.bars.ABSMaximum);
					if (height < 0f)
						height = 0f;
					item.BarRect = new RectangleF(item.BoundRect.X,
						((item.BoundRect.Y + num2) - height) + (this.values.Visible ? this.values.Font.GetHeight(gr) : 0f),
						item.BoundRect.Width,
						height);
					if (this.Values.Visible)
					{
						item.ValueRect.X = item.BoundRect.X;
						item.ValueRect.Y = (item.BarRect.Top - this.values.Font.GetHeight(gr)) - (this.nBarsGap / 2);
						item.ValueRect.Width = item.BoundRect.Width + (2 * this.nBarsGap);
						item.ValueRect.Height = this.values.Font.GetHeight(gr);
					}
					else
						item.ValueRect = RectangleF.Empty;
				}
				num++;
			}
		}

		private void CreateDescFont(Graphics gr, SizeF sizeBound)
		{
			var fNewSize = sizeBound.Height / 15f;
			var num2 = (sizeBound.Width - (2 * this.nBarsGap)) / gr.MeasureString(this.description.Text, this.description.Font).Width;
			var num3 = this.description.Font.Size * num2;
			if ((fNewSize > 0f) || (num3 > 0f))
				if (fNewSize <= 0f)
					this.description.FontSetSize(num3);
				else if (num3 <= 0f)
					this.description.FontSetSize(fNewSize);
				else
					this.description.FontSetSize((fNewSize > num3) ? num3 : fNewSize);
		}

		private void CreateLabelFont(Graphics gr, SizeF sizeBar)
		{
			float width;
			var fNewSize = 100f + (sizeBar.Width / 24f);
			var num2 = 0f;
			for (var i = 0; i < this.bars.Count; i++)
			{
				width = gr.MeasureString(this.bars[i].Label, this.Label.Font).Width;
				if (width > num2)
					num2 = width;
			}
			var num5 = sizeBar.Width / num2;
			width = this.Label.Font.Size * num5;
			if ((fNewSize > 0f) || (width > 0f))
				if (fNewSize <= 0f)
					this.Label.FontSetSize(width);
				else if (width <= 0f)
					this.Label.FontSetSize(fNewSize);
				else
					this.Label.FontSetSize((fNewSize > width) ? width : fNewSize);
		}

		private void CreateValueFont(Graphics gr, SizeF sizeBar)
		{
			float width;
			var fNewSize = 100f + (sizeBar.Width / 24f);
			var num2 = 0f;
			var text = string.Empty;
			for (var i = 0; i < this.bars.Count; i++)
			{
				if (this.Values.Mode == CValueProperty.ValueMode.Digit)
					text = string.Format("{0:F1}", this.bars[i].Value);
				else if ((this.Values.Mode == CValueProperty.ValueMode.Percent) && (this.bars.ABSTotal > 0.0))
					text = (this.bars[i].Value / this.bars.ABSTotal).ToString("P1", CultureInfo.CurrentCulture);
				width = gr.MeasureString(text, this.Values.Font).Width;
				if (width > num2)
					num2 = width;
			}
			width = this.Values.Font.Size * (sizeBar.Width / num2);
			if ((fNewSize > 0f) || (width > 0f))
				if (fNewSize <= 0f)
					this.Values.FontSetSize(width);
				else if (width <= 0f)
					this.Values.FontSetSize(fNewSize);
				else
					this.Values.FontSetSize((fNewSize > width) ? width : fNewSize);
		}

		private void DrawBar(Graphics gr, HBarItem bar)
		{
			bar.Draw(gr);
			if (this.Label.Visible)
			{
				var height = this.Label.Font.GetHeight(gr);
				gr.DrawString(bar.Label, this.Label.Font, new SolidBrush(this.Label.Color), new RectangleF(bar.BarRect.X, bar.BarRect.Bottom + this.nBarsGap, bar.BarRect.Width, height));
			}
			if (this.Values.Visible)
			{
				var s = string.Empty;
				if (this.Values.Mode == CValueProperty.ValueMode.Digit)
					s = bar.Value.ToString("F1");
				else if ((this.Values.Mode == CValueProperty.ValueMode.Percent) && (this.bars.Total > 0.0))
					s = (bar.Value / this.bars.Total).ToString("P1", CultureInfo.CurrentCulture);
				var num2 = this.Values.Font.GetHeight(gr);
				gr.DrawString(s,
					this.Values.Font,
					new SolidBrush(this.Values.Color),
					new RectangleF(bar.BarRect.X, (bar.BarRect.Top - num2) - 1f, bar.BarRect.Width + (2 * this.nBarsGap), num2));
			}
		}

		private void DrawBars(Graphics gr, Size sizeChart)
		{
			if (((this.description != null) && (this.label != null)) && (this.values != null))
			{
				var nBarsGap = this.nBarsGap;
				if (this.SizingMode == BarSizingMode.AutoScale)
				{
					var nBarWidth = this.nBarWidth;
					if (this.bars.Count > 0)
					{
						this.nBarsGap = 4 + ((12 * this.bounds.Width) / ((0x159 * this.bars.Count) * 7));
						if (this.nBarsGap > 50)
							this.nBarsGap = 50;
						this.nBarWidth = (this.bounds.Width - ((this.bars.Count + 1) * this.nBarsGap)) / this.bars.Count;
						if (this.nBarWidth <= 0)
							this.nBarWidth = 0x18;
					}
					this.CreateLabelFont(gr, new Size(this.nBarWidth, 0));
					this.CreateValueFont(gr, new Size(this.nBarWidth, 0));
					this.CreateDescFont(gr, this.bounds.Size);
					this.CalculatePositions(gr);
					this.nBarWidth = nBarWidth;
				}
				else
				{
					if ((this.values.Font == null) || (this.values.Font.Size != this.values.FontDefaultSize))
						this.Values.FontReset();
					if ((this.Label.Font == null) || (this.Label.Font.Size != this.Label.FontDefaultSize))
						this.Label.FontReset();
					if ((this.Description.Font == null) || (this.Description.Font.Size != this.Description.FontDefaultSize))
						this.Description.FontReset();
					this.CalculatePositions(gr);
				}
				this.shadow.Draw(gr, this.BackColor);
				if (this.Description.Visible && (this.Description.Font != null))
				{
					var format = StringFormat.GenericDefault;
					format.LineAlignment = StringAlignment.Center;
					format.Alignment = StringAlignment.Center;
					format.Trimming = StringTrimming.Character;
					format.FormatFlags = StringFormatFlags.DirectionRightToLeft;
					gr.DrawString(this.description.Text, this.description.Font, new SolidBrush(this.description.Color), this.rectDesc, format);
				}
				foreach (var item in this.bars)
				{
					item.Draw(gr);
					if (this.Label.Visible)
						gr.DrawString(item.Label, this.Label.Font, new SolidBrush(this.Label.Color), item.LabelRect);
					if (this.Values.Visible)
					{
						var s = string.Empty;
						if (this.Values.Mode == CValueProperty.ValueMode.Digit)
							s = item.Value.ToString("F1");
						else if ((this.Values.Mode == CValueProperty.ValueMode.Percent) && (this.bars.ABSTotal != 0.0))
							s = (item.Value / this.bars.ABSTotal).ToString("P1", CultureInfo.CurrentCulture);
						gr.DrawString(s, this.Values.Font, new SolidBrush(this.Values.Color), item.ValueRect);
					}
				}
				this.border.Draw(gr);
				this.nBarsGap = nBarsGap;
			}
		}

		private void DrawChart(ref Bitmap bmp)
		{
			if (bmp == null)
				bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
			using (var graphics = Graphics.FromImage(bmp))
			{
				this.CalculateBound(bmp.Size);
				this.Background.Draw(graphics, this.rectBK);
				this.DrawBars(graphics, bmp.Size);
			}
		}

		public bool GetAt(int nIndex, out HBarItem bar)
		{
			bar = null;
			if ((nIndex < 0) || (nIndex >= this.bars.Count))
				return false;
			bar = this.bars[nIndex];
			return true;
		}

		private void HBarChart_BackColorChanged(object sender, EventArgs e)
		{
			if ((this.shadow != null) && ((this.shadow.Mode == CShadowProperty.Modes.Both) || (this.shadow.Mode == CShadowProperty.Modes.Outer)))
				this.RedrawChart();
		}

		private HBarItem HitTest(Point MousePoint, out int nIndex)
		{
			nIndex = -1;
			for (var i = 0; i < this.bars.Count; i++)
				if (this.bars[i].BoundRect.Contains(MousePoint))
				{
					nIndex = i;
					return this.bars[i];
				}
			return null;
		}

		private void HBarChart_MouseClick(object sender, MouseEventArgs e)
		{
			int num;
			var bar = this.HitTest(e.Location, out num);
			if (bar != null)
				this.RaiseClickEvent(bar, num);
		}

		private void HBarChart_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			int num;
			var bar = this.HitTest(e.Location, out num);
			if (bar != null)
				this.RaiseDoubleClickEvent(bar, num);
		}

		private void HBarChart_MouseLeave(object sender, EventArgs e)
		{
			if (!this.RectangleToScreen(this.ClientRectangle).Contains(Cursor.Position))
			{
				this.SetCurrTooltip(null);
				this.OnBarLeave();
			}
		}

		public bool InsertAt(int nIndex, double dValue, string strLabel, Color colorBar)
		{
			if ((nIndex < 0) || (nIndex >= this.bars.Count))
				return false;
			this.bars.Insert(nIndex, new HBarItem(dValue, strLabel, colorBar));
			return true;
		}

		public bool ModifyAt(int nIndex, HBarItem barNew)
		{
			if ((nIndex < 0) || (nIndex >= this.bars.Count))
				return false;
			this.bars.RemoveAt(nIndex);
			this.bars.Insert(nIndex, barNew);
			return true;
		}

		public bool ModifyAt(int nIndex, double dNewValue)
		{
			if ((nIndex < 0) || (nIndex >= this.bars.Count))
				return false;
			this.bars[nIndex].Value = dNewValue;
			return true;
		}

		private void OnBarEnter(HBarItem bar, int nIndex)
		{
			this.nLastVisitedBarIndex = nIndex;
			this.RaiseHoverInEvent(bar, nIndex);
			this.SetCurrTooltip(bar);
		}

		private void OnBarLeave()
		{
			if (this.nLastVisitedBarIndex >= 0)
			{
				this.SetCurrTooltip(null);
				this.RaiseHoverOutEvent(this.bars[this.nLastVisitedBarIndex], this.nLastVisitedBarIndex);
				this.nLastVisitedBarIndex = -1;
			}
		}

		protected override void OnBindingContextChanged(EventArgs e)
		{
			if (this.dataSourceManager != null)
				try
				{
					this.dataSourceManager.ConnectTo(this.DataSource, this.DataMember);
					return;
				}
				catch (ArgumentException)
				{
					if (!this.DesignMode)
						throw;
					this.DataMember = string.Empty;
					return;
				}
			base.OnBindingContextChanged(e);
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			if (this.ptLastTooltipMouseLoction != e.Location) // get_Location())
			{
				int num;
				this.ptLastTooltipMouseLoction = e.Location; // get_Location();
				var bar = this.HitTest(e.Location, out num);
				if (bar != null)
				{
					if (this.nLastVisitedBarIndex >= 0)
					{
						if (num != this.nLastVisitedBarIndex)
							this.OnBarEnter(bar, num);
					}
					else
						this.OnBarEnter(bar, num);
					this.SetCurrTooltip(bar);
				}
				else
					this.OnBarLeave();
			}
		}

		private void OnPaint(object sender, PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			if (this.bmpBackBuffer == null)
				this.DrawChart(ref this.bmpBackBuffer);
			if (this.bmpBackBuffer != null)
				e.Graphics.DrawImage(this.bmpBackBuffer, e.ClipRectangle, e.ClipRectangle, GraphicsUnit.Pixel);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}

		private void OnSize(object sender, EventArgs e)
		{
			this.RedrawChart();
		}

		public bool Print(bool bFitToPaper, string strDocName)
		{
			Bitmap bitmap;
			var printer = new CPrinter();
			printer.ShowOptions();
			printer.Document.DocumentName = strDocName;
			printer.FitToPaper = bFitToPaper;
			if (bFitToPaper)
				bitmap = new Bitmap(printer.Document.DefaultPageSettings.Bounds.Width, printer.Document.DefaultPageSettings.Bounds.Height);
			else
				bitmap = (Bitmap)this.bmpBackBuffer.Clone();
			this.DrawChart(ref bitmap);
			printer.BmpBuffer = bitmap;
			bool flag;
			flag = printer.Print();
			bitmap.Dispose();
			return flag;
		}

		private void RaiseClickEvent(HBarItem bar, int nIndex)
		{
			if (this.BarClicked != null)
				this.BarClicked(this, new BarEventArgs(bar, nIndex));
		}

		private void RaiseDoubleClickEvent(HBarItem bar, int nIndex)
		{
			if (this.BarDoubleClicked != null)
				this.BarDoubleClicked(this, new BarEventArgs(bar, nIndex));
		}

		private void RaiseHoverInEvent(HBarItem bar, int nIndex)
		{
			if (this.BarMouseEnter != null)
				this.BarMouseEnter(this, new BarEventArgs(bar, nIndex));
		}

		private void RaiseHoverOutEvent(HBarItem bar, int nIndex)
		{
			if (this.BarMouseLeave != null)
				this.BarMouseLeave(this, new BarEventArgs(bar, nIndex));
		}

		public void RedrawChart()
		{
			if (this.bmpBackBuffer != null)
			{
				this.bmpBackBuffer.Dispose();
				this.bmpBackBuffer = null;
			}
			this.Refresh();
		}

		public bool RemoveAt(int nIndex)
		{
			if ((nIndex < 0) || (nIndex >= this.bars.Count))
				return false;
			this.bars.RemoveAt(nIndex);
			return true;
		}

		private void SetCurrTooltip(HBarItem bar)
		{
			if (bar == null)
			{
				this.tooltip.Hide(this);
				this.tooltip.RemoveAll();
			}
			else
			{
				var str = string.Empty;
				var str2 = string.Empty;
				if (this.bars.Total > 0.0)
					str2 = (bar.Value / this.bars.Total).ToString("P2", CultureInfo.CurrentCulture);
				str = string.Format("{0}\r\n{1}", bar.Value, str2);
				if (Environment.OSVersion.Version.Major != 6)
				{
					this.tooltip.Hide(this);
					this.tooltip.RemoveAll();
				}
				this.tooltip.ToolTipTitle = bar.Label;
				this.tooltip.SetToolTip(this, str);
			}
		}
	}
}