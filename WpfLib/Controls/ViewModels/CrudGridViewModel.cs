using System.Collections;
using System.Collections.ObjectModel;
using Library.Data.Models;
using Library.Wpf.Bases;
using Library.Wpf.Markers;

namespace Library.Wpf.Controls.ViewModels;

[ViewModel]
public interface ICrudGridViewModel : IViewModel<ICrudGridViewModel>
{
    IEnumerable<DataColumnBindingInfo> GetHeaders();
    IEnumerable GetItems();
    object AddNew();
    object Update(object item);
    void Delete(object item);

    static ICrudGridViewModel New<TItem>(IEnumerable<TItem> items, IEnumerable<DataColumnBindingInfo> headers)
        where TItem : new() =>
        new CrudGridViewModel<TItem>(items, headers);

    static ICrudGridViewModel New<TItem>(IEnumerable<TItem> items, params DataColumnBindingInfo[] headers)
        where TItem : new() =>
        new CrudGridViewModel<TItem>(items, headers);
}

internal class CrudGridViewModel<TItem> : ICrudGridViewModel
    where TItem : new()
{
    private readonly ObservableCollection<TItem> _items;
    private readonly IEnumerable<DataColumnBindingInfo> _headers;

    public CrudGridViewModel(IEnumerable<TItem> items, IEnumerable<DataColumnBindingInfo> headers)
        : this(items, headers.ToArray())
    {
    }
    public CrudGridViewModel(IEnumerable<TItem> items, params DataColumnBindingInfo[] headers)
    {
        this._items = items is ObservableCollection<TItem> c ? c : new ObservableCollection<TItem>(items);
        this._headers = headers;
    }

    public void Delete(object item) =>
        this._items.Remove(item.To<TItem>());

    public IEnumerable GetItems() =>
        this._items;

    public IEnumerable<DataColumnBindingInfo> GetHeaders() =>
        this._headers;

    public object AddNew()
    {
        var result = new TItem();
        this._items.Add(result);
        return result;
    }

    public object Update(object item) => item;
}
