using System.Windows.Controls;
using System.Windows.Data;
using Library.Data.Models;
using Library.Validations;

namespace Library.Wpf.Helpers;

public static class DataBindingHelper
{
    public static Binding ToBinding(this DataColumnBindingInfo dataColumn)
        => new(nameof(Tuple<string, string>.Item2));

    public static DataGridColumn ToDataGridColumn(this DataColumnBindingInfo dataColumn) =>
        dataColumn.ArgumentNotNull().DataType switch
        {
            DataColumnBindingType.None or DataColumnBindingType.Text or _ =>
                 new DataGridTextColumn { Header = dataColumn.Title, Binding = dataColumn.ToBinding() }
        };
}
