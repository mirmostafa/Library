#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.ComponentModel;
using System.Windows.Forms;
using Library40.Win.Controls;

namespace Library40.Win.Internals
{
	[ToolboxItem(false)]
	// dont show up in the toolbox, this will be created by the Add TabStripPage verb on the TabPageSwitcherDesigner
	[Docking(DockingBehavior.Never)] // dont ask about docking
	[DesignerCategory("Code")] // dont bring up the component designer when opened
	public class TabStripPage : RibbonPanel
	{
		/// <summary>
		///     Bring this TabStripPage to the front of the switcher.
		/// </summary>
		public void Activate()
		{
			var tabPageSwitcher = this.Parent as TabPageSwitcher;
			if (tabPageSwitcher != null)
			{
				tabPageSwitcher.SelectedTabStripPage = this;

				try
				{
					var x0 = tabPageSwitcher.TabStrip.SelectedTab.Bounds.Location.X;
					var xf = tabPageSwitcher.TabStrip.SelectedTab.Bounds.Right;
					tabPageSwitcher.SelectedTabStripPage.LinePos(x0, xf, true);
				}
				catch
				{
				}
			}
		}
	}
}