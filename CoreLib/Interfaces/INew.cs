namespace Library.Interfaces;

public interface INew<out TClass>
    where TClass : new()
{
    static TClass New() => new();
}