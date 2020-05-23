#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Library35.Helpers;
using Library35.Windows.EventsArgs;
using Library35.Windows.Helpers;

namespace Library35.Windows.Controls
{
	public partial class ListView : System.Windows.Forms.ListView
	{
		private const string _Reorder = "Reorder";
		private readonly ListViewColumnSorter _Sorter;
		private bool _AllowRowReorder = true;
		private string _DragDataFormat = DataFormats.FileDrop;

		public ListView()
		{
			this.InitializeComponent();
			this.Printer = new ListViewPrinter(this);
			this._Sorter = new ListViewColumnSorter(this);
			this.Sortable = false;
			this.AllowRowReorder = true;
		}

		[DefaultValue("FileDrop")]
		public string DragDataFormat
		{
			get { return this._DragDataFormat; }
			set { this._DragDataFormat = value; }
		}

		[DefaultValue(false)]
		public bool Sortable
		{
			get { return this._Sorter.Enabled; }
			set { this._Sorter.Enabled = value; }
		}

		[Browsable(true)]
		public ListViewPrinter Printer { get; private set; }

		[DefaultValue(true)]
		public bool AllowRowReorder
		{
			get { return this._AllowRowReorder; }
			set
			{
				this._AllowRowReorder = value;
				base.AllowDrop = value;
			}
		}

		public new SortOrder Sorting
		{
			get { return SortOrder.None; } // ReSharper disable ValueParameterNotUsed
			set // ReSharper restore ValueParameterNotUsed
			{ base.Sorting = SortOrder.None; }
		}

		[DefaultValue(false)]
		public bool IsSelectAllEnabled { get; set; }

		public event EventHandler<AddingDraggedItemsEventArgs> AddingDraggedItems;

		protected virtual void OnAddingDraggedItems(AddingDraggedItemsEventArgs e)
		{
			if (this.AddingDraggedItems != null)
				this.AddingDraggedItems(this, e);
		}

		public new DragDropEffects DoDragDrop(Object data, DragDropEffects allowedEffects = DragDropEffects.Copy)
		{
			this.OnAddingDraggedItems(new AddingDraggedItemsEventArgs((IEnumerable<string>)data));
			return DragDropEffects.Copy;
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
				var group = this.Groups.Cast<ListViewGroup>().FirstOrDefault(g => g.Header.Equals(groupName));
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
			e.Effect = e.Data.GetDataPresent(this.DragDataFormat) ? DragDropEffects.Copy : (e.Data.GetDataPresent("FileGroupDescriptor") ? DragDropEffects.Copy : DragDropEffects.None);
		}

		protected override void OnItemDrag(ItemDragEventArgs e)
		{
			base.OnItemDrag(e);
			if (!this.AllowRowReorder)
				return;
			this.DoDragDrop(_Reorder, DragDropEffects.Move);
		}

		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);
			if (!e.Data.GetDataPresent(this.DragDataFormat, false))
				return;
			var data = (string[])e.Data.GetData(this.DragDataFormat);
			this.OnAddingDraggedItems(new AddingDraggedItemsEventArgs(data));
		}

		private void ListView_KeyUp(object sender, KeyEventArgs e)
		{
			if (!this.MultiSelect || !this.IsSelectAllEnabled)
				return;
			if (e.Control && e.KeyCode == Keys.A)
				this.SelectAll();
		}

		public void SelectAll()
		{
			this.BeginUpdate();
			foreach (ListViewItem item in this.Items)
				item.Selected = true;
			this.EndUpdate();
			this.Update();
		}
	}
}