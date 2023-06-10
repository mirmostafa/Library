using System.Runtime.Serialization;

using Library.Windows;

namespace Library.Exceptions;

/// <summary>
///     Base exception abstract class
/// </summary>
/// <seealso cref="Exception" />
/// <seealso cref="HanyCo.Mes20.Infra.Exceptions.IException" />
[Serializable]
public abstract class ExceptionBase : Exception, IException, IToNotificationMessage<ExceptionBase>
{
    protected ExceptionBase()
    {
    }

    protected ExceptionBase(string message)
            : base(message)
    {
    }

    protected ExceptionBase(string? message, Exception? innerException)
            : base(message, innerException)
    {
    }

    protected ExceptionBase(string message, string? instruction = null, string? title = null, string? details = null, Exception? inner = null, object? owner = null)
            : base(message, inner)
    {
        this.Instruction = instruction;
        this.Owner = owner;
        this.Title = title;
        this.Details = details;
    }

    protected ExceptionBase(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
    {
    }

    protected ExceptionBase(NotificationMessage notificationMessage)
            => (this.Title, this.Instruction, _, this.Details, _, this.Owner) = notificationMessage;

    /// <summary>
    /// Gets or sets the details.
    /// </summary>
    /// <value>
    /// The details.
    /// </value>
    public string? Details { get; init; }

    /// <summary>
    ///     Gets the instruction.
    /// </summary>
    /// <value>
    ///     The instruction.
    /// </value>
    public string? Instruction { get; init; }

    /// <summary>
    /// Gets or sets the owner.
    /// </summary>
    /// <value>
    /// The owner.
    /// </value>
    public object? Owner { get; init; }

    /// <summary>
    /// Gets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public string? Title { get; init; }

    public NotificationMessage ToNotificationMessage()
        => new(this.Title ?? string.Empty, this.Instruction, null, this.Details, MessageLevel.Error, this.Owner);
}