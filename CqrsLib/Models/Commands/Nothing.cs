namespace Library.Cqrs.Models.Commands;

public class Nothing
{
    private Nothing()
    { }

    public static Nothing Instance { get; } = new();

    public string ToCode() 
        => string.Empty;
}