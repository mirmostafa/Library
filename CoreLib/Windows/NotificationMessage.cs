namespace Library.Windows;

public record struct NotificationMessage(in string Text, in string? Instruction = null, in string? Title = null, in string? Details = null, MessageLevel level = MessageLevel.Info)
{
    public static explicit operator string?(NotificationMessage? fullMessage) => fullMessage?.Text;
    public static implicit operator NotificationMessage?(string? message) => message is null ? null : new(message);

    public override string ToString()
        => this.Text;
}

public enum MessageLevel
{
    Info,
    Warning,
    Error
}