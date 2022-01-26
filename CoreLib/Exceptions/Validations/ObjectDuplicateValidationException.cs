namespace Library.Exceptions.Validations;

[Serializable]
public sealed class ObjectDuplicateValidationException : ValidationExceptionBase
{
    public static new int ErrorCode { get; } = 610 * -1;

    public ObjectDuplicateValidationException(string objectName)
        : base($"{objectName} already exists", "")
    {

    }
}
