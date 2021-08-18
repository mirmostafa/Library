namespace Library.Exceptions.Validations
{
    [Serializable]
    public sealed class ObjectDuplicateValidationException : ValidationExceptionBase
    {
        public ObjectDuplicateValidationException(string objectName)
            : base($"{objectName} already exists", "")
        {

        }
    }
}