#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Windows.Forms;

namespace Library35.Windows.Controls.BarChart
{
	public class CDataSourceManager
	{
		private readonly object owner;
		private CDataConnection data;

		public CDataSourceManager(HBarChart owner)
		{
			this.owner = owner;
		}

		public CDataConnection DataConnection
		{
			get { return this.data; }
		}

		public object DataEventHandler
		{
			get
			{
				if (this.data == null)
					return null;
				return this.data.DataEventHandler;
			}
			set
			{
				if (this.data == null)
					this.data = new CDataConnection((UserControl)this.owner, value);
				else
					this.data.DataEventHandler = value;
			}
		}

		public string DataMember
		{
			get
			{
				if (this.data == null)
					return null;
				return this.data.DataMember;
			}
		}

		public object DataSource
		{
			get { return this.data.DataSource; }
		}

		internal void ConnectTo(object dataSource, string dataMember)
		{
			if (this.data == null)
				this.data = new CDataConnection((UserControl)this.owner, null);
			this.data.SetDataSource(dataSource, dataMember);
		}
	}
}