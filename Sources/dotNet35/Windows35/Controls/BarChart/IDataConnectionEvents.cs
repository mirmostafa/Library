#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library35.Windows.Controls.BarChart
{
	internal interface IDataConnectionEvents
	{
		void DataSource_DataBoundCompleted();
		void DataSource_ItemAdded(int nItemIndex);
		void DataSource_ItemDeleted(int nItemIndex);
		void DataSource_ItemUpdated(int nRowIndex, int nColIndex);
		void DataSource_ResetItems();
		void DataSource_SelectedRowChanged(int nPosition);
		void SetData(object chart, object dataConnection);
	}
}