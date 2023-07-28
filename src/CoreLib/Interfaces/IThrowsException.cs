using Library.Exceptions;

namespace Library.Interfaces;

//public interface IThrowsException<TSelfException>
//    where TSelfException : Exception, IException, new()
//{
//    [DoesNotReturn]
//    static void Throw()
//        => throw new TSelfException();

//    [DoesNotReturn]
//    static void Throw(string message)
//        => throw Activator.CreateInstance(typeof(TSelfException), message)!.Cast().To<Exception>();
//}

public interface IThrowableException
{
    [DoesNotReturn]
    static abstract void Throw();

    [DoesNotReturn]
    static abstract void Throw(string message);

    [DoesNotReturn]
    static abstract void Throw(string message, Exception innerException);
}