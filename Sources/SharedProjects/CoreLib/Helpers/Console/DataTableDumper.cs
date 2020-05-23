#region

using System;
using System.Data;
using System.Linq;

#endregion

namespace Mohammad.Helpers.Console
{
    public static class DataTableDumper
    {
        public static void WriteDump(this DataTable dataTable)
        {
            var dumper = new Dumper(0);
            dumper.Write(dataTable, null);
        }

        private class Dumper : Dumper<DataTable>
        {
            public Dumper(int depth)
                : base(depth) { }

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
                    ConsoleHelper.LineFeed();
                };

                System.Console.SetWindowSize(160, System.Console.WindowHeight);
                element.Columns.Cast<DataColumn>().ForEach(writeColumn);
                ConsoleHelper.LineFeed();
                "-".Repeat(System.Console.WindowWidth).WriteLine();
                element.Rows.Cast<DataRow>().ForEach(writeRow);
                ConsoleHelper.LineFeed();
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
                    ConsoleHelper.LineFeed();
                };

                System.Console.SetWindowSize(160, System.Console.WindowHeight);
                element.Columns.Cast<DataColumn>().ForEach(writeColumn);
                ConsoleHelper.LineFeed();
                "-".Repeat(System.Console.WindowWidth).WriteLine();
                element.Rows.Cast<DataRow>().ForEach(writeRow);
                ConsoleHelper.LineFeed();
            }
        }
    }
}