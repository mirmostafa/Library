using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Mohammad.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for LookupBoxItemPage.xaml
    /// </summary>
    public partial class LookupBoxItemPage
    {
        private IEnumerable _ItemsSource;
        private IList _SelectedItems;

        [Localizability(LocalizationCategory.NeverLocalize)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable ItemsSource
        {
            set
            {
                if (this._ItemsSource.Equals(value))
                    return;
                this._ItemsSource = value;
                this.BindData();
            }
            get { return this._ItemsSource; }
        }

        public bool OwnedBindData { get; set; }
        public IEnumerable SelectedItems { get { return this._SelectedItems != null ? (IEnumerable) this._SelectedItems : Enumerable.Empty<object>(); } }
        public LookupBoxItemPage() { this.InitializeComponent(); }
        public void Connect(int connectionId, object target) { }
        public event EventHandler BindingData;
        public event EventHandler BoundData;
        public void OnBoundData(EventArgs e) { this.BoundData.Raise(this, TaskScheduler.FromCurrentSynchronizationContext()); }
        protected virtual void OnBindingData(EventArgs e) { this.BindingData.Raise(this, TaskScheduler.FromCurrentSynchronizationContext()); }

        public void BindData()
        {
            this.OnBindingData(EventArgs.Empty);
            if (!this.OwnedBindData)
                this.ItemsDataGrid.ItemsSource = this.ItemsSource;
            this.OnBoundData(EventArgs.Empty);
        }

        private void ItemsDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._SelectedItems = e.AddedItems;
            this.OnSelectedItemsChanged(EventArgs.Empty);
        }

        public event EventHandler SelectedItemsChanged;
        protected virtual void OnSelectedItemsChanged(EventArgs e) { this.SelectedItemsChanged.RaiseAsync(this); }
    }
}