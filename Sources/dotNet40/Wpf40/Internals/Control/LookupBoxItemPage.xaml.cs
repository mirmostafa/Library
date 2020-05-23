#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Library40.Helpers;

namespace Library40.Wpf.Internals.Control
{
	/// <summary>
	///     Interaction logic for LookupBoxItemPage.xaml
	/// </summary>
	public partial class LookupBoxItemPage
	{
		private IEnumerable _ItemsSource;
		private IList _SelectedItems;

		public LookupBoxItemPage()
		{
			this.InitializeComponent();
		}

		[Localizability(LocalizationCategory.NeverLocalize)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IEnumerable ItemsSource
		{
			set
			{
				if (this._ItemsSource == value)
					return;
				this._ItemsSource = value;
				this.BindData();
			}
			get { return this._ItemsSource; }
		}
		public bool OwnedBindData { get; set; }
		public IEnumerable SelectedItems
		{
			get { return this._SelectedItems != null ? (IEnumerable)this._SelectedItems : Enumerable.Empty<Object>(); }
		}
		public event EventHandler BindingData;
		public event EventHandler BoundData;

		public void OnBoundData(EventArgs e)
		{
			this.BoundData.Raise(this, TaskScheduler.FromCurrentSynchronizationContext());
		}

		protected virtual void OnBindingData(EventArgs e)
		{
			this.BindingData.Raise(this, TaskScheduler.FromCurrentSynchronizationContext());
		}

		public void BindData()
		{
			this.OnBindingData(EventArgs.Empty);
			if (!this.OwnedBindData)
				this.ItemsDataGrid.ItemsSource = this.ItemsSource;
			this.OnBoundData(EventArgs.Empty);
		}

		private void ItemsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this._SelectedItems = e.AddedItems;
			this.OnSelectedItemsChanged(EventArgs.Empty);
		}

		public event EventHandler SelectedItemsChanged;

		protected virtual void OnSelectedItemsChanged(EventArgs e)
		{
			this.SelectedItemsChanged.Raise(this);
		}
	}
}