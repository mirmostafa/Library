namespace WpfLibrary.Bases
{
    public interface IViewModel
    {
    }

    public interface IViewModel<TViewModel> : IViewModel
        where TViewModel : IViewModel<TViewModel>
    {
        //> static IViewMode CreateNew();
    }
}
