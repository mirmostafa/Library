#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Windows.Forms;

namespace Mohammad.Helpers.Html.Controls
{
    public static class HtmlButton
    {
        public static void ButtonPerformClick(this HtmlDocument document, string name, string attName)
        {
            document.ClickButton(attName, name);
        }
    }
}