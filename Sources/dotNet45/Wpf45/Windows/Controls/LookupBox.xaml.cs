using System;
using System.Collections;

namespace Mohammad.Wpf.Windows.Controls
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

        public IEnumerable SelectedItems => this._ItemsPage.SelectedItems;
        public IEnumerable ItemsSource { get; set; }

        private void ItemsComboBoxDropDownOpened(object sender, EventArgs e)
        {
            if (this._ItemsPage != null)
            {
                return;
            }

            this._ItemsPage = new LookupBoxItemPage {ItemsSource = this.ItemsSource};
            this._ItemsPage.SelectedItemsChanged += delegate
            {
                //this._ItemsPage	
            };
            this.ItemHostFrame.Navigate(this._ItemsPage);
        }
    }
}