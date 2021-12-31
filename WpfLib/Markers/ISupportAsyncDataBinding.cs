using System.ComponentModel;

namespace Library.Wpf.Markers;

[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
public interface IHasDataBindingAbility
{
    /// <summary>
    /// Occurs when [binding data].
    /// </summary>
    event EventHandler? BindingData;

    /// <summary>
    /// Rebinds the data.
    /// </summary>
    void RebindData();
}

public interface ISupportDataBinding : IHasDataBindingAbility
{
    /// <summary>
    /// Binds the data.
    /// </summary>
    void BindData();
}

public interface ISupportAsyncDataBinding : IHasDataBindingAbility
{
    /// <summary>
    /// Binds the data asynchronously.
    /// </summary>
    /// <returns></returns>
    Task BindDataAsync();
}
