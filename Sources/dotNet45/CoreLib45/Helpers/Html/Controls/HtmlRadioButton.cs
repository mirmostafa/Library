#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Windows.Forms;

namespace Mohammad.Helpers.Html.Controls
{
    public static class HtmlRadioButton
    {
        public static void SetChecked(this HtmlDocument document, string name, string attValue, bool check)
        {
            document.ClickButton(name, attValue, false);
        }
    }
}