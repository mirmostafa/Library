using System.ComponentModel;
using System.Windows.Forms;
using Mohammad.Win.Controls;

namespace Mohammad.Win.Internals
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
                catch {}
            }
        }
    }
}