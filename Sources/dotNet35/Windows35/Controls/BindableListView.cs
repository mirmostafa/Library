#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Library35.EventsArgs;
using Library35.Exceptions;
using Library35.Helpers;

namespace Library35.Windows.Controls
{
	/*
     * Columns must be added by smart-tag.
     */

	public partial class BindableListView : ListView, IBindingListView
	{
		private object _DataSource;

		public BindableListView()
		{
			this.InitializeComponent();
			this.EditOnDoubleClick = true;
			this.DeleteOnShiftDelPressed = false;
			this.AutoRebind = true;
		}

		public IEnumerable<ListViewItem> ListViewItems
		{
			get { return this.Items.Cast<ListViewItem>(); }
		}

		public object DataSource
		{
			get { return this._DataSource; }
			set
			{
				if (this._DataSource == value)
					return;
				if (value is BindingSource)
					(value as BindingSource).DataSourceChanged += (p1, p2) => this.Rebind();
				this._DataSource = value;
				this.Rebind();
			}
		}

		[DefaultValue(false)]
		public bool AutoGenerateColumns { get; set; }

		[DefaultValue(true)]
		public bool AutoRebind { get; set; }

		public object SelectedEntity
		{
			get { return this.SelectedItems.Count > 0 ? this.SelectedEntities.ElementAt(0) : null; }
		}

		public IEnumerable SelectedEntities
		{
			get { return this.SelectedItems.Cast<ListViewItem>().Select(item => item.Tag); }
		}

		[DefaultValue(true)]
		public bool EditOnDoubleClick { get; set; }

		[DefaultValue(false)]
		public bool DeleteOnShiftDelPressed { get; set; }

		public IEnumerable<Object> Entities
		{
			get { return this.ListViewItems.Select(item => item.Tag); }
		}

		#region IBindingListView Members
		public event ListChangedEventHandler ListChanged;

		/// <summary>
		///     Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public IEnumerator GetEnumerator()
		{
			return this.Entities.GetEnumerator();
		}

		/// <summary>
		///     Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />,
		///     starting at a particular
		///     <see
		///         cref="T:System.Array" />
		///     index.
		/// </summary>
		/// <param name="array">
		///     The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from
		///     <see
		///         cref="T:System.Collections.ICollection" />
		///     . The <see cref="T:System.Array" /> must have zero-based indexing.
		/// </param>
		/// <param name="index">
		///     The zero-based index in <paramref name="array" /> at which copying begins.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		///     <paramref name="array" /> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///     <paramref name="index" /> is less than zero.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		///     <paramref name="array" /> is multidimensional.-or-
		///     <paramref
		///         name="index" />
		///     is equal to or greater than the length of <paramref name="array" />.-or- The number of elements in the source
		///     <see
		///         cref="T:System.Collections.ICollection" />
		///     is greater than the available space from <paramref name="index" /> to the end of the destination
		///     <paramref
		///         name="array" />
		///     .
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		///     The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of
		///     the destination
		///     <paramref
		///         name="array" />
		///     .
		/// </exception>
		/// <filterpriority>2</filterpriority>
		public void CopyTo(Array array, int index)
		{
			array.CopyTo(this.Entities.ToArray(), index);
		}

		/// <summary>
		///     Gets the number of elements contained in the <see cref="T:System.Collections.ICollection" />.
		/// </summary>
		/// <returns>
		///     The number of elements contained in the <see cref="T:System.Collections.ICollection" />.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public int Count
		{
			get { return this.Entities.Count(); }
		}

		/// <summary>
		///     Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
		/// </summary>
		/// <returns>
		///     An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		object ICollection.SyncRoot
		{
			get { return null; }
		}

		/// <summary>
		///     Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized
		///     (thread safe).
		/// </summary>
		/// <returns>
		///     true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise,
		///     false.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		/// <summary>
		///     Adds an item to the <see cref="T:System.Collections.IList" />.
		/// </summary>
		/// <returns>
		///     The position into which the new element was inserted.
		/// </returns>
		/// <param name="value">
		///     The <see cref="T:System.Object" /> to add to the <see cref="T:System.Collections.IList" />.
		/// </param>
		/// <exception cref="T:System.NotSupportedException">
		///     The <see cref="T:System.Collections.IList" /> is read-only.-or- The
		///     <see
		///         cref="T:System.Collections.IList" />
		///     has a fixed size.
		/// </exception>
		/// <filterpriority>2</filterpriority>
		public int Add(object value)
		{
			return this.CoreInsertItem(value, null);
		}

