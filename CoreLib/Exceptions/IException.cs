using Library.Windows;

namespace Library.Exceptions;

public interface IException
{
    /// <summary>
    /// Gets or sets the details.
    /// </summary>
    /// <value>
    /// The details.
    /// </value>
    public string? Details { get; }

    /// <summary>
    ///     Gets the instruction.
    /// </summary>
    /// <value>
    ///     The instruction.
    /// </value>
    string? Instruction { get; }

    /// <summary>
    ///     Gets the message.
    /// </summary>
    /// <value>
    ///     The message.
    /// </value>
    string Message { get; }

    /// <summary>
    ///     Gets the owner.
    /// </summary>
    /// <value>
    ///     The owner.
    /// </value>
    object? Owner { get; }

    /// <summary>
    /// Gets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    string? Title { get; }

    /// <summary>
    ///     Gets the base exception.
    /// </summary>
    /// <returns></returns>
    Exception? GetBaseException();

    NotificationMessage ToFullMessage()
        => new(this.Message, this.Instruction, this.Title, this.Details);
}