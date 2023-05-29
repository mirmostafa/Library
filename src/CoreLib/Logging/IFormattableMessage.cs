namespace Library.Logging;

/// <summary>
/// Interface for formatting messages.
/// </summary>
/// <returns>Formatted message.</returns>
public interface IFormattableMessage
{
    /// <summary>
    /// Formats to string.
    /// </summary>
    string Format();
}