		/// <summary>
		///     Determines whether the <see cref="T:System.Collections.IList" /> contains a specific value.
		/// </summary>
		/// <returns>
		///     true if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Collections.IList" />; otherwise,
		///     false.
		/// </returns>
		/// <param name="value">
		///     The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />.
		/// </param>
		/// <filterpriority>2</filterpriority>
		public bool Contains(object value)
		{
			return this.Entities.Contains(value);
		}

		/// <summary>
		///     Determines the index of a specific item in the <see cref="T:System.Collections.IList" />.
		/// </summary>
		/// <returns>
		///     The index of <paramref name="value" /> if found in the list; otherwise, -1.
		/// </returns>
		/// <param name="value">
		///     The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />.
		/// </param>
		/// <filterpriority>2</filterpriority>
		public int IndexOf(object value)
		{
			var result = this.GetListViewItemByData(value);
			return result != null ? result.Index : -1;
		}

		/// <summary>
		///     Inserts an item to the <see cref="T:System.Collections.IList" /> at the specified index.
		/// </summary>
		/// <param name="index">
		///     The zero-based index at which <paramref name="value" /> should be inserted.
		/// </param>
		/// <param name="value">
		///     The <see cref="T:System.Object" /> to insert into the <see cref="T:System.Collections.IList" />.
		/// </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///     <paramref name="index" /> is not a valid index in the
		///     <see
		///         cref="T:System.Collections.IList" />
		///     .
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		///     The <see cref="T:System.Collections.IList" /> is read-only.-or- The
		///     <see
		///         cref="T:System.Collections.IList" />
		///     has a fixed size.
		/// </exception>
		/// <exception cref="T:System.NullReferenceException">
		///     <paramref name="value" /> is null reference in the
		///     <see
		///         cref="T:System.Collections.IList" />
		///     .
		/// </exception>
		/// <filterpriority>2</filterpriority>
		public void Insert(int index, object value)
		{
			this.CoreInsertItem(value, index);
		}

		/// <summary>
		///     Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList" />.
		/// </summary>
		/// <param name="value">
		///     The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Collections.IList" />.
		/// </param>
		/// <exception cref="T:System.NotSupportedException">
		///     The <see cref="T:System.Collections.IList" /> is read-only.-or- The
		///     <see
		///         cref="T:System.Collections.IList" />
		///     has a fixed size.
		/// </exception>
		/// <filterpriority>2</filterpriority>
		public void Remove(object value)
		{
			this.Items.Remove(this.GetListViewItemByData(value));
		}

		/// <summary>
		///     Removes the <see cref="T:System.Collections.IList" /> item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///     <paramref name="index" /> is not a valid index in the
		///     <see
		///         cref="T:System.Collections.IList" />
		///     .
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		///     The <see cref="T:System.Collections.IList" /> is read-only.-or- The
		///     <see
		///         cref="T:System.Collections.IList" />
		///     has a fixed size.
		/// </exception>
		/// <filterpriority>2</filterpriority>
		public void RemoveAt(int index)
		{
			this.Items.RemoveAt(index);
		}

		/// <summary>
		///     Gets or sets the element at the specified index.
		/// </summary>
		/// <returns>
		///     The element at the specified index.
		/// </returns>
		/// <param name="index">The zero-based index of the element to get or set. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///     <paramref name="index" /> is not a valid index in the
		///     <see
		///         cref="T:System.Collections.IList" />
		///     .
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		///     The property is set and the <see cref="T:System.Collections.IList" /> is read-only.
		/// </exception>
		/// <filterpriority>2</filterpriority>
		public object this[int index]
		{
			get { return this.Items[index].Tag; }
			set { throw new NotSupportedException(); }
		}

		/// <summary>
		///     Gets a value indicating whether the <see cref="T:System.Collections.IList" /> is read-only.
		/// </summary>
		/// <returns>
		///     true if the <see cref="T:System.Collections.IList" /> is read-only; otherwise, false.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		bool IList.IsReadOnly
		{
			get { return true; }
		}

		/// <summary>
		///     Gets a value indicating whether the <see cref="T:System.Collections.IList" /> has a fixed size.
		/// </summary>
		/// <returns>
		///     true if the <see cref="T:System.Collections.IList" /> has a fixed size; otherwise, false.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		bool IList.IsFixedSize
		{
			get { return false; }
		}

		/// <summary>
		///     Adds a new item to the list.
		/// </summary>
		/// <returns>
		///     The item added to the list.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">
		///     <see cref="P:System.ComponentModel.IBindingList.AllowNew" /> is false.
		/// </exception>
		public object AddNew()
		{
			var e = new AddingNewEventArgs();
			return e.NewObject ?? (this.Entities.Any()
				? this.Entities.ElementAt(0).GetType().GetConstructor(new Type[]
				                                                      {
				                                                      }).Invoke(new object[]
				                                                                {
				                                                                })
				: null);
		}

