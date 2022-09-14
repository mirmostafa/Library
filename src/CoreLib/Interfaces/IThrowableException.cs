using Library.Exceptions;
using Library.Helpers;

namespace Library.Interfaces;

public interface IThrowableException<TException>
    where TException : Exception, IException, new()
{
    [DoesNotReturn]
    static void Throw() 
        => throw new TException();
    
    [DoesNotReturn]
    static void Throw(string message) 
        => throw Activator.CreateInstance(typeof(TException), message)!.To<Exception>();
}