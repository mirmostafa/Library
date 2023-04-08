using System.Diagnostics.CodeAnalysis;

using Library.Exceptions.Validations;

using Windows.UI.Notifications;

namespace Library.Wpf.Windows.UI;

public sealed class Toast
{
    private readonly string _appTitle;
    private ToastNotification? _toast;
    private ToastNotifier? _toastNotifier;

    private Toast(string appTitle)
        => this._appTitle = appTitle.ArgumentNotNull();

    public static Toast CreateImageLongContent(string imagePath, string title, string content, string appTitle)
        => new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastImageAndText02, new[] { title, content }, imagePath)
        };

    public static Toast CreateImageLongTitle(string imagePath, string title, string appTitle)
        => new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastImageAndText01, new[] { title }, imagePath)
        };

    public static Toast CreateImageLongTitle(string imagePath, string title, string content, string appTitle)
        => new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastImageAndText03, new[] { title, content }, imagePath)
        };

    public static Toast CreateImageMultipleLineContent(string imagePath, string title, string line1, string line2, string appTitle)
        => new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastImageAndText04, new[] { title, line1, line2 }, imagePath)
        };

    public static Toast CreateLongContent(string content, string appTitle)
        => new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastText01, new[] { content })
        };

    public static Toast CreateLongContent(string title, string content, string appTitle)
        => new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastText02, new[] { title, content })
        };

    /// <summary>
    /// Creates a long content toast notification.
    /// </summary>
    /// <param name="content">The content. (Max length: 170 chars)</param>
    /// <param name="title">The title.</param>
    /// <param name="appTitle">The application title.</param>
    /// <returns></returns>
    public static Toast CreateLongTitle(string title, string content, string appTitle) =>
        new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastText03, new[] { title, content })
        };

    public static Toast CreateMultipleLineContent(string line1, string line2, string title, string appTitle)
        => new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastText04, new[] { title, line1, line2 })
        };

    [MemberNotNull(nameof(_toastNotifier))]
    public Toast Build()
        => this.Fluent(this._toastNotifier = ToastNotificationManager.CreateToastNotifier(this._appTitle));

    public Toast SetExpirationTime(DateTimeOffset? dateTimeOffset)
        => this.Do(t => t.ExpirationTime = dateTimeOffset);

    public Toast SetGroup(string group)
        => this.Do(t => t.Group = group);

    public Toast SetPriority(ToastNotificationPriority priority)
        => this.Do(t => t.Priority = priority);

    public Toast SetRemoteId(string id)
        => this.Do(t => t.RemoteId = id);

    /// <summary>
    /// Shows this instance.
    /// </summary>
    public void Show()
    {
        if (this._toastNotifier is null)
        {
            _ = this.Build();
        }
        this._toastNotifier.Show(this._toast);
    }

    private static ToastNotification InnerCreateToast(ToastTemplateType type,
                                                      IEnumerable<string> stringElementChildren,
                                                      string? imagePath = null,
                                                      string? audioSource = null)
    {
        var toastXml = ToastNotificationManager.GetTemplateContent(type);

        if (stringElementChildren?.Any() is true)
        {
            var stringElements = toastXml.GetElementsByTagName("text");
            var children = stringElementChildren.ToArray();
            for (var index = 0; index < children.Length; index++)
            {
                _ = stringElements[index].AppendChild(toastXml.CreateTextNode(children[index]));
            }
        }

        if (imagePath is not null)
        {
            var imageElements = toastXml.GetElementsByTagName("image");
            imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;
        }
        if (audioSource is not null)
        {
            var audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", audioSource);
            audio.SetAttribute("loop", "false");
            _ = toastXml.DocumentElement.AppendChild(audio);
        }

        return new ToastNotification(toastXml);
    }

    private Toast Do(Action<ToastNotification> action)
    {
        this.Validate();
        action(this._toast!);
        return this;
    }

    private void Validate()
    {
        if (this._toastNotifier is not null)
        {
            Throw(new ValidationException("Cannot be changed."));
        }
    }
}