#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Library40.Win.Internals
{
	internal class TabPageSwitcherDesigner : ParentControlDesigner
	{
		private ISelectionService selectionService;
		// service which lets you know when the selection changes in the designer.

		private DesignerVerbCollection verbs;
		// a collection of actions to perform (appear as links in propgrid, on designer action panel)

		/// <summary>
		///     The TabPageSwitcher we're designing - strongly typed wrapper around Component property.
		/// </summary>
		public TabPageSwitcher ControlSwitcher
		{
			get { return this.Component as TabPageSwitcher; }
		}

		/// <summary>
		///     Fetches the selection service from the service provider - from this we can tell what's selected and when selection
		///     changes
		/// </summary>
		internal ISelectionService SelectionService
		{
			get
			{
				if (this.selectionService == null)
				{
					this.selectionService = (ISelectionService)this.GetService(typeof (ISelectionService));
					Debug.Assert(this.selectionService != null, "Failed to get Selection Service!");
				}
				return this.selectionService;
			}
		}

		/// <summary>
		///     List of "verbs" or actions to be used in the designer.  These typically appear on the Context Menu,
		///     links on the property grid, and as links on the designer action panel.
		/// </summary>
		public override DesignerVerbCollection Verbs
		{
			get
			{
				if (this.verbs == null)
				{
					this.verbs = new DesignerVerbCollection();
					this.verbs.Add(new DesignerVerb("Add Tabstrip Page", this.OnAdd));
				}

				return this.verbs;
			}
		}

		/// <summary>
		///     when the designer disposes, we need to be careful about
		///     unhooking from service events we've subscribed to.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
				this.SelectionService.SelectionChanged -= this.SelectionService_SelectionChanged;
		}

		/// <summary>
		///     This is called when the designer is first loaded.
		///     Usually a good time to hook up to events.  If you want to
		///     set property defaults, InitializeNewComponent is what you
		///     want to override
		/// </summary>
		/// <param name="component"></param>
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.SelectionService.SelectionChanged += this.SelectionService_SelectionChanged;
		}

		public override bool CanParent(Control control)
		{
			return control is TabStripPage;
		}

		/// <summary>
		///     Method implementation for our "Add TabStripPage verb".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eevent"></param>
		private void OnAdd(object sender, EventArgs eevent)
		{
			// fetch our designer host
			var host = (IDesignerHost)this.GetService(typeof (IDesignerHost));
			if (host != null)
			{
				// Create a transaction so we're friendly to undo/redo and serialization
				DesignerTransaction t = null;
				try
				{
					try
					{
						t = host.CreateTransaction("Add Tabstrip Page" + this.Component.Site.Name);
					}
					catch (CheckoutException ex)
					{
						if (ex == CheckoutException.Canceled)
							return;
						throw ex;
					}

					// Add a TabStripPage to the controls collection of the TabPageSwitcher

					// Notify the TabPageSwitcher that it's control collection is changing.                    
					MemberDescriptor member = TypeDescriptor.GetProperties(this.ControlSwitcher)["Controls"];
					var page = host.CreateComponent(typeof (TabStripPage)) as TabStripPage;
					this.RaiseComponentChanging(member);

					// add the page to the controls collection.
					this.ControlSwitcher.Controls.Add(page);

					// set the SelectedTabStripPage to the current page so that it opens correctly
					this.SetProperty("SelectedTabStripPage", page);

					// Raise event that we're done changing the controls property.
					this.RaiseComponentChanged(member, null, null);

					// if we have an associated TabStrip,
					// add a matching Tab to the TabStrip.
					if (this.ControlSwitcher.TabStrip != null)
					{
						// add a tab to the toolstrip designer
						MemberDescriptor itemsProp = TypeDescriptor.GetProperties(this.ControlSwitcher.TabStrip)["Items"];

						var tab = host.CreateComponent(typeof (Tab)) as Tab;
						this.RaiseComponentChanging(itemsProp);

						this.ControlSwitcher.TabStrip.Items.Add(tab);
						this.RaiseComponentChanged(itemsProp, null, null);

						this.SetProperty(tab, "DisplayStyle", ToolStripItemDisplayStyle.ImageAndText);
						this.SetProperty(tab, "Text", tab.Name);
						this.SetProperty(tab, "TabStripPage", page);
						this.SetProperty(this.ControlSwitcher.TabStrip, "SelectedTab", tab);
					}
				}
				finally
				{
					if (t != null)
						t.Commit();
				}
			}
		}

		private void SelectionService_SelectionChanged(object sender, EventArgs e)
		{
			var selectedComponents = (IList)this.SelectionService.GetSelectedComponents();
			if (selectedComponents.Count == 1)
			{
				var tab = selectedComponents[0] as Tab;
				if (tab != null)
				{
					this.SetProperty("SelectedTabStripPage", tab.TabStripPage);
					this.SetProperty(tab, "Checked", true);
				}
			}
		}

		private void SetProperty(object target, string propname, object value)
		{
			var propDescriptor = TypeDescriptor.GetProperties(target)[propname];
			if (propDescriptor != null)
				propDescriptor.SetValue(target, value);
		}

		private void SetProperty(string propname, object value)
		{
			this.SetProperty(this.ControlSwitcher, propname, value);
		}
	}
}