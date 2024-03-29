namespace Library.Exceptions;

[Serializable]
public sealed class BreakException : Exception
{
    /// <summary>
    ///     Throws a new instance of Break Exception
    /// </summary>
    /// <exception cref="HanyCo.Mes20.Infra.Exceptions.BreakException"></exception>
    [DoesNotReturn]
    public static void Throw()
        => throw new BreakException();
}