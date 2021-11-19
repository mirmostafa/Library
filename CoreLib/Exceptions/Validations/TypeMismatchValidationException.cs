namespace Library.Exceptions.Validations;

public sealed class TypeMismatchValidationException : ValidationExceptionBase
{
    public TypeMismatchValidationException()
    {
    }

    public TypeMismatchValidationException(string message)
        : base(message)
    {
    }

    public TypeMismatchValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
