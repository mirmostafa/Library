#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Windows.Forms;

namespace Library40.Helpers.HtmlControls
{
	public static class HtmlButton
	{
		public static void ButtonPerformClick(this HtmlDocument document, string name, string attName)
		{
			document.ClickButton(attName, name);
		}
	}
}