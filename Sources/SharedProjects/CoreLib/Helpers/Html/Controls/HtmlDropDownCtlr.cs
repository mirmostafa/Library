using System;
using System.Windows.Forms;
using mshtml;

namespace Mohammad.Helpers.Html.Controls
{
    public static class HtmlDropDownCtlr
    {
        public static void DropDown_OnSetSelectedIndex(this HtmlDocument document, string name, int index, bool fireOnChange = true)
        {
            document.SetAttribute("name", name, index.ToString(), "select", "selectedIndex");

            if (!fireOnChange)
                return;
            var dropDown = (HTMLSelectElement) document.GetElementById(name).DomElement;
            var doc4 = (IHTMLDocument4) document.DomDocument;
            object eventObject = null;
            eventObject = doc4.CreateEventObject(ref eventObject);
            dropDown.FireEvent("ONCHANGE", ref eventObject);
        }

        public static int DropDown_OnGetSelectedIndex(this HtmlDocument document, string name)
        {
            return Convert.ToInt32(document.GetAttribute("name", name, "select", "selectedIndex"));
        }

        public static bool DropDown_OnGetSelectedValue(this HtmlDocument document, string name, out string value)
        {
            value = "";
            return true;
        }

        public static void DropDown_OnAddItem(this HtmlDocument document, string name, string displayMember, string valueMember, int index)
        {
            var select = (HTMLSelectElement) document.GetElementById(name).DomElement;
            //((mshtml.HTMLSelectElementClass)select.options).children.add(option);
            //((mshtml.HTMLSelectElementClass) select.options).children.add();
            var option = document.CreateElement("option");

            select.options.children.add(option);
        }
    }
}