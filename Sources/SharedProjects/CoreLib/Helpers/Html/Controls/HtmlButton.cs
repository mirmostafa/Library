using System.Windows.Forms;

namespace Mohammad.Helpers.Html.Controls
{
    public static class HtmlButton
    {
        public static void ButtonPerformClick(this HtmlDocument document, string name, string attName) { document.ClickButton(attName, name); }
    }
}