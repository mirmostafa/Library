#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;

namespace Library40.Win.Settings
{
	public class ListViewSettings : ControlSetings
	{
		private Collection<ListViewColumnSettings> _ColumnsSettings;
		public string ListViewName { get; set; }

		public Collection<ListViewColumnSettings> ColumnsSettings
		{
			get
			{
				if (this._ColumnsSettings == null)
					this._ColumnsSettings = new Collection<ListViewColumnSettings>();
				return this._ColumnsSettings;
			}
			set { this._ColumnsSettings = value; }
		}

		public View View { get; set; }

		public void Save(ListView listView)
		{
			this.ListViewName = listView.Name;
			this.View = listView.View;
			this.ColumnsSettings.Clear();
			foreach (ColumnHeader column in listView.Columns)
				this.ColumnsSettings.Add(new ListViewColumnSettings
				                         {
					                         ColumnIndex = column.Index,
					                         DisplayIndex = column.DisplayIndex,
					                         Text = column.Text,
					                         Width = column.Width
				                         });
		}

		public void Load(ListView listView)
		{
			foreach (ColumnHeader column in listView.Columns)
			{
				var columnSettings = this.ColumnsSettings.Where(cs => cs.ColumnIndex.Equals(column.Index)).FirstOrDefault();
				if (columnSettings == null)
					continue;
				column.DisplayIndex = columnSettings.DisplayIndex;
				column.Text = columnSettings.Text;
				column.Width = columnSettings.Width;
			}
		}
	}

	public class ListViewColumnSettings
	{
		public int ColumnIndex { get; set; }

		public string Text { get; set; }

		public int DisplayIndex { get; set; }

		public int Width { get; set; }
	}
}