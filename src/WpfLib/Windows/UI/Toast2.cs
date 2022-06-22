using Microsoft.Toolkit.Uwp.Notifications;

namespace Library.Wpf.Windows.UI;

public static class Toast2
{
    public static bool IsEnabled { get; set; } = true;

    public static void Info(string message, string? title = null)
        => IsEnabled.IfTrue(() => new ToastContentBuilder()
                                        .AddText(message)
                                        .AddText(title, AdaptiveTextStyle.Title)
                                        .Show());
}