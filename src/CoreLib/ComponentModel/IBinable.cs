namespace Library.ComponentModel;

public interface IAsyncBindable<TViewModel>
{
    Task BindAsync(TViewModel viewModel);
}

public interface IAsyncBindable
{
    Task BindAsync();
}

public interface IAsyncBindableBidirectionalViewModel<TViewModel> : IAsyncBindable<TViewModel>, IBidirectionalViewModel<TViewModel>
{
}

public interface IAsyncBindableUnidirectionalViewModel<TViewModel> : IAsyncBindable<TViewModel>, IUnidirectionalViewModel<TViewModel>
{
}

public interface IBidirectionalViewModel<TViewModel> : IUnidirectionalViewModel<TViewModel>
{
    new TViewModel ViewModel { get; set; }
}

public interface IBinable<TViewMode>
{
    void Bind(TViewMode viewMode);

    void Rebind();
}

public interface IBindable
{
    void Bind();
}

public interface IResetable
{
    void Reset();
}

public interface IUnidirectionalViewModel<TViewModel>
{
    TViewModel ViewModel { get; }
}

public interface IResetable<T>
{
    T Reset();
}

public interface ISupportReadOnly
{
    bool IsReadOnly { get; set; }
}