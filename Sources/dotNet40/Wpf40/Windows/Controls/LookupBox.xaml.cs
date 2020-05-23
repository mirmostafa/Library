#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using Library40.Wpf.Internals.Control;

namespace Library40.Wpf.Windows.Controls
{
	/// <summary>
	///     Interaction logic for LookupBox.xaml
	/// </summary>
	public partial class LookupBox
	{
		private LookupBoxItemPage _ItemsPage;

		public LookupBox()
		{
			this.InitializeComponent();
		}

		public IEnumerable SelectedItems
		{
			get { return this._ItemsPage.SelectedItems; }
		}
		public IEnumerable ItemsSource { get; set; }

		private void ItemsComboBoxDropDownOpened(object sender, EventArgs e)
		{
			if (this._ItemsPage != null)
				return;
			this._ItemsPage = new LookupBoxItemPage
			                  {
				                  ItemsSource = this.ItemsSource
			                  };
			this._ItemsPage.SelectedItemsChanged += delegate
			                                        {
				                                        //this._ItemsPage	
			                                        };
			this.ItemHostFrame.Navigate(this._ItemsPage);
		}
	}
}