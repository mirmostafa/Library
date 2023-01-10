namespace Library.DesignPatterns.Markers;

public interface Builder<out TResult>
{
    TResult Build();
}

public interface IBuilder
{
    void Build();
}

public interface Builder<in TArgs, out TResult>
{
    TResult Build(TArgs args);
}