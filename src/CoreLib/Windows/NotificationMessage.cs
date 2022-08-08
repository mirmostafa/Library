namespace Library.Windows;

public record struct NotificationMessage(in string Text, in string? Instruction = null, in string? Title = null, in string? Details = null, MessageLevel? level = MessageLevel.Info, object? Owner = null)
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

public interface IFromNotificationMessage<T>
    where T : IFromNotificationMessage<T>
{
    static abstract T FromNotificationMessage(NotificationMessage message);

    NotificationMessage ToNotificationMessage();
}

public interface IToNotificationMessage<T>
    where T : IToNotificationMessage<T>
{
    NotificationMessage ToNotificationMessage();
}

public interface INotificationMessageCompatible<T> : IToNotificationMessage<T>, IFromNotificationMessage<T>
    where T : INotificationMessageCompatible<T>
{
}