using Library.Results;

namespace Library.Wpf.Windows;
public interface IStatefulPage
{
    public bool IsViewModelChanged { get; }
}

public interface IAsyncSavePage
{
    /// <summary>
    /// Saves the data asynchronously.
    /// </summary>
    /// <returns></returns>
    Task<Result<int>> SaveAsync();
}