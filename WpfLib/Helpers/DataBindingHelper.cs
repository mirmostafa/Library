using Library.Data.Models;
using Library.Validations;
using System.Windows.Data;

namespace Library.Wpf.Helpers;

public static class DataBindingHelper
{
    public static Binding ToBinding(this IDataColumnBindingInfo dataColumn)
        => new(dataColumn.ArgumentNotNull().BindingPath);

    public static DataGridColumn ToDataGridColumn(this IDataColumnBindingInfo dataColumn) =>
        dataColumn.ArgumentNotNull().DataType switch
        {
            DataColumnBindingType.None or DataColumnBindingType.Text or _ =>
                 new DataGridTextColumn { Header = dataColumn.Title, Binding = dataColumn.ToBinding() }
        };

    public static IEnumerable<DataGridColumn> ToDataGridColumn(this IEnumerable<IDataColumnBindingInfo> dataColumns) =>
            dataColumns.Select(ToDataGridColumn);
}
