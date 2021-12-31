using System.Diagnostics.CodeAnalysis;
using Library.Validations;
using Windows.UI.Notifications;

namespace Library.Wpf.Windows.UI;

public class Toast
{
    //public static void CreateToast(string filePath)
    //{
    //    var type = ToastTemplateType.ToastImageAndText02;
    //    var title = "Title";
    //    var content = "Message";

    //    var toastXml = ToastNotificationManager.GetTemplateContent(type);

    //    // Fill in the text elements
    //    var stringElements = toastXml.GetElementsByTagName((string?)"text");
    //    stringElements[0].AppendChild(toastXml.CreateTextNode(title));
    //    stringElements[1].AppendChild(toastXml.CreateTextNode(content));

    //    // Specify the absolute path to an image
    //    var imageElements = toastXml.GetElementsByTagName((string?)"image");
    //    imageElements[0].Attributes.GetNamedItem("src").NodeValue = filePath;

    //    // Change default audio if desired - ref - https://docs.microsoft.com/en-us/uwp/schemas/tiles/toastschema/element-audio
    //    var audio = toastXml.CreateElement("audio");
    //    //audio.SetAttribute("src", "ms-winsoundevent:Notification.Reminder");
    //    //audio.SetAttribute("src", "ms-winsoundevent:Notification.IM");
    //    //audio.SetAttribute("src", "ms-winsoundevent:Notification.Mail"); // sounds like default
    //    //audio.SetAttribute("src", "ms-winsoundevent:Notification.Looping.Call7");  
    //    audio.SetAttribute("src", "ms-winsoundevent:Notification.Looping.Call2");
    //    //audio.SetAttribute("loop", "false");
    //    // Add the audio element
    //    toastXml.DocumentElement.AppendChild(audio);

    //    var actions = toastXml.CreateElement("actions");
    //    toastXml.DocumentElement.AppendChild(actions);

    //    //// Create a simple button to display on the toast
    //    //var action = toastXml.CreateElement("action");
    //    //actions.AppendChild(action);
    //    //action.SetAttribute("content", "Show details");
    //    //action.SetAttribute("arguments", "viewdetails");

    //    // Create the toast 
    //    var toast = new ToastNotification(toastXml);

    //    // Show the toast. Be sure to specify the AppUserModelId
    //    // on your application's shortcut!
    //    ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);
    //}
    private ToastNotification? _toast;
    private ToastNotifier _toastNotifier;
    private readonly string _appTitle;

    private Toast(string appTitle) =>
        this._appTitle = appTitle.ArgumentNotNull();

    public static Toast CreateLongContent(string title, string appTitle) =>
        new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastText01, new[] { title })
        };
    public static Toast CreateLongContent(string content, string title, string appTitle) =>
        new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastText02, new[] { title, content })
        };
    public static Toast CreateLongTitle(string content, string title, string appTitle) =>
        new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastText03, new[] { title, content })
        };
    public static Toast CreateMultipleLineContent(string line1, string line2, string title, string appTitle) =>
        new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastText04, new[] { title, line1, line2 })
        };
    public static Toast CreateImageLongTitle(string imagePath, string title, string appTitle) =>
        new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastImageAndText01, new[] { title }, imagePath)
        };
    public static Toast CreateImageLongContent(string imagePath, string title, string content, string appTitle) =>
        new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastImageAndText02, new[] { title, content }, imagePath)
        };
    public static Toast CreateImageLongTitle(string imagePath, string title, string content, string appTitle) =>
        new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastImageAndText03, new[] { title, content }, imagePath)
        };
    public static Toast CreateImageMultipleLineContent(string imagePath, string title, string line1, string line2, string appTitle) =>
        new(appTitle)
        {
            _toast = InnerCreateToast(ToastTemplateType.ToastImageAndText04, new[] { title, line1, line2 }, imagePath)
        };

    [MemberNotNull(nameof(_toastNotifier))]
    public Toast Build() =>
        this.Fluent(this._toastNotifier = ToastNotificationManager.CreateToastNotifier(this._appTitle));

    public void Show()
    {
        if (this._toastNotifier is null)
        {
            this.Build();
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
                stringElements[index].AppendChild(toastXml.CreateTextNode(children[index]));
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
            toastXml.DocumentElement.AppendChild(audio);

        }

        return new ToastNotification(toastXml);
    }
}
