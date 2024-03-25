using Library.Exceptions;

namespace Library.Interfaces;

public interface IThrowableException
{
    [DoesNotReturn]
    static abstract void Throw();

    [DoesNotReturn]
    static abstract void Throw(string message);

    [DoesNotReturn]
    static abstract void Throw(string message, Exception innerException);
}