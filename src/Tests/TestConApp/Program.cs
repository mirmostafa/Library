using Library.DesignPatterns;

namespace TestConApp;

internal partial class Program
{
    private static void Main()
    {
        var p = new Person("Ali", 5);
        var maybe = new Monad<Person>(p)
                        .NotNull(p => new Person("Reza", 5))
                        .AnyWay(p => WriteLine($"p: {p}"))
                        .NotNull(_ => (Person?)null)
                        .AnyWay(p => WriteLine($"p: {p}"));
    }
}