		/// <summary>
		///     Adds the <see cref="T:System.ComponentModel.PropertyDescriptor" /> to the indexes used for searching.
		/// </summary>
		/// <param name="property">
		///     The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to add to the indexes used for searching.
		/// </param>
		void IBindingList.AddIndex(PropertyDescriptor property)
		{
		}

		/// <summary>
		///     Sorts the list based on a <see cref="T:System.ComponentModel.PropertyDescriptor" /> and a
		///     <see
		///         cref="T:System.ComponentModel.ListSortDirection" />
		///     .
		/// </summary>
		/// <param name="property">
		///     The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to sort by.
		/// </param>
		/// <param name="direction">
		///     One of the <see cref="T:System.ComponentModel.ListSortDirection" /> values.
		/// </param>
		/// <exception cref="T:System.NotSupportedException">
		///     <see cref="P:System.ComponentModel.IBindingList.SupportsSorting" /> is false.
		/// </exception>
		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
		}

		/// <summary>
		///     Returns the index of the row that has the given <see cref="T:System.ComponentModel.PropertyDescriptor" />.
		/// </summary>
		/// <returns>
		///     The index of the row that has the given <see cref="T:System.ComponentModel.PropertyDescriptor" />.
		/// </returns>
		/// <param name="property">
		///     The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to search on.
		/// </param>
		/// <param name="key">
		///     The value of the <paramref name="property" /> parameter to search for.
		/// </param>
		/// <exception cref="T:System.NotSupportedException">
		///     <see cref="P:System.ComponentModel.IBindingList.SupportsSearching" /> is false.
		/// </exception>
		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			return -1;
		}

		/// <summary>
		///     Removes the <see cref="T:System.ComponentModel.PropertyDescriptor" /> from the indexes used for searching.
		/// </summary>
		/// <param name="property">
		///     The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to remove from the indexes used for searching.
		/// </param>
		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
		}

		/// <summary>
		///     Removes any sort applied using
		///     <see
		///         cref="M:System.ComponentModel.IBindingList.ApplySort(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)" />
		///     .
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">
		///     <see cref="P:System.ComponentModel.IBindingList.SupportsSorting" /> is false.
		/// </exception>
		void IBindingList.RemoveSort()
		{
		}

		/// <summary>
		///     Gets whether you can add items to the list using <see cref="M:System.ComponentModel.IBindingList.AddNew" />.
		/// </summary>
		/// <returns>
		///     true if you can add items to the list using <see cref="M:System.ComponentModel.IBindingList.AddNew" />; otherwise,
		///     false.
		/// </returns>
		public bool AllowNew
		{
			get { return true; }
		}

		/// <summary>
		///     Gets whether you can update items in the list.
		/// </summary>
		/// <returns>
		///     true if you can update the items in the list; otherwise, false.
		/// </returns>
		public bool AllowEdit
		{
			get { return true; }
		}

		/// <summary>
		///     Gets whether you can remove items from the list, using
		///     <see cref="M:System.Collections.IList.Remove(System.Object)" /> or
		///     <see
		///         cref="M:System.Collections.IList.RemoveAt(System.Int32)" />
		///     .
		/// </summary>
		/// <returns>
		///     true if you can remove items from the list; otherwise, false.
		/// </returns>
		public bool AllowRemove
		{
			get { return true; }
		}

		/// <summary>
		///     Gets whether a <see cref="E:System.ComponentModel.IBindingList.ListChanged" /> event is raised when the list
		///     changes or an item in the list changes.
		/// </summary>
		/// <returns>
		///     true if a <see cref="E:System.ComponentModel.IBindingList.ListChanged" /> event is raised when the list changes or
		///     when an item changes; otherwise, false.
		/// </returns>
		public bool SupportsChangeNotification
		{
			get { return false; }
		}

		/// <summary>
		///     Gets whether the list supports searching using the
		///     <see
		///         cref="M:System.ComponentModel.IBindingList.Find(System.ComponentModel.PropertyDescriptor,System.Object)" />
		///     method.
		/// </summary>
		/// <returns>
		///     true if the list supports searching using the
		///     <see
		///         cref="M:System.ComponentModel.IBindingList.Find(System.ComponentModel.PropertyDescriptor,System.Object)" />
		///     method; otherwise, false.
		/// </returns>
		bool IBindingList.SupportsSearching
		{
			get { return false; }
		}

		/// <summary>
		///     Gets whether the list supports sorting.
		/// </summary>
		/// <returns>
		///     true if the list supports sorting; otherwise, false.
		/// </returns>
		bool IBindingList.SupportsSorting
		{
			get { return false; }
		}

		/// <summary>
		///     Gets whether the items in the list are sorted.
		/// </summary>
		/// <returns>
		///     true if
		///     <see
		///         cref="M:System.ComponentModel.IBindingList.ApplySort(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)" />
		///     has been called and
		///     <see
		///         cref="M:System.ComponentModel.IBindingList.RemoveSort" />
		///     has not been called; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">
		///     <see cref="P:System.ComponentModel.IBindingList.SupportsSorting" /> is false.
		/// </exception>
		bool IBindingList.IsSorted
		{
			get { return false; }
		}

		/// <summary>
		///     Gets the <see cref="T:System.ComponentModel.PropertyDescriptor" /> that is being used for sorting.
		/// </summary>
		/// <returns>
		///     The <see cref="T:System.ComponentModel.PropertyDescriptor" /> that is being used for sorting.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">
		///     <see cref="P:System.ComponentModel.IBindingList.SupportsSorting" /> is false.
		/// </exception>
		PropertyDescriptor IBindingList.SortProperty
		{
			get { return default(PropertyDescriptor); }
		}

		/// <summary>
		///     Gets the direction of the sort.
		/// </summary>
		/// <returns>
		///     One of the <see cref="T:System.ComponentModel.ListSortDirection" /> values.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">
		///     <see cref="P:System.ComponentModel.IBindingList.SupportsSorting" /> is false.
		/// </exception>
		ListSortDirection IBindingList.SortDirection
		{
			get { return default(ListSortDirection); }
		}

		/// <summary>
		///     Sorts the data source based on the given <see cref="T:System.ComponentModel.ListSortDescriptionCollection" />.
		/// </summary>
		/// <param name="sorts">
		///     The <see cref="T:System.ComponentModel.ListSortDescriptionCollection" /> containing the sorts to apply to the data
		///     source.
		/// </param>
		public void ApplySort(ListSortDescriptionCollection sorts)
		{
		}

		/// <summary>
		///     Removes the current filter applied to the data source.
		/// </summary>
		void IBindingListView.RemoveFilter()
		{
		}

		/// <summary>
		///     Gets or sets the filter to be used to exclude items from the collection of items returned by the data source
		/// </summary>
		/// <returns>
		///     The string used to filter items out in the item collection returned by the data source.
		/// </returns>
		string IBindingListView.Filter
		{
			get
			{
				//throw new NotImplementedException();
				return string.Empty;
			}
			set
			{
				//throw new NotImplementedException();
			}
		}

		/// <summary>
		///     Gets the collection of sort descriptions currently applied to the data source.
		/// </summary>
		/// <returns>
		///     The <see cref="T:System.ComponentModel.ListSortDescriptionCollection" /> currently applied to the data source.
		/// </returns>
		ListSortDescriptionCollection IBindingListView.SortDescriptions
		{
			get { return null; }
		}

		/// <summary>
		///     Gets a value indicating whether the data source supports advanced sorting.
		/// </summary>
		/// <returns>
		///     true if the data source supports advanced sorting; otherwise, false.
		/// </returns>
		public bool SupportsAdvancedSorting
		{
			get { return false; }
		}

		/// <summary>
		///     Gets a value indicating whether the data source supports filtering.
		/// </summary>
		/// <returns>
		///     true if the data source supports filtering; otherwise, false.
		/// </returns>
		public bool SupportsFiltering
		{
			get { return false; }
		}
		#endregion

		public event EventHandler<ItemActingEventArgs<object>> Editing;
		public event EventHandler<ItemActingEventArgs<IEnumerable>> Deleting;
		public event EventHandler<ItemActingEventArgs<object>> ItemBinding;
		public event EventHandler<BindableListViewItemDataBound> ItemBound;
		public event EventHandler<AddingNewEventArgs> AddingNew;
		public event EventHandler DataBound;

		protected virtual void OnAddingNew(AddingNewEventArgs e)
		{
			if (this.AddingNew != null)
				this.AddingNew(this, e);
		}

		protected virtual int CoreInsertItem(object value, int? index)
		{
			ListViewItem item = null;
			//Get item's schema
			var properties = ObjectHelper.ReflectProperties(value);

			//Check if this row is excepted.
			if (!this.OnItemBinding(new ItemActingEventArgs<Object>(value)))
				return -1;

			//iterate on columns' tag which represent table's fields
			foreach (var property in this.Columns.Cast<ColumnHeader>().Where(column => column.Tag != null).Select(column => properties[column.Tag.ToString()]))
				//is the first field on current row?
				if (item == null)
				{
					//Yes: add new item to rows
					item = index.HasValue ? this.Items.Insert(index.Value, ToItemString(property)) : this.Items.Add(ToItemString(property));
					//Save the entity in tag for future uses
					item.Tag = value;
				}
				else
					//No: add sub item to item
					item.SubItems.Add(ToItemString(property));
			if (item != null)
				this.OnItemBound(new BindableListViewItemDataBound(value, item));
			return item != null ? item.Index : -2;
		}

		public ListViewItem GetListViewItemByData(object value)
		{
			return this.ListViewItems.Where(item => item.Tag.Equals(value)).FirstOrDefault();
		}

		public void Rebind()
		{
			if (!this.AutoGenerateColumns && this.Columns.Cast<ColumnHeader>().All(col => col.Tag == null))
				throw new LibraryException("Please fill the tags or set AutoGenerateColumns to true.");
			try
			{
				if (this.DataSource == null)
					this.Items.Clear();
				else
				{
					var entityList = this.ExportEntities().Cast<Object>();

					//Game's just began.
					this.SuspendLayout();
					var selectedItems = this.SelectedItems.Cast<ListViewItem>();

					//Clear old items
					this.Items.Clear();

					//Retrieve entity list
					if (!entityList.Any())
						return;

					//Generating columns by the schema of first entity.
					var entity = entityList.First();
					if (this.AutoGenerateColumns)
						//Clear old columns
						this.GenerateColumns(entity);
					//Add entities to the list
					foreach (var datasourceItem in entityList.Where(ent => ent != null))
						this.Add(datasourceItem);
					this.SelectedIndices.Clear();
					foreach (var item in selectedItems)
						this.SelectedIndices.Add(this.Items.IndexOf(item));
				}
			}
			finally
			{
				this.ResumeLayout();
				this.OnDataBound(EventArgs.Empty);
			}
		}

		protected virtual void GenerateColumns(object entity)
		{
			this.Columns.Clear();
			foreach (var property in ObjectHelper.ReflectProperties(entity))
			{
				//Add property name as key and parsed property name the title of column
				var col = this.Columns.Add(property.Key, property.Key.SeparateCamelCase());
				col.TextAlign = HorizontalAlignment.Right;
				col.Tag = property.Key;
			}
		}

		protected virtual IEnumerable ExportEntities()
		{
			IEnumerable result;
			if (this.DataSource is BindingSource)
				result = ((this.DataSource as BindingSource).DataSource) as IEnumerable<Object>;
			else if (this.DataSource is IEnumerable)
				result = this.DataSource as IEnumerable;
			else
				throw new NotSupportedException();
			return result;
		}

		private void OnDataBound(EventArgs e)
		{
			this.DataBound.Raise(e);
		}

		private void OnItemBound(BindableListViewItemDataBound e)
		{
			this.ItemBound.Raise(this, e);
		}

		private bool OnItemBinding(ItemActingEventArgs<object> e)
		{
			this.ItemBinding.Raise(this, e);
			return !e.Handled;
		}

		public void DeleteSelectedEntities()
		{
			if (this.SelectedItems.Count > 0)
				if (this.OnDeleting(new ItemActingEventArgs<IEnumerable>(this.SelectedEntities)) && this.AutoRebind)
					this.Rebind();
		}

		private bool OnDeleting(ItemActingEventArgs<IEnumerable> e)
		{
			return !this.Deleting.Raise(this, e).Handled;
		}

		public void EditSelectedEntity()
		{
			if (this.SelectedItems.Count == 1)
				if (this.OnEditing(new ItemActingEventArgs<object>(this.SelectedItems[0].Tag)) && this.AutoRebind)
					this.Rebind();
		}

		private bool OnEditing(ItemActingEventArgs<object> e)
		{
			return !this.Editing.Raise(this, e).Handled;
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick(e);
			if (this.EditOnDoubleClick)
				this.EditSelectedEntity();
		}

		public static string ToItemString(object o)
		{
			if (o is bool)
				return (Boolean)o ? "*" : "";
			return (o ?? "").ToString();
		}
	}

	public class BindableListViewItemDataBound : ItemActedEventArgs<object>
	{
		public BindableListViewItemDataBound(object item, ListViewItem listViewItem)
			: base(item)
		{
			this.ListViewItem = listViewItem;
		}

		public ListViewItem ListViewItem { get; set; }
	}
}