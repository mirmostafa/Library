#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

#region
using System;
using System.Data;
using System.Linq;
#endregion

namespace Library35.Helpers.Console
{
	public static class DataTableDumper
	{
		#region Methods

		#region WriteDump
		public static void WriteDump(this DataTable dataTable)
		{
			var dumper = new Dumper(0);
			dumper.Write(dataTable, null);
		}
		#endregion

		#endregion

		#region Nested Types

		#region Dumper
		private class Dumper : Dumper<DataTable>
		{
			public Dumper(int depth)
				: base(depth)
			{
			}

			internal override void Write(DataTable element, string prefix)
			{
				Action<DataColumn> writeColumn = delegate(DataColumn column)
				                                 {
					                                 column.Write();
					                                 this.WriteTab();
				                                 };
				Action<DataRow> writeRow = delegate(DataRow row)
				                           {
					                           Action<object> writeData = delegate(object data)
					                                                      {
						                                                      data.Write();
						                                                      this.WriteTab();
					                                                      };
					                           row.GetColumnsData().ForEach(writeData);
					                           Helper.LineFeed();
				                           };

				System.Console.SetWindowSize(160, System.Console.WindowHeight);
				element.Columns.Cast<DataColumn>().ForEach(writeColumn);
				Helper.LineFeed();
				"-".Repeat(System.Console.WindowWidth).WriteLine();
				element.Rows.Cast<DataRow>().ForEach(writeRow);
				Helper.LineFeed();
			}

			internal override void WriteLine(DataTable element, string prefix)
			{
				Action<DataColumn> writeColumn = delegate(DataColumn column)
				                                 {
					                                 column.Write();
					                                 this.WriteTab();
				                                 };
				Action<DataRow> writeRow = delegate(DataRow row)
				                           {
					                           Action<object> writeData = delegate(object data)
					                                                      {
						                                                      data.Write();
						                                                      this.WriteTab();
					                                                      };
					                           row.GetColumnsData().ForEach(writeData);
					                           Helper.LineFeed();
				                           };

				System.Console.SetWindowSize(160, System.Console.WindowHeight);
				element.Columns.Cast<DataColumn>().ForEach(writeColumn);
				Helper.LineFeed();
				"-".Repeat(System.Console.WindowWidth).WriteLine();
				element.Rows.Cast<DataRow>().ForEach(writeRow);
				Helper.LineFeed();
			}
		}
		#endregion

		#endregion
	}
}