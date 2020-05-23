#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace Library35.Windows.Controls.BarChart
{
	public class CDataConnection
	{
		#region ConnectionStates enum
		public enum ConnectionStates
		{
			None,
			Initializing,
			Initialized
		}
		#endregion

		#region DataSourceStates enum
		public enum DataSourceStates
		{
			None,
			Initializing,
			Initialized
		}
		#endregion

		private readonly CDataColumnCollection columns;
		private readonly UserControl parent;
		private readonly ArrayList rows;
		private ConnectionStates connectionState;
		protected CurrencyManager currencyManager;
		private object dataEventHandler;
		private string dataMember;
		private object dataSource;
		private DataSourceStates dataSourceState;
		private int nLastSelectedRowIndex;

		public CDataConnection()
		{
			this.dataMember = string.Empty;
			this.dataEventHandler = null;
			this.parent = null;
			this.columns = new CDataColumnCollection();
			this.rows = new ArrayList();
		}

		public CDataConnection(UserControl parent, object dataEventHandler)
			: this()
		{
			this.parent = parent;
			this.SetEventHandler(dataEventHandler);
		}

		public CDataColumnCollection Columns
		{
			get { return this.columns; }
		}

		public ConnectionStates ConnectionState
		{
			get { return this.connectionState; }
		}

		public CurrencyManager CurrencyManager
		{
			get { return this.currencyManager; }
		}

		public object DataEventHandler
		{
			get { return this.dataEventHandler; }
			set { this.SetEventHandler(value); }
		}

		public string DataMember
		{
			get { return this.dataMember; }
		}

		public object DataSource
		{
			get { return this.dataSource; }
		}

		public DataSourceStates DataSourceState
		{
			get { return this.dataSourceState; }
		}

		public int LastSelectedRowIndex
		{
			get { return this.nLastSelectedRowIndex; }
		}

		public ArrayList Rows
		{
			get { return this.rows; }
		}

		private void AddItem(int itemIndex)
		{
			if ((this.columns == null) || (this.columns.Count == 0))
				this.RenewAllData();
			if ((this.columns != null) && (this.columns.Count != 0))
			{
				var itemProperties = this.currencyManager.GetItemProperties();
				if (itemProperties != null)
				{
					var list = new ArrayList(this.columns.Count);
					for (var i = 0; i < this.columns.Count; i++)
						list.Add(itemProperties[i].GetValue(this.currencyManager.List[itemIndex]));
					this.rows.Insert(itemIndex, list);
					var dataEventHandler = this.dataEventHandler as IDataConnectionEvents;
					if (dataEventHandler != null)
						dataEventHandler.DataSource_ItemAdded(itemIndex);
				}
			}
		}

		private void CurrencyManager_ListChanged(object sender, ListChangedEventArgs e)
		{
			switch (e.ListChangedType)
			{
				case ListChangedType.ItemAdded:
					this.AddItem(e.NewIndex);
					return;

				case ListChangedType.ItemDeleted:
					this.DeleteItem(e.NewIndex);
					return;

				case ListChangedType.ItemChanged:
					this.UpdateItem(e.NewIndex);
					return;
			}
			this.ResetItems();
		}

		private void CurrencyManager_PositionChanged(object sender, EventArgs e)
		{
			this.OnSelecltedRowChanged();
		}

		private void DataSource_Initialized(object sender, EventArgs e)
		{
			var dataSource = this.dataSource as ISupportInitializeNotification;
			if (dataSource != null)
				dataSource.Initialized -= this.DataSource_Initialized;
			this.dataSourceState = DataSourceStates.Initialized;
			this.SetDataSource(this.dataSource, this.dataMember);
		}

		private void DeleteItem(int itemIndex)
		{
			if ((this.columns == null) || (this.columns.Count == 0))
				this.RenewAllData();
			if (((((this.columns != null) && (this.columns.Count != 0)) && ((this.rows != null) && (this.rows.Count != 0))) && (this.currencyManager.GetItemProperties() != null)) &&
			    (itemIndex < this.rows.Count))
			{
				var dataEventHandler = this.dataEventHandler as IDataConnectionEvents;
				if (dataEventHandler != null)
					dataEventHandler.DataSource_ItemDeleted(itemIndex);
				this.rows.RemoveAt(itemIndex);
			}
		}

		public void Dispose()
		{
			if (this.currencyManager != null)
			{
				this.currencyManager.PositionChanged -= this.CurrencyManager_PositionChanged;
				this.currencyManager.ListChanged -= this.CurrencyManager_ListChanged;
			}
			this.currencyManager = null;
			if (this.rows != null)
			{
				for (var i = 0; i < this.rows.Count; i++)
					((ArrayList)this.rows[i]).Clear();
				this.rows.Clear();
			}
			if ((this.columns != null) && (this.columns.Count > 0))
				this.columns.Clear();
		}

		public int GetColumnIndex(string dataPropertyName)
		{
			var itemProperties = this.currencyManager.GetItemProperties();
			if (itemProperties == null)
				return -1;
			for (var i = 0; i < itemProperties.Count; i++)
				if (string.Compare(itemProperties[i].Name, dataPropertyName, true, CultureInfo.InvariantCulture) == 0)
					return i;
			return -1;
		}

		private void OnSelecltedRowChanged()
		{
			if (this.nLastSelectedRowIndex != this.currencyManager.Position)
			{
				this.ResetColumns();
				this.ResetRows();
			}
			var dataEventHandler = this.dataEventHandler as IDataConnectionEvents;
			if (dataEventHandler != null)
				dataEventHandler.DataSource_SelectedRowChanged(this.currencyManager.Position);
			this.nLastSelectedRowIndex = this.currencyManager.Position;
		}

		private void RenewAllData()
		{
			this.ResetColumns();
			this.ResetRows();
		}

		private void ResetColumns()
		{
			this.Columns.Clear();
			CDataColumnItem item;
			if (this.currencyManager != null)
			{
				var itemProperties = this.currencyManager.GetItemProperties();
				if (itemProperties != null)
					for (var i = 0; i < itemProperties.Count; i++)
					{
						item = new CDataColumnItem
						       {
							       BoundIndex = i,
							       Converter = itemProperties[i].Converter,
							       DisplayName = itemProperties[i].DisplayName,
							       IsReadonly = itemProperties[i].IsReadOnly,
							       Name = itemProperties[i].Name,
							       ValueType = itemProperties[i].PropertyType
						       };
						this.Columns.Add(item);
					}
			}
		}

		private void ResetItems()
		{
			this.RenewAllData();
			var dataEventHandler = this.dataEventHandler as IDataConnectionEvents;
			if ((dataEventHandler != null) && (dataEventHandler != null))
				dataEventHandler.DataSource_ResetItems();
		}

		private void ResetRows()
		{
			this.rows.Clear();
			if (((this.columns != null) && (this.columns.Count != 0)) && (this.currencyManager != null))
			{
				var itemProperties = this.currencyManager.GetItemProperties();
				if (itemProperties != null)
					for (var i = 0; i < this.currencyManager.List.Count; i++)
					{
						var list = new ArrayList(this.columns.Count);
						for (var j = 0; j < this.columns.Count; j++)
							list.Add(itemProperties[j].GetValue(this.currencyManager.List[i]));
						this.rows.Add(list);
					}
			}
		}

		public void SetDataSource(object dataSource, string dataMember)
		{
			if (this.connectionState != ConnectionStates.Initializing)
			{
				this.connectionState = ConnectionStates.Initializing;
				var notification = this.dataSource as ISupportInitializeNotification;
				if ((notification != null) && (this.dataSourceState == DataSourceStates.Initializing))
					notification.Initialized -= this.DataSource_Initialized;
				if (dataMember == null)
					dataMember = string.Empty;
				this.dataSource = dataSource;
				this.dataMember = dataMember;
				if (this.parent.BindingContext != null)
					try
					{
						if (this.currencyManager != null)
						{
							this.currencyManager.PositionChanged -= this.CurrencyManager_PositionChanged;
							this.currencyManager.ListChanged -= this.CurrencyManager_ListChanged;
						}
						if ((this.dataSource != null) && (this.dataSource != Convert.DBNull))
							if ((notification != null) && !notification.IsInitialized)
							{
								if (this.dataSourceState == DataSourceStates.None)
								{
									this.dataSourceState = DataSourceStates.Initializing;
									notification.Initialized += this.DataSource_Initialized;
								}
								this.currencyManager = null;
							}
							else
							{
								this.currencyManager = this.parent.BindingContext[this.dataSource, this.dataMember] as CurrencyManager;
								var dataEventHandler = this.dataEventHandler as IDataConnectionEvents;
								this.RenewAllData();
								if (dataEventHandler != null)
									dataEventHandler.DataSource_DataBoundCompleted();
							}
						else
							this.currencyManager = null;
						if (this.currencyManager != null)
						{
							this.currencyManager.PositionChanged += this.CurrencyManager_PositionChanged;
							this.currencyManager.ListChanged += this.CurrencyManager_ListChanged;
						}
					}
					finally
					{
						this.connectionState = ConnectionStates.Initialized;
					}
			}
		}

		private void SetEventHandler(object dataEventHandler)
		{
			if (this.dataEventHandler != dataEventHandler)
			{
				if (this.dataEventHandler != null)
					this.dataEventHandler = null;
				this.dataEventHandler = dataEventHandler;
				var events = dataEventHandler as IDataConnectionEvents;
				if (events != null)
					events.SetData(this.parent, this);
			}
		}

		private void UpdateItem(int itemIndex)
		{
			if ((this.columns == null) || (this.columns.Count == 0))
				this.RenewAllData();
			if (((this.columns != null) && (this.columns.Count != 0)) && ((this.rows != null) && (this.rows.Count != 0)))
			{
				var itemProperties = this.currencyManager.GetItemProperties();
				if (itemProperties != null)
				{
					var num = -1;
					var num2 = 0;
					for (var i = 0; i < this.columns.Count; i++)
						if (((ArrayList)this.rows[itemIndex])[i] != itemProperties[i].GetValue(this.currencyManager.List[itemIndex]))
						{
							((ArrayList)this.rows[itemIndex])[i] = itemProperties[i].GetValue(this.currencyManager.List[itemIndex]);
							num = i;
							num2++;
						}
					var dataEventHandler = this.dataEventHandler as IDataConnectionEvents;
					if (dataEventHandler != null)
						dataEventHandler.DataSource_ItemUpdated(itemIndex, (num2 == 1) ? num : -1);
				}
			}
		}
	}
}