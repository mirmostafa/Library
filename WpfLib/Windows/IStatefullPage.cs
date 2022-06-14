using Library.Results;

namespace Library.Wpf.Windows;
public interface IStatefulPage
{
    bool IsViewModelChanged { get; }
}

public interface ISaveChangesAsync
{
    Task<Result> SaveChangesAsync();
}