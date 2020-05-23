#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Drawing;

namespace Library40.Win.Controls.BarChart
{
	public class CreateChartForEachRow : IDataConnectionEvents
	{
		private readonly Color[] colors =
		{
			Color.FromArgb(0xff, 200, 0xff, 0xff), Color.FromArgb(0xff, 150, 200, 0xff), Color.FromArgb(0xff, 100, 100, 200), Color.FromArgb(0xff, 0xff, 60, 130),
			Color.FromArgb(0xff, 250, 200, 0xff), Color.FromArgb(0xff, 0xff, 0xff, 0), Color.FromArgb(0xff, 0xff, 0x9b, 0x37), Color.FromArgb(0xff, 150, 200, 0x9b),
			Color.FromArgb(0xff, 0xff, 0xff, 200), Color.FromArgb(0xff, 100, 150, 200), Color.FromArgb(0xff, 130, 0xeb, 250), Color.FromArgb(0xff, 150, 240, 80)
		};
		private HBarChart chart;
		private CDataConnection data;

		#region IDataConnectionEvents Members
		public void DataSource_DataBoundCompleted()
		{
			if (this.chart != null)
				this.DataSource_ResetItems();
		}

		public void DataSource_ItemAdded(int nItemIndex)
		{
			if (this.data.LastSelectedRowIndex == nItemIndex)
				this.DataSource_ResetItems();
		}

		public void DataSource_ItemDeleted(int nItemIndex)
		{
			if (((nItemIndex >= 0) && (this.chart != null)) && (nItemIndex == this.data.LastSelectedRowIndex))
			{
				this.chart.Items.Clear();
				this.chart.RedrawChart();
			}
		}

		public void DataSource_ItemUpdated(int nRowIndex, int nColIndex)
		{
			if (((nRowIndex >= 0) && (this.chart != null)) && (nRowIndex == this.data.LastSelectedRowIndex))
			{
				if (nColIndex < 0)
					for (var i = 0; i < this.data.Columns.Count; i++)
					{
						var list = (ArrayList)this.data.Rows[nRowIndex];
						if (((list != null) && (list[i] != null)) && (list[i] != Convert.DBNull))
							this.chart.ModifyAt(i, Convert.ToDouble(list[i]));
						else
							this.chart.ModifyAt(i, 0.0);
					}
				else
				{
					var dNewValue = Convert.ToDouble(((ArrayList)this.data.Rows[nRowIndex])[nColIndex]);
					this.chart.ModifyAt(nColIndex, dNewValue);
				}
				this.chart.RedrawChart();
			}
		}

		public void DataSource_ResetItems()
		{
			if ((this.data.LastSelectedRowIndex >= 0) && (this.chart != null))
			{
				this.chart.Items.Clear();
				var random = new Random(1);
				for (var i = 0; i < this.data.Columns.Count; i++)
				{
					var list = (ArrayList)this.data.Rows[this.data.LastSelectedRowIndex];
					if (((list != null) && (list[i] != null)) && (list[i] != Convert.DBNull))
						this.chart.Add(Convert.ToDouble(list[i]),
							string.IsNullOrEmpty(this.data.Columns[i].DisplayName) ? this.data.Columns[i].Name : this.data.Columns[i].DisplayName,
							this.colors[random.Next(0, this.colors.Length - 1)]);
					else
						this.chart.Add(null);
				}
				this.chart.RedrawChart();
			}
		}

		public void DataSource_SelectedRowChanged(int nPosition)
		{
			if ((nPosition >= 0) && (this.chart != null))
			{
				this.chart.Items.Clear();
				var random = new Random(1);
				for (var i = 0; i < this.data.Columns.Count; i++)
				{
					var list = (ArrayList)this.data.Rows[nPosition];
					if (((list != null) && (list[i] != null)) && (list[i] != Convert.DBNull))
						this.chart.Add(Convert.ToDouble(list[i]),
							string.IsNullOrEmpty(this.data.Columns[i].DisplayName) ? this.data.Columns[i].Name : this.data.Columns[i].DisplayName,
							this.colors[random.Next(0, this.colors.Length - 1)]);
					else
						this.chart.Add(null);
				}
				this.chart.RedrawChart();
			}
		}

		public void SetData(object chart, object dataConnection)
		{
			this.chart = chart as HBarChart;
			this.data = dataConnection as CDataConnection;
		}
		#endregion
	}
}