using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Mohammad.Win.EventsArgs;
using Mohammad.Win.Helpers;

namespace Mohammad.Win.Controls
{
    public partial class ListView : System.Windows.Forms.ListView
    {
        private const string _Reorder = "Reorder";
        private readonly ListViewColumnSorter _Sorter;
        private bool _AllowRowReorder = true;

        public ListView()
        {
            this.InitializeComponent();
            this.Printer = new ListViewPrinter(this);
            this._Sorter = new ListViewColumnSorter(this);
            this.Sortable = false;
            this.AllowRowReorder = true;
        }

        [DefaultValue("FileDrop")]
        public string DragDataFormat { get; set; } = DataFormats.FileDrop;

        [DefaultValue(false)]
        public bool Sortable
        {
            get => this._Sorter.Enabled;
            set => this._Sorter.Enabled = value;
        }

        [Browsable(true)]
        public ListViewPrinter Printer { get; }

        [DefaultValue(true)]
        public bool AllowRowReorder
        {
            get => this._AllowRowReorder;
            set
            {
                this._AllowRowReorder = value;
                this.AllowDrop = value;
            }
        }

        public new SortOrder Sorting
        {
            get => SortOrder.None; // ReSharper disable ValueParameterNotUsed
            set // ReSharper restore ValueParameterNotUsed
                => base.Sorting = SortOrder.None;
        }

        [DefaultValue(false)]
        public bool IsSelectAllEnabled { get; set; }

        public new DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects = DragDropEffects.Copy)
        {
            this.OnAddingDraggedItems(new AddingDraggedItemsEventArgs((IEnumerable<string>)data));
            return DragDropEffects.Copy;
        }

        public void SelectAll()
        {
            this.BeginUpdate();
            foreach (ListViewItem item in this.Items)
            {
                item.Selected = true;
            }

            this.EndUpdate();
            this.Update();
        }

        protected virtual void OnAddingDraggedItems(AddingDraggedItemsEventArgs e)
        {
            if (this.AddingDraggedItems != null)
            {
                this.AddingDraggedItems(this, e);
            }
        }

        protected override void OnColumnClick(ColumnClickEventArgs e)
        {
            base.OnColumnClick(e);
            //this.GroupColumnIndex = e.Column;
            this.Sort();
        }

        protected virtual void RearrangeInGroup(int columnIndex)
        {
            foreach (var item in this.Items.Cast<ListViewItem>())
            {
                var groupName = item.SubItems[columnIndex].Text;
                var group = this.Groups.Cast<ListViewGroup>().Where(g => g.Header.Equals(groupName)).FirstOrDefault();
                if (group == null)
                {
                    group = new ListViewGroup(groupName);
                    this.Groups.Add(group);
                }

                item.Group = group;
            }
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            e.Effect = e.Data.GetDataPresent(this.DragDataFormat)
                ? DragDropEffects.Copy
                : e.Data.GetDataPresent("FileGroupDescriptor")
                    ? DragDropEffects.Copy
                    : DragDropEffects.None;
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);
            if (!this.AllowRowReorder)
            {
                return;
            }

            this.DoDragDrop(_Reorder, DragDropEffects.Move);
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);
            if (!e.Data.GetDataPresent(this.DragDataFormat, false))
            {
                return;
            }

            var data = (string[])e.Data.GetData(this.DragDataFormat);
            this.OnAddingDraggedItems(new AddingDraggedItemsEventArgs(data));
        }

        private void ListView_KeyUp(object sender, KeyEventArgs e)
        {
            if (!this.MultiSelect || !this.IsSelectAllEnabled)
            {
                return;
            }

            if (e.Control && e.KeyCode == Keys.A)
            {
                this.SelectAll();
            }
        }

        public event EventHandler<AddingDraggedItemsEventArgs> AddingDraggedItems;
    }
}