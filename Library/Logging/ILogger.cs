namespace Library.Logging
{
    public interface ILogger : ILogger<object>
    {
        /// <summary>
        /// The empty Logger
        /// </summary>
        static readonly ILogger Empty = new EmptyLogger();
    }
}
