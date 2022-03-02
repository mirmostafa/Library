using Library.Data.Models;
using Library.Wpf.Bases;
using Library.Wpf.Markers;
using System.Collections;

namespace Library.Wpf.Controls.ViewModels;

[ViewModel]
public interface ICrudGridModelBinder : IViewModel
{
    IEnumerable<DataColumnBindingInfo> GetHeaders();
    IEnumerable GetItems();
    object AddNew();
    object Update(object item);
    void Delete(object item);
}
