namespace Library.Cqrs;

public class Nothing
{
    private Nothing() { }

    public static Nothing Instance { get; } = new();

    public string ToCode() => string.Empty;
}
