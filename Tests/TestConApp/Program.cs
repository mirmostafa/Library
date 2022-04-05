namespace TestConApp;

internal partial class Program
{
    private static void Main(params string[] args)
    {
        var guid = Guid.NewGuid();
        var readFromDb = guid.ToString();
        Console.WriteLine(readFromDb);
        var g = ToGuidFromString(readFromDb);
        Console.WriteLine(g);
    }

    public static Guid ToGuidFromString(in ReadOnlySpan<char> str)
    {
        Span<char> buffer = stackalloc char[24];
        for (var i = 0; i < 24; i++)
        {
            buffer[i] = str[i];
        }
        Span<byte> result = stackalloc byte[16];
        _ = Convert.TryFromBase64Chars(buffer, result, out _);
        return new(result);
    }
}