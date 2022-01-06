using Microsoft.Toolkit.Uwp.Notifications;

namespace Library.Wpf.Windows.UI;

public static class Toast2
{
    public static void Info(string message, string? title) =>
        new ToastContentBuilder()
            .AddText(message)
            .AddText(title, AdaptiveTextStyle.Title)
            .Show();
}
