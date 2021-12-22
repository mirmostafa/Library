using System.Collections;
using System.Windows.Controls;
using Library.EventsArgs;
using Library.Validations;
using Library.Wpf.Controls.ViewModels;

namespace Library.Wpf.Controls;
/// <summary>
/// Interaction logic for DataCrudGridView.xaml
/// </summary>
public partial class DataCrudGridView : UserControl
{
    public event EventHandler<ItemActingEventArgs<ICrudGridViewModel?>> DataBind;

    public ICrudGridViewModel? ViewModel
    {
        get => this.DataContext?.To<ICrudGridViewModel?>();
        set => this.DataContext = value;
    }

    public DataCrudGridView() =>
        this.InitializeComponent();

    private void DataCrudGridView_Loaded(object sender, RoutedEventArgs e) =>
        this.BindData();

    private void BindData()
    {
        Check.IfNotNull(this.ViewModel);

        this.OnDataBind();
        if (!this.DataGrid.Columns.Any())
        {
            _ = this.ViewModel.GetHeaders()
                              .Select(x => x.ToDataGridColumn())
                              .ForEach(this.DataGrid.Columns.Add)
                              .Build();
        }
        this.DataGrid.ItemsSource = this.ViewModel.GetItems();
    }

    private void OnDataBind()
    {
        if (this.DataBind is null)
        {
            return;
        }

        var e = new ItemActingEventArgs<ICrudGridViewModel?>(this.ViewModel);
        this.DataBind(this, e);
        if (e.Handled)
        {
            return;
        }

        this.ViewModel = e.Item;
        this.BindDataGrid();
    }

    private void BindDataGrid() =>
        this.DataGrid.BindItemsSource(this.GenerateItems());
    private IEnumerable? GenerateItems() =>
        this.ViewModel?.GetItems();

    private void NewItemButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.ViewModel is null)
        {
            return;
        }

        var item = this.ViewModel.AddNew();
        this.BindDataGrid();
        this.DataGrid.SelectedItem = item;
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
        foreach (var item in this.DataGrid.SelectedItems)
        {
            this.ViewModel.Delete(item);
        }
        this.BindDataGrid();
    }
}
