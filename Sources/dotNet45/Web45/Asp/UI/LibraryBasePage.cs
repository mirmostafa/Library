namespace Mohammad.Web.Asp.UI
{
    public abstract class LibraryBasePage : BasePage
    {
        protected abstract void ShowMessage(string message, string title, string options);
        protected void ShowInfo(string message, string title = null) { this.ShowMessage(message, title ?? this.Title, "info | ok"); }
        protected void ShowError(string message, string title = null) { this.ShowMessage(message, title ?? this.Title, "warn | ok"); }
        protected void ShowWarning(string message, string title = null) { this.ShowMessage(message, title ?? this.Title, "error | ok"); }
        protected void ShowQuestion(string message, string title = null) { this.ShowMessage(message, title ?? this.Title, "question | yes | no"); }
    }
}