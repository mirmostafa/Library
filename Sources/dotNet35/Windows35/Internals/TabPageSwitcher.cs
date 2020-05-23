#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Library35.Windows.Controls;

namespace Library35.Windows.Internals
{
	/// <summary>
	///     The TabPageSwitcher works on Z-Order principals - it Dock.Fills all
	///     the contents and uses BringToFront to quickly bring a page to the front
	/// </summary>
	[Designer(typeof (TabPageSwitcherDesigner))] // specify a custom designer
	[DesignerCategory("code")]
	// when this file is opened, open the editor instead of the designer
	[Docking(DockingBehavior.AutoDock)] // when this control is dropped onto the form, try to Dock.Fill
	[ToolboxItem(false)] // dont show this control in the toolbox, it will be added by the TabStripToolBoxItem
	public class TabPageSwitcher : ContainerControl
	{
		private TabStripPage selectedTabStripPage;

		public TabPageSwitcher()
		{
			this.ResetBackColor();
		}

		/// <summary>
		///     specify the default size for the control
		/// </summary>
		protected override Size DefaultSize
		{
			get { return new Size(100, 60); }
		}

		/// <summary>
		///     specify a default padding to give a border around the control
		/// </summary>
		protected override Padding DefaultPadding
		{
			get { return new Padding(4, 0, 4, 2); }
		}

		/// <summary>
		///     The associated TabStrip
		/// </summary>
		public TabStrip TabStrip { get; set; }

		/// <summary>
		///     Specify the selected TabStripPage.
		/// </summary>
		public TabStripPage SelectedTabStripPage
		{
			get { return this.selectedTabStripPage; }
			set
			{
				if (this.selectedTabStripPage != value)
				{
					this.selectedTabStripPage = value;

					if (this.selectedTabStripPage != null)
						if (!this.Controls.Contains(value))
							this.Controls.Add(this.selectedTabStripPage);
						else
							this.selectedTabStripPage.BringToFront();
				}
			}
		}

		/// <summary>
		///     Expose a Load event
		/// </summary>
		public event EventHandler Load;

		/// <summary>
		///     Occurs when the selected tab has changed
		/// </summary>
		public event EventHandler SelectedTabStripPageChanged;

		/// <summary>
		///     Handle the OnHandleCreated event to fire the Load event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (!this.RecreatingHandle)
				this.OnLoad(EventArgs.Empty);
		}

		/// <summary>
		///     When the control is loaded, if we dont have a SelectedTabStripPage, look for one to activate.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnLoad(EventArgs e)
		{
			if (this.Load != null)
				this.Load(this, e);

			if (!this.DesignMode)
				if (this.TabStrip.Items.Count != 0)
				{
					this.TabStrip.SelectedTab = (Tab)this.TabStrip.Items[0];
					((Tab)this.TabStrip.Items[0]).b_active = true;
				}
		}

		/// <summary>
		///     Override OnControlAdded, all controls added to this control should become Dock.Fill.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnControlAdded(ControlEventArgs e)
		{
			e.Control.Dock = DockStyle.Fill;
			base.OnControlAdded(e);
		}

		/// <summary>
		///     occurs when the selected tab has changed
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSelectedTabStripPageChanged(EventArgs e)
		{
			if (this.SelectedTabStripPageChanged != null)
				this.SelectedTabStripPageChanged(this, EventArgs.Empty);
		}

		#region DesignerSerialization Friendliness
		private bool ShouldSerializeBackColor()
		{
			return this.BackColor != Color.FromArgb(191, 219, 255);
		}

		public override void ResetBackColor()
		{
			this.BackColor = Color.FromArgb(191, 219, 255);
		}
		#endregion
	}
}