using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Mohammad.Helpers.Html.Controls
{
    public static class HtmlControl
    {
        public static void TextBoxSetText(this HtmlDocument document, string attName, string attValue, string value)
        {
            var tags = document.GetElementsByTagName("input").Cast<HtmlElement>();
            foreach (var tag in tags.Where(tag => tag.GetAttribute(attName).Equals(attValue)))
                tag.SetAttribute("value", value);
        }

        internal static void SetAttribute(this HtmlDocument document, string attName, string attValue, string value, string tagName = "input",
            string attributeName = "value")
        {
            var tags = document.GetElementsByTagName(tagName).Cast<HtmlElement>();
            foreach (var tag in tags.Where(tag => tag.GetAttribute(attName).Equals(attValue)))
                tag.SetAttribute(attributeName, value);
        }

        internal static string GetAttribute(this HtmlDocument document, string attName, string attValue, string tagName = "input", string attributeName = "value")
        {
            return
                document.GetElementsByTagName(tagName)
                    .Cast<HtmlElement>()
                    .Where(tag => tag.GetAttribute(attName).Equals(attValue))
                    .Select(tag => tag.GetAttribute(attributeName))
                    .FirstOrDefault();
        }

        internal static void InvokeMember(this HtmlDocument document, string attName, string attValue, string tagName = "input", string methodName = "click")
        {
            var tags = document.GetElementsByTagName(tagName).Cast<HtmlElement>();
            foreach (var tag in tags.Where(tag => tag.GetAttribute(attName).Equals(attValue)))
                tag.InvokeMember(methodName);
        }

        internal static void ClickButton(this HtmlDocument document, string attName, string attValue, bool all = true)
        {
            var tags = document.GetElementsByTagName("input").Cast<HtmlElement>();
            if (all)
            {
                foreach (var tag in tags.Where(tag => tag.GetAttribute(attName).Equals(attValue)))
                    tag.InvokeMember("click");
            }
            else
            {
                var element = tags.FirstOrDefault(tag => tag.GetAttribute(attName).Equals(attValue));
                if (element != null)
                    element.InvokeMember("click");
            }
        }

        public static IEnumerable<HtmlElement> HtmlH1GetAll(this HtmlDocument document) { return document.GetElementsByTagName("h1").Cast<HtmlElement>(); }
        public static IEnumerable<HtmlElement> HtmlH2GetAll(this HtmlDocument document) { return document.GetElementsByTagName("h2").Cast<HtmlElement>(); }
        public static IEnumerable<HtmlElement> HtmlSpanGetAll(this HtmlDocument document) { return document.GetElementsByTagName("span").Cast<HtmlElement>(); }

        public static HtmlElement HtmlSpanGetByAttribute(this HtmlDocument document, string attName, string attValue)
        {
            return document.GetElementsByTagName("span").Cast<HtmlElement>().FirstOrDefault(el => el.GetAttribute(attName).ToLower().Equals(attValue.ToLower()));
        }
    }
}