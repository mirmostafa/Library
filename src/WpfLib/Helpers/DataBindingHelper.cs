using System.Windows.Data;

using Library.Data.Models;

namespace Library.Wpf.Helpers;

public static class DataBindingHelper
{
    public static Binding ToBinding(this IDataColumnBindingInfo dataColumn) =>
        new(dataColumn.ArgumentNotNull().BindingPathOrElement);

    public static DataGridColumn ToDataGridColumn(this IDataColumnBindingInfo dataColumn) =>
        dataColumn.ArgumentNotNull().DataType switch
        {
            DataColumnBindingType.Default or DataColumnBindingType.Text or _ =>
                 new DataGridTextColumn { Header = dataColumn.Title, Binding = dataColumn.ToBinding() }
        };

    public static IEnumerable<DataGridColumn> ToDataGridColumn(this IEnumerable<IDataColumnBindingInfo> dataColumns) =>
            dataColumns.Select(ToDataGridColumn);
}