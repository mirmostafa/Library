namespace Library.Interfaces;

public static class Math
{
    static Func<int, int, int> Sum => (x, y) => x + y;
}