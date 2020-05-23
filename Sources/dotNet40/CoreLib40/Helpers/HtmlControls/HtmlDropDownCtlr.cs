#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Windows.Forms;
using mshtml;

namespace Library40.Helpers.HtmlControls
{
	public static class HtmlDropDownCtlr
	{
		public static void DropDown_SetSelectedIndex(this HtmlDocument document, string name, int index, bool fireOnChange = true)
		{
			document.SetAttribute("name", name, index.ToString(), "select", "selectedIndex");

			if (!fireOnChange)
				return;
			var dropDown = (HTMLSelectElement)document.GetElementById(name).DomElement;
			var doc4 = (IHTMLDocument4)document.DomDocument;
			object eventObject = null;
			eventObject = doc4.CreateEventObject(ref eventObject);
			dropDown.FireEvent("ONCHANGE", ref eventObject);
		}

		public static int DropDown_GetSelectedIndex(this HtmlDocument document, string name)
		{
			return Convert.ToInt32(document.GetAttribute("name", name, "select", "selectedIndex"));
		}

		public static bool DropDown_GetSelectedValue(this HtmlDocument document, string name, out string value)
		{
			value = "";
			return true;
		}

		public static void DropDown_AddItem(this HtmlDocument document, string name, string displayMember, string valueMember, int index)
		{
			var select = (HTMLSelectElement)document.GetElementById(name).DomElement;
			//((mshtml.HTMLSelectElementClass)select.options).children.add(option);
			//((mshtml.HTMLSelectElementClass) select.options).children.add();
			var option = document.CreateElement("option");

			select.options.children.add(option);
		}
	}
}