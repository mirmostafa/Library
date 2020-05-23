using System.Windows.Forms;

namespace Mohammad.Helpers.Html.Controls
{
    public static class HtmlRadioButton
    {
        public static void SetChecked(this HtmlDocument document, string name, string attValue, bool check) { document.ClickButton(name, attValue, false); }
    }
}