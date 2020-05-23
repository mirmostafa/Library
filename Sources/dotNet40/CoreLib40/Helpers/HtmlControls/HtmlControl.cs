#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Library40.Helpers.HtmlControls
{
	public static class HtmlControl
	{
		public static void TextBox_SetText(this HtmlDocument document, string attName, string attValue, string value)
		{
			var tags = document.GetElementsByTagName("input").Cast<HtmlElement>();
			foreach (var tag in tags.Where(tag => tag.GetAttribute(attName).Equals(attValue)))
				tag.SetAttribute("value", value);
		}

		internal static void SetAttribute(this HtmlDocument document, string attName, string attValue, string value, string tagName = "input", string attributeName = "value")
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
				foreach (var tag in tags.Where(tag => tag.GetAttribute(attName).Equals(attValue)))
					tag.InvokeMember("click");
			else
			{
				var element = tags.Where(tag => tag.GetAttribute(attName).Equals(attValue)).FirstOrDefault();
				if (element != null)
					element.InvokeMember("click");
			}
		}

		public static IEnumerable<HtmlElement> HtmlH1_GetAll(this HtmlDocument document)
		{
			return document.GetElementsByTagName("h1").Cast<HtmlElement>();
		}

		public static IEnumerable<HtmlElement> HtmlH2_GetAll(this HtmlDocument document)
		{
			return document.GetElementsByTagName("h2").Cast<HtmlElement>();
		}

		public static IEnumerable<HtmlElement> HtmlSpan_GetAll(this HtmlDocument document)
		{
			return document.GetElementsByTagName("span").Cast<HtmlElement>();
		}

		public static HtmlElement HtmlSpan_GetByAttribute(this HtmlDocument document, string attName, string attValue)
		{
			return document.GetElementsByTagName("span").Cast<HtmlElement>().Where(el => el.GetAttribute(attName).ToLower().Equals(attValue.ToLower())).FirstOrDefault();
		}
	}
}