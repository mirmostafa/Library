using Microsoft.Toolkit.Uwp.Notifications;

namespace Library.Wpf.Windows.UI;

/// <summary>
/// </summary>
/// <remarks>
/// If your app has an uninstaller, in your uninstaller you should call
/// ToastNotificationManagerCompat.Uninstall();. If your app is a "portable app" without an
/// installer, consider calling this method upon app exit unless you have notifications that are
/// meant to persist after your app is closed. The uninstall method will clean up any scheduled and
/// current notifications, remove any associated registry values, and remove any associated
/// temporary files that were created by the library.
/// </remarks>
/// <seealso cref="IDisposable"/>
public sealed class Toast2 : IDisposable
{
    private readonly ToastContentBuilder _builder = new();
    private readonly List<Button> _buttonList = new();
    private bool _disposedValue;
    private Action<Toast2>? _onActivated;

    public Toast2()
    {
        ToastNotificationManagerCompat.OnActivated -= this.OnActivated;
        ToastNotificationManagerCompat.OnActivated += this.OnActivated;
    }

    ~Toast2()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: false);
    }

    public static void ClearHistory() =>
        ToastNotificationManagerCompat.History.Clear();

    public static Toast2 New() =>
        new();

    public static void ShowText(string text, TimeSpan? expirationTime = null) =>
        New().AddText(text).Show(expirationTime);

    public Toast2 AddAppLogoOverride(Uri logoUri, bool circlized = false) =>
        this.Do(b => b.AddAppLogoOverride(logoUri, circlized ? ToastGenericAppLogoCrop.Circle : ToastGenericAppLogoCrop.Default));

    public Toast2 AddAppLogoOverride(string logoPath, bool circlized = false) =>
        this.AddAppLogoOverride(new Uri(logoPath), circlized);

    /// <summary>
    /// Add a button to the current toast.
    /// </summary>
    /// <param name="content">Text to display on the button.</param>
    /// <param name="arguments">
    /// App-defined string of arguments that the app can later retrieve once it is activated when
    /// the user clicks the button.
    /// </param>
    /// <returns>The current instance of <see cref="Toast2"/></returns>
    public Toast2 AddButton(string text, Action<Button> onClick, string? id = null)
        => this.AddButton(new(text, onClick, id));

    /// <summary>
    /// Add an button to the toast that will be display to the right of the input text box,
    /// achieving a quick reply scenario.
    /// </summary>
    /// <param name="textBoxId">
    /// ID of an existing <see cref="ToastTextBox"/> in order to have this button display to the
    /// right of the input, achieving a quick reply scenario.
    /// </param>
    /// <param name="content">Text to display on the button.</param>
    /// <param name="activationType">
    /// Type of activation this button will use when clicked. Defaults to Foreground.
    /// </param>
    /// <param name="arguments">
    /// App-defined string of arguments that the app can later retrieve once it is activated when
    /// the user clicks the button.
    /// </param>
    /// <returns>The current instance of <see cref="Toast2"/></returns>
    public Toast2 AddButton(string textBoxId, string text, Action<Button> onClick, string? id = null)
        => this.AddButton(new(text, onClick, id));

    public Toast2 AddButton(Button button)
        => this.Do(_ =>
        {
            var id = button.Id ?? Guid.NewGuid().ToString();

            this._buttonList.Add(button);
            _ = this._builder.AddButton(button.Text, ToastActivationType.Foreground, id);
            button.Owner = this;
        });

    public Toast2 AddInlineImage(Uri imageUri)
        => this.Do(b => b.AddInlineImage(imageUri));

    public Toast2 AddInlineImage(string imagePath)
        => this.AddInlineImage(new Uri(imagePath));

    /// <summary>
    /// Add an input text box that the user can type into.
    /// </summary>
    /// <param name="id">
    /// Required ID property so that developers can retrieve user input once the app is activated.
    /// </param>
    /// <param name="placeHolderContent">
    /// Placeholder text to be displayed on the text box when the user hasn't typed any text yet.
    /// </param>
    /// <param name="title">Title text to display above the text box.</param>
    /// <returns>The current instance of <see cref="Toast2"/></returns>
    public Toast2 AddInputTextBox(string id, string? placeHolderContent = default, string? title = default)
        => this.Do(b => b.AddInputTextBox(id, placeHolderContent, title));

    /// <summary>
    /// Add text to the toast.
    /// </summary>
    /// <param name="text">Custom text to display on the tile.</param>
    /// <returns></returns>
    public Toast2 AddText(string text) =>
        this.Do(b => b.AddText(text));

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public Toast2 SetOnNotificationActivated(Action<string> onActivated)
    {
        this._onActivated = (Action<Toast2>?)onActivated;
        return this;
    }

    /// <summary>
    /// Shows a new toast notification with the current content.
    /// </summary>
    public void Show()
        => this._builder.Show();

    public void Show(TimeSpan? expirationTime = null)
        => this._builder.Show(toast =>
        {
            if (expirationTime is { } et)
            {
                toast.ExpirationTime = DateTime.Now.AddMilliseconds(et.TotalMilliseconds);
            }
        });

    private void Dispose(bool disposing)
    {
        if (!this._disposedValue)
        {
            if (disposing)
            {
            }
            ToastNotificationManagerCompat.Uninstall();
            this._disposedValue = true;
        }
    }

    private Toast2 Do(Action<ToastContentBuilder> action)
    {
        action(this._builder);
        return this;
    }

    // Listen to notification activation
    private void OnActivated(ToastNotificationActivatedEventArgsCompat e)
    {
        var button = this._buttonList.FirstOrDefault(b => b.Id == e.Argument);
        if (button?.OnClick is { } click)
        {
            Application.Current.RunInUiThread(() => click(button));
        }
        //!? I know that this block is called twice, but I have no solution so far.
        else
        {
            Application.Current.RunInUiThread(() => this._onActivated?.Invoke(this));
        }

        ////else
        ////{
        ////    // Obtain the arguments from the notification
        ////    var args = ToastArguments.Parse(e.Argument);

        ////    // Obtain any user input (text boxes, menu selections) from the notification
        ////    var userInput = e.UserInput;

        ////    // Need to dispatch to UI thread if performing UI operations
        ////    Application.Current.Dispatcher.Invoke(delegate
        ////    {
        ////        // TODO: Show the corresponding content
        ////        _ = MessageBox.Show("Toast activated. Args: " + e.Argument);
        ////    });
        ////}
    }

    public record Button(string Text, Action<Button> OnClick, string? Id = null)
    {
        public Toast2 Owner { get; internal set; } = default!;
    }
}