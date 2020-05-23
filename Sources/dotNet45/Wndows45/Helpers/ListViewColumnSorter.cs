using System.Collections;
using System.Windows.Forms;

namespace Mohammad.Win.Helpers
{
    public class ListViewColumnSorter : IComparer
    {
        private int _ColIndex;
        private bool _Enabled;
        public ListView ListView { get; }

        public bool Enabled
        {
            get { return this._Enabled; }
            set
            {
                if (this._Enabled == value)
                    return;
                this._Enabled = value;
                if (this._Enabled)
                {
                    this.ListView.ListViewItemSorter = this;
                    this.ListView.ColumnClick += this.ListViewColumnClick;
                }
                else
                {
                    this.ListView.ListViewItemSorter = null;
                    this.ListView.ColumnClick -= this.ListViewColumnClick;
                }
            }
        }

        public ListViewColumnSorter(ListView listView)
        {
            this.ListView = listView;
            this.Enabled = true;
        }

        private void ListViewColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.ListView.SuspendLayout();
            this._ColIndex = e.Column;
            this.ListView.Sorting = this.ListView.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            if (this.ListView.Items.Count > 0)
                this.ListView.RedrawItems(0, this.ListView.Items.Count - 1, true);
            this.ListView.ResumeLayout();
        }

        #region IComparer Members

        public int Compare(object x, object y)
        {
            var item1 = x as ListViewItem;
            var item2 = y as ListViewItem;
            var result = string.Compare(item1.SubItems.Count > this._ColIndex ? item1.SubItems[this._ColIndex].Text : string.Empty,
                item2.SubItems.Count > this._ColIndex ? item2.SubItems[this._ColIndex].Text : string.Empty);
            return this.ListView.Sorting == SortOrder.Ascending ? result : result *= -1;
        }

        #endregion
    }
}