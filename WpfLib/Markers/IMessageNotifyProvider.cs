using Library.Windows;

namespace Library.Wpf.Markers;

public interface IMessageNotifyProvider
{
    static abstract MessageNotifyResult Show(NotificationMessage message);
}

public enum MessageNotifyResult
{
    None = 0,
    Ok = 1,
    Yes = 2,
    No = 4,
    Cancel = 8,
    Retry = 0x10,
    Close = 0x20,
    CustomButtonClicked = 0x100
}