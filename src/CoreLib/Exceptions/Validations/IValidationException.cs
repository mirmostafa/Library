namespace Library.Exceptions.Validations;

public interface IValidationException : IException
{
    static abstract int ErrorCode { get; }
}