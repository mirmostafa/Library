namespace Library.Wpf.Markers;

////[EditorBrowsable(EditorBrowsableState.Never)]
////[Browsable(false)]
////public interface ISupportDataBind
////{
////    /// <summary>
////    /// Occurs when [binding data].
////    /// </summary>
////    event EventHandler? BindingData;

////    /// <summary>
////    /// Rebinds the data.
////    /// </summary>
////    void RebindData();
////}

////public interface ISupportDataBinding : ISupportDataBind
////{
////    /// <summary>
////    /// Binds the data.
////    /// </summary>
////    void BindData();
////}

public interface ISupportAsyncDataBinding
{
    /// <summary>
    /// Binds the data asynchronously.
    /// </summary>
    /// <returns></returns>
    Task BindDataAsync();
    /// <summary>
    /// Rebinds the data asynchronously.
    /// </summary>
    /// <returns></returns>
    Task RebindDataAsync();

}
