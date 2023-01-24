namespace Library.Windows;

public record struct NotificationMessage(in string Text, in string? Instruction = null, in string? Title = null, in string? Details = null, MessageLevel? level = MessageLevel.Info, in object? Owner = null)
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

public interface IFromNotificationMessage<TSelf>: IToNotificationMessage<TSelf>
    where TSelf : IFromNotificationMessage<TSelf>
{
    static abstract TSelf FromNotificationMessage(NotificationMessage message);
}

public interface IToNotificationMessage<TSelf>
    where TSelf : IToNotificationMessage<TSelf>
{
    NotificationMessage ToNotificationMessage();
}

public interface INotificationMessageCompatible<TSelf> : IToNotificationMessage<TSelf>, IFromNotificationMessage<TSelf>
    where TSelf : INotificationMessageCompatible<TSelf>
{
}