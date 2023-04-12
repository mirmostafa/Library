namespace Library.Coding;

public sealed class ProgressRoutine<T> : IProgress<(T State, int Current, int Maximum)>
{
    private readonly Action<(T State, int Current, int Maximum)> _reporter;

    public ProgressRoutine(Action<(T State, int Current, int Maximum)> reporter)
        => this._reporter = reporter;
    
    public void Report((T State, int Current, int Maximum) value) 
        => this._reporter(value);

    public void Report(T state, int current, int maximum)
        => this._reporter((state, current, maximum));
}