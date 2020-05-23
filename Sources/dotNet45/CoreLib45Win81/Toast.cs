using System;
using Windows.UI.Notifications;
using Mohammad.EventsArgs;
using Mohammad.Helpers;

namespace Mohammad.Win81
{
    public class Toast
    {
        private readonly string _ApplicationId;

        private readonly ToastNotification _Toast;

        protected Toast(ToastNotification toast, string applicationId)
        {
            this._Toast = toast;
            this._ApplicationId = applicationId;

            this._Toast.Activated += this.ToastNotification_Activated;
            this._Toast.Dismissed += this.ToastNotification_Dismissed;
            this._Toast.Failed += this.ToastNotification_Failed;
        }

        protected static Toast GetToast(ToastTemplateType template, string applicationId, Uri image, params string[] lines)
        {
            var toastXml = ToastNotificationManager.GetTemplateContent(template);
            var toastNotification = new ToastNotification(toastXml);

            if (lines.Length > 0)
            {
                var stringElements = toastNotification.Content.GetElementsByTagName("text");
                for (var index = 0; index < lines.Length; index++)
                    stringElements[index].AppendChild(toastNotification.Content.CreateTextNode(lines[index]));
            }
            if (image != null)
            {
                var imageElements = toastNotification.Content.GetElementsByTagName("image");
                imageElements[0].Attributes[1].NodeValue = image.ToString();
            }

            return new Toast(toastNotification, applicationId);
        }

        public event EventHandler<ItemActedEventArgs<Exception>> Failed;
        public event EventHandler<ItemActedEventArgs<ToastDismissalReason>> Dismissed;
        public event EventHandler Activated;

        private void ToastNotification_Failed(ToastNotification sender, ToastFailedEventArgs args)
        {
            this.OnFailed(new ItemActedEventArgs<Exception>(args.ErrorCode));
        }

        private void ToastNotification_Dismissed(ToastNotification sender, ToastDismissedEventArgs args)
        {
            this.OnDismissed(new ItemActedEventArgs<ToastDismissalReason>((ToastDismissalReason) args.Reason));
        }

        private void ToastNotification_Activated(ToastNotification sender, object args) { this.OnActivated(); }

        public static void Show(string applicationId, string caption) { Get(applicationId, caption).Show(); }

        public static void Show(string applicationId, Uri image, string caption) { Get(applicationId, image, caption).Show(); }

        public static void Show(string applicationId, string caption, string line) { Get(applicationId, caption, line).Show(); }

        public static void Show(string applicationId, Uri image, string caption, string line) { Get(applicationId, image, caption, line).Show(); }

        public static void Show(string applicationId, string caption, string line1, string line2) { Get(applicationId, caption, line1, line2).Show(); }

        public static void Show(string applicationId, Uri image, string caption, string line1, string line2)
        {
            Get(applicationId, image, caption, line1, line2).Show();
        }

        public static Toast Get(string applicationId, string caption) => GetToast(ToastTemplateType.ToastText01, applicationId, null, caption);
        public static Toast Get(string applicationId, Uri image, string caption) => GetToast(ToastTemplateType.ToastImageAndText01, applicationId, null, caption);

        public static Toast Get(string applicationId, string caption, string line) => GetToast(ToastTemplateType.ToastText03, applicationId, null, caption, line);

        public static Toast Get(string applicationId, Uri image, string caption, string line)
            => GetToast(ToastTemplateType.ToastImageAndText03, applicationId, image, caption, line);

        public static Toast Get(string applicationId, string caption, string line1, string line2)
            => GetToast(ToastTemplateType.ToastText04, applicationId, null, caption, line1, line2);

        public static Toast Get(string applicationId, Uri image, string caption, string line1, string line2)
            => GetToast(ToastTemplateType.ToastImageAndText04, applicationId, image, caption, line1, line2);

        public void Show()
        {
            ToastNotificationManager.CreateToastNotifier(this._ApplicationId ?? (ApplicationHelper.ApplicationTitle ?? "Application")).Show(this._Toast);
        }

        protected virtual void OnFailed(ItemActedEventArgs<Exception> e) { this.Failed?.Invoke(this, e); }

        protected virtual void OnDismissed(ItemActedEventArgs<ToastDismissalReason> e) { this.Dismissed?.Invoke(this, e); }

        protected virtual void OnActivated() { this.Activated?.Invoke(this, EventArgs.Empty); }

        public enum ToastDismissalReason
        {
            UserCanceled,
            ApplicationHidden,
            TimedOut
        }
    }
}