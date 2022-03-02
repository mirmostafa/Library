namespace Library.Wpf.Dialogs
{
    public static class MsgBox
    {
        public static MessageBoxResult Show(
            string? text,
            string? caption,
            MessageBoxButton button,
            MessageBoxImage icon,
            MessageBoxResult defaultResult,
            Window? owner)
            => MessageBox.Show(owner ?? Application.Current.MainWindow, text, caption, button, icon, defaultResult);

        public static void Inform(string text, string? caption = null, Window? owner = null)
            => Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, owner);

        public static MessageBoxResult Ask(string text, string? caption = null, Window? owner = null)
            => Show(text, caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, owner);

        public static MessageBoxResult AskWithWarning(string text, string? caption = null, Window? owner = null)
            => Show(text, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes, owner);

        public static void Error(string text, string? caption = null, Window? owner = null)
            => Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, owner);

        public static void Error(Exception exception, string? text = null, string? caption = null, Window? owner = null)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            _ = Show(
                $"{text}{Environment.NewLine}{exception.GetBaseException().Message}",
                caption,
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.OK,
                owner);
        }
    }
}

namespace Mohammad.EventsArgs
{
}

namespace Library.Wpf.Helpers
{
}

namespace Library.Wpf.Windows.Media
{
}

namespace Mohammad.Collections.Specialized
{
}

namespace Library.Wpf.Helpers
{
}

namespace Library.Wpf.Helpers
{

    public delegate void SetSelectedEventHandler<T>(ItemsControl container, SetSelectedInfo<T> info);
}

namespace Mohammad.Internals
{
}