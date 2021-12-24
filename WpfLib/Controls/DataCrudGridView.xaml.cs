﻿using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using Library.Data.Models;
using Library.EventsArgs;
using Library.Validations;

namespace Library.Wpf.Controls;
/// <summary>
/// Interaction logic for DataCrudGridView.xaml
/// </summary>
public partial class DataCrudGridView : UserControl
{
    public event EventHandler<ItemActingEventArgs<IEnumerable<IDataColumnBindingInfo>>> ColumnBind;
    public event EventHandler<ItemActingEventArgs<ICollection<object>?>> DataBind;
    public event EventHandler<ItemActingEventArgs<object>> ItemAdd;
    public event EventHandler<ItemActingEventArgs<IEnumerable<object>>> ItemRemove;

    public ICollection<object> ViewModel
    {
        get => (this.DataContext ??= new List<object>()).To<IList<object>>();
        set => this.DataContext = value;
    }

    public DataCrudGridView() =>
        this.InitializeComponent();

    private void DataCrudGridView_Loaded(object sender, RoutedEventArgs e) =>
        this.IfTrue(!ControlHelper.IsDesignTime(), () => this.BindData());

    [MemberNotNullWhen(true, nameof(ViewModel))]
    private bool BindData()
    {
        return initializeColumns() && bindData();

        bool initializeColumns()
        {
            if (this.DataGrid.Columns.Any())
            {
                return true;
            }
            var columnBindArgs = new ItemActingEventArgs<IEnumerable<IDataColumnBindingInfo>>();
            this.ColumnBind?.Invoke(this, columnBindArgs);
            if (columnBindArgs.Handled || (columnBindArgs?.Item?.Any() ?? true))
            {
                return false;
            }
            this.DataGrid.AddColumns(columnBindArgs.Item);
            return true;
        }
        bool bindData()
        {
            var dataBindArgs = new ItemActingEventArgs<ICollection<object>?>(this.ViewModel);
            this.DataBind?.Invoke(this, dataBindArgs);
            if (dataBindArgs.Handled)
            {
                return false;
            }
            this.ViewModel = dataBindArgs.Item.NotNull();
            this.BindDataGrid();
            return true;
        }
    }

    private DataCrudGridView BindDataGrid() =>
        this.Fluent(this.DataGrid.BindItemsSource(this.ViewModel));

    private void NewItemButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.ViewModel is null)
        {
            return;
        }

        var itemAddEventArg = new ItemActingEventArgs<object>();
        this.ItemAdd?.Invoke(this, itemAddEventArg);
        if (itemAddEventArg.Handled || itemAddEventArg.Item is null)
        {
            return;
        }
        this.ViewModel.Add(itemAddEventArg.Item);
        this.BindDataGrid();
    }

    private void DeleteItemsButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.ViewModel is null)
        {
            return;
        }

        if (!this.DataGrid.SelectedItems.Any())
        {
            throw new Exceptions.Validations.NoItemValidationException("New item selected.");
        }
        var itemRemoveArgs = new ItemActingEventArgs<IEnumerable<object>>(this.DataGrid.SelectedItems.Cast<object>());
        this.ItemRemove?.Invoke(this, itemRemoveArgs);
        if (itemRemoveArgs.Handled)
        {
            return;
        }

        this.BindDataGrid();
    }
}
