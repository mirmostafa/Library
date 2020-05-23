using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Mohammad.Helpers;
using Mohammad.Helpers.Internals;
using Mohammad.Win.Settings;

namespace Mohammad.Win.Helpers
{
    /// <summary>
    ///     A utility class to do some common tasks about controls
    /// </summary>
    public static partial class ControlHelper
    {
        private static readonly Collection<TabPageInfo> _TabPages = new Collection<TabPageInfo>();
        public static bool IsDesignMode { get { return LicenseManager.UsageMode == LicenseUsageMode.Designtime; } }

        /// <summary>
        ///     Binds a combo box to an enumeration.
        /// </summary>
        /// <param name="comboBox">Combo box to be bound</param>
        /// <typeparam name="TEnum">Enumeration as DataSource</typeparam>
        public static void Bind<TEnum>(this ComboBox comboBox) where TEnum : struct
        {
            var members =
                EnumHelper.GetMembers<TEnum>(false)
                    .Where(item => EnumHelper.GetItemAttribute<TEnum, NonSerializedAttribute>(item.ValueMember) == null)
                    .ToCollection();
            comboBox.DisplayMember = "DisplayMember";
            comboBox.ValueMember = "ValueMember";
            comboBox.DataSource = members;
        }

        /// <summary>
        ///     Binds a combo box to an enumeration.
        /// </summary>
        /// <param name="comboBox">Combo box to be bound</param>
        /// <param name="defaultValue">Default value after binding</param>
        /// <typeparam name="TEnum">Enumeration as DataSource</typeparam>
        public static void Bind<TEnum>(this ComboBox comboBox, TEnum defaultValue) where TEnum : struct
        {
            Bind<TEnum>(comboBox);
            foreach (var item in comboBox.Items.Cast<MemberInfo<TEnum>>().Where(item => item.ValueMember.Equals(defaultValue)))
            {
                comboBox.SelectedValue = item.ValueMember;
                break;
            }
        }

        public static void Bind(this ComboBox comboBox, object dataSource, string displayMember, string valueMember, object selectedItem = null)
        {
            comboBox.DataSource = dataSource;
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
            if (selectedItem != null && comboBox.Items.Contains(comboBox.SelectedItem = selectedItem))
                comboBox.SelectedItem = selectedItem;
        }

        public static void Bind<TSource, TKey>(this ComboBox comboBox, IQueryable<TSource> dataSource, string displayMember, string valueMember,
            Func<TSource, TKey> orderBy, object selectedItem = null)
        {
            comboBox.Bind(dataSource.OrderBy(orderBy).ToList(), displayMember, valueMember, selectedItem);
        }

        /// <summary>
        ///     Examines the full content of listView control's SetProperty collection and returns the enumerator instead of a
        ///     generic list.
        /// </summary>
        /// <param name="control">The control to examine.</param>
        /// <typeparam name="T">The Type we are looking for - we will also treat any derived types as a match!</typeparam>
        /// <returns>The actual enumerator allowing us to NOT have to create a helper intermediate list.</returns>
        public static IEnumerable<T> GetControls<T>(this Control control) where T : class
        {
            foreach (Control childControl in control.Controls)
            {
                T buffer;
                if ((buffer = childControl as T) != null)
                    yield return buffer;

                if (childControl.Controls.Count > 0)
                    foreach (var t in GetControls<T>(childControl))
                        yield return t;
            }
        }

        /// <summary>
        ///     Loops on the control's "Controls" collection and returns the enumerator of controls in given control type, instead
        ///     of a generic list.
        /// </summary>
        /// <param name="control">The control to examine.</param>
        /// <returns>The actual enumerator allowing us to NOT have to create a helper intermediate list.</returns>
        public static IEnumerable<Control> GetControls(this Control control)
        {
            foreach (Control childControl in control.Controls)
            {
                yield return childControl;

                if (childControl.Controls.Count > 0)
                    foreach (var t in GetControls(childControl))
                        yield return t;
            }
        }

        public static IEnumerable<TreeNode> GetAllNodes(this TreeView treeView) { return treeView.Nodes.GetAllNodes(); }

        private static IEnumerable<TreeNode> GetAllNodes(this TreeNodeCollection treeNodeCollection)
        {
            foreach (TreeNode treeNode in treeNodeCollection)
            {
                yield return treeNode;
                if (treeNode.Nodes.Count > 0)
                    foreach (var node in GetAllNodes(treeNode.Nodes))
                        yield return node;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <typeparam name="TDefaultValueType"></typeparam>
        /// <returns></returns>
        public static TDefaultValueType GetDefualtValue<TDefaultValueType>(object target, string propertyName)
        {
            var attr = target.GetType().GetProperty(propertyName).GetCustomAttributes(typeof(DefaultValueAttribute), true);
            if (attr.GetLength(0) < 1 || attr[0] == null)
                return default(TDefaultValueType);
            return (TDefaultValueType) ((DefaultValueAttribute) attr[0]).Value;
        }

        /// <summary>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="properyName"></param>
        /// <param name="properyValue"></param>
        public static void SetProperty(this Control target, string properyName, object properyValue)
        {
            foreach (Control control in target.Controls)
                PropertyHelper.SetValue(control, properyName, properyValue);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public static TComponent Clone<TComponent>(this TComponent source) where TComponent : Component, new()
        {
            var result = new TComponent();
            typeof(TComponent).GetProperties().ForEach(property => PropertyHelper.SetValue(result, property.Name, property.GetValue(source, null)));
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="cololumnIndex"></param>
        /// <param name="ascending"></param>
        public static void SortAlphabetic(this ListView listView, int cololumnIndex, bool ascending)
        {
            listView.SuspendLayout();

            for (short i = 0; i <= listView.Items.Count - 1; i++)
            for (var j = (short) (i + 1); j <= listView.Items.Count - 1; j++)
            {
                string tempVar;
                var iText = string.IsNullOrEmpty(listView.Items[i].SubItems[cololumnIndex].Text)
                    ? listView.Items[i].SubItems[cololumnIndex].Text
                    : listView.Items[i].SubItems[cololumnIndex].Text.Substring(1, 1);
                var jText = string.IsNullOrEmpty(listView.Items[j].SubItems[cololumnIndex].Text)
                    ? listView.Items[j].SubItems[cololumnIndex].Text
                    : listView.Items[j].SubItems[cololumnIndex].Text.Substring(1, 1);
                if (ascending)
                {
                    if (string.Compare(iText, jText, false) > 0)
                        for (short k = 0; k <= listView.Columns.Count - 1; k++)
                        {
                            tempVar = listView.Items[i].SubItems[k].Text;
                            listView.Items[i].SubItems[k].Text = listView.Items[j].SubItems[k].Text;
                            listView.Items[j].SubItems[k].Text = tempVar;
                        }
                }
                else if (string.Compare(iText, jText, false) > 0)
                {
                    for (short k = 0; k <= listView.Columns.Count - 1; k++)
                    {
                        tempVar = listView.Items[i].SubItems[k].Text;
                        listView.Items[i].SubItems[k].Text = listView.Items[j].SubItems[k].Text;
                        listView.Items[j].SubItems[k].Text = tempVar;
                    }
                }
            }
            listView.ResumeLayout();
        }

        public static void ClearControls<TControl>(this Control control) where TControl : Control
        {
            GetControls<TControl>(control).Cast<Control>().ForEach(child => child.Text = "");
        }

        public static void DoubleBufferedAll(this Control control, bool doubleBuffred)
        {
            control.GetControls<ListView>().ForEach(listView => listView.DoubleBuffered(true));
        }

        public static void DoubleBuffered(this ListView listView, bool doubleBuffered) { ObjectHelper.SetProperty(listView, "DoubleBuffered", true); }
        public static void DoubleBuffered(this TreeView treeView, bool doubleBuffered) { ObjectHelper.SetProperty(treeView, "DoubleBuffered", true); }

        public static DialogResult ShowDialog<TForm>(FormSettings formSettings, out TForm form) where TForm : Form, new()
        {
            form = new TForm();
            if (formSettings == null)
                formSettings = new FormSettings();
            formSettings.SetForm(form);
            var result = form.ShowDialog();
            return result;
        }

        public static DialogResult ShowDialog<TForm>(params object[] args) where TForm : Form, new()
        {
            DialogResult result;
            var types = new Type[args.Length];
            for (var counter = 0; counter < args.Length; counter++)
                types[counter] = args[counter].GetType();
            using (var form = ObjectHelper.CreateInstance<TForm>(types, args))
                result = form.ShowDialog();
            return result;
        }

        public static void Show<TForm>(params object[] args) where TForm : Form
        {
            var types = new Type[args.Length];
            for (var counter = 0; counter < args.Length; counter++)
                types[counter] = args[counter].GetType();
            ObjectHelper.CreateInstance<TForm>(types, args).Show();
        }

        public static void ShowSingleton<TForm>(Control parent, Func<TForm> creator) where TForm : Form
        {
            var form = (TForm) Application.OpenForms.Cast<Form>().Where(EqualsTo<TForm>).SingleOrDefault();
            if (form == null)
            {
                form = creator();
                form.TopLevel = false;
                form.Parent = parent;
                form.Show();
            }
            form.BringToFront();
            if (form.WindowState == FormWindowState.Minimized)
                form.WindowState = FormWindowState.Normal;
        }

        public static void ShowSingleton<TForm>(FormSettings formSettings) where TForm : Form, new()
        {
            var form = (TForm) Application.OpenForms.Cast<Form>().Where(EqualsTo<TForm>).SingleOrDefault();
            if (form == null)
            {
                form = new TForm();
                if (formSettings == null)
                    formSettings = new FormSettings();
                formSettings.SetForm(form);
            }
            form.BringToFront();
            if (form.WindowState == FormWindowState.Minimized)
                form.WindowState = FormWindowState.Normal;
        }

        private static bool EqualsTo<TForm>(Form openForm) where TForm : Form { return openForm is TForm; }

        public static void ShowSingleton<TForm>(Predicate<TForm> perdicate) where TForm : Form, new()
        {
            var form = Application.OpenForms.Cast<Form>().Where(EqualsTo<TForm>).Cast<TForm>().Where(openForm => perdicate(openForm)).SingleOrDefault() ??
                       new TForm();
            form.Show();
            form.BringToFront();
            if (form.WindowState == FormWindowState.Minimized)
                form.WindowState = FormWindowState.Normal;
        }

        public static void ShowSingleton<TForm>(Predicate<TForm> perdicate, params object[] args) where TForm : Form
        {
            var form = Application.OpenForms.Cast<Form>().Where(openForm => EqualsTo<TForm>(openForm) && perdicate((TForm) openForm)).SingleOrDefault() as TForm;
            if (form == null)
            {
                var types = new Type[args.Length];
                for (var counter = 0; counter < args.Length; counter++)
                    types[counter] = args[counter].GetType();
                form = ObjectHelper.CreateInstance<TForm>(types, args);
            }
            form.Show();
            form.BringToFront();
            if (form.WindowState == FormWindowState.Minimized)
                form.WindowState = FormWindowState.Normal;
        }

        public static void ShowSingleton<TForm>(Predicate<TForm> perdicate, FormSettings formSettings, params object[] args) where TForm : Form
        {
            var form = Application.OpenForms.Cast<Form>().Where(openForm => EqualsTo<TForm>(openForm) && perdicate((TForm) openForm)).SingleOrDefault() as TForm;
            if (form == null)
            {
                var types = new Type[args.Length];
                for (var counter = 0; counter < args.Length; counter++)
                    types[counter] = args[counter].GetType();
                form = ObjectHelper.CreateInstance<TForm>(types, args);
                if (formSettings == null)
                    formSettings = new FormSettings();
                formSettings.SetForm(form);
                form.Show();
            }
            form.BringToFront();
            if (form.WindowState == FormWindowState.Minimized)
                form.WindowState = FormWindowState.Normal;
        }

        public static void ShowSingleton<TForm>(Func<TForm> creator, Predicate<TForm> perdicate, FormSettings<TForm> formSettings) where TForm : Form
        {
            var form = Application.OpenForms.Cast<Form>().Where(openForm => EqualsTo<TForm>(openForm) && perdicate((TForm) openForm)).SingleOrDefault() as TForm;
            if (form == null)
            {
                form = creator();
                if (formSettings != null)
                    formSettings.SetForm(form);
                form.Show();
            }
            form.BringToFront();
            if (form.WindowState == FormWindowState.Minimized)
                form.WindowState = FormWindowState.Normal;
        }

        public static void Regroup(this ListView listView, bool recreateGroups, int columnIndex)
        {
            listView.SuspendLayout();
            if (recreateGroups)
                listView.Groups.Clear();
            foreach (ListViewItem item in listView.Items)
            {
                if (!listView.Groups.ContainsName(item.SubItems[columnIndex].Text))
                    listView.Groups.Add(item.SubItems[columnIndex].Text, item.SubItems[columnIndex].Text);
                item.Group = listView.Groups[item.SubItems[columnIndex].Text];
            }
            listView.ResumeLayout();
        }

        public static bool ContainsName(this ListViewGroupCollection groups, string name)
        {
            return groups.Cast<ListViewGroup>().Any(group => group.Name.EqualsTo(name));
        }

        public static void PrepareBackColors(this ListView listView, Color rowBackColor)
        {
            if (listView.ShowGroups)
            {
                var i = 0;
                foreach (ListViewGroup group in listView.Groups)
                foreach (ListViewItem lvi in group.Items)
                {
                    lvi.BackColor = i % 2 == 0 ? listView.BackColor : rowBackColor;
                    i++;
                }
            }
            else
            {
                foreach (ListViewItem lvi in listView.Items)
                    lvi.BackColor = lvi.Index % 2 == 0 ? listView.BackColor : rowBackColor;
            }
        }

        public static IEnumerable<KeyValuePair<ListView, ListViewColumnSorter>> InitializeListViewSorters(this Control control)
        {
            var result = new Dictionary<ListView, ListViewColumnSorter>();
            control.GetControls<ListView>().ForEach(listView => result.Add(listView, new ListViewColumnSorter(listView)));
            return result;
        }

        public static void RemoveSelectedItems(this ListBox listBox)
        {
            while (listBox.SelectedIndex != -1)
                listBox.Items.RemoveAt(listBox.SelectedIndex);
        }

        public static bool RunInCurrentThread(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
                return false;
            }
            action();
            return true;
        }

        //public static bool RunInCurrentThread<T>(this Control control, Action<T> action, T t)
        //{
        //    if (control.InvokeRequired)
        //    {
        //        control.Invoke(action, t);
        //        return false;
        //    }
        //    action(t);
        //    return true;
        //}

        /// <summary>
        /// </summary>
        /// <param name="control"></param>
        /// <typeparam name="TControl"></typeparam>
        internal static void FormatGrids<TControl>(this TControl control) where TControl : Control
        {
            control.GetControls<DataGridView>()
                .ForEach(
                    dataGridView =>
                        dataGridView.Columns.ForEach(
                            column => ((DataGridViewColumn) column).HeaderText = ((DataGridViewColumn) column).HeaderText.SeparateCamelCase()));
        }

        /// <summary>
        /// </summary>
        /// <param name="dataGridViews"></param>
        public static void FormatGrid(params DataGridView[] dataGridViews)
        {
            dataGridViews.ForEach(
                dataGridView =>
                    dataGridView.Columns.ForEach(column => ((DataGridViewColumn) column).HeaderText = ((DataGridViewColumn) column).HeaderText.SeparateCamelCase()));
        }

        private static TabPageInfo CreateInfo(TabPage tabPage)
        {
            var parent = (TabControl) tabPage.Parent;
            return new TabPageInfo(parent, tabPage, parent.TabPages.IndexOf(tabPage));
        }

        public static void SetVisible(this TabPage tabPage, bool visible)
        {
            if (visible)
            {
                TabPageInfo info;
                if ((info = Find(tabPage)) == null)
                    return;
                info.Parent.TabPages.Insert(Find(tabPage).Index, tabPage);
                _TabPages.Remove(info);
            }
            else
            {
                if (Find(tabPage) != null)
                    return;
                var parent = (TabControl) tabPage.Parent;
                _TabPages.Add(CreateInfo(tabPage));
                parent.TabPages.Remove(tabPage);
            }
        }

        public static bool GetVisible(this TabPage tabPage) { return Find(tabPage) == null; }

        private static TabPageInfo Find(TabPage tabPage)
        {
            return (from tab in _TabPages
                    where tab.TabPage == tabPage
                    select tab).Take(1).SingleOrDefault();
        }

        public static void CallOnDataChanged(this Form form, EventHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");
            form.GetControls<TextBox>().ForEach(control => control.TextChanged += handler);
            form.GetControls<CheckBox>().ForEach(control => control.CheckedChanged += handler);
            form.GetControls<ComboBox>().ForEach(control => control.SelectedIndexChanged += handler);
            form.GetControls<CheckedListBox>().ForEach(control => control.ItemCheck += (sender, e) => handler(sender, EventArgs.Empty));
        }

        public static IEnumerable<TEntity> GetTagEntities<TEntity>(this ListView listView)
        {
            return listView.Items.Cast<ListViewItem>().Select(item => (TEntity) item.Tag);
        }

        public static IEnumerable<ListViewItem> GetItems(this ListView listView) { return listView.Items.Cast<ListViewItem>(); }

        public static void RemoveRange(this ListView.ListViewItemCollection listViewItems, IEnumerable<ListViewItem> items, Action<ListViewItem> removed = null)
        {
            if (EnumerableHelper.Count(items) == 0)
                return;
            var listView = ((ListViewItem) EnumerableHelper.ElementAt(items, 0)).ListView;
            listView.BeginUpdate();
            if (removed == null)
                items.ForEach(item => listView.Items.Remove(item));
            else
                items.ForEach(item =>
                {
                    listView.Items.Remove(item);
                    removed(item);
                });
            listView.EndUpdate();
            listView.Update();
        }

        public static IEnumerable<TEntity> GetEntities<TEntity>(this ListView listView)
        {
            return listView.Items.Cast<ListViewItem>().Select(item => (TEntity) item.Tag);
        }

        public static IEnumerable<TEntity> GetSelectedEntities<TEntity>(this ListView listView)
        {
            return listView.SelectedItems.Cast<ListViewItem>().Select(item => (TEntity) item.Tag);
        }

        public static IEnumerable<ListViewItem> GetSelectedItems(this ListView listView) { return listView.SelectedItems.Cast<ListViewItem>(); }
        public static IEnumerable<TabPage> GetTabPages(this TabControl tabControl) { return tabControl.TabPages.Cast<TabPage>(); }

        public static void Clear(params Control[] controls)
        {
            foreach (var control in controls)
                if (control is ComboBox)
                {
                    (control as ComboBox).DataSource = null;
                    (control as ComboBox).Items.Clear();
                }
                else if (control is TextBox)
                {
                    (control as TextBox).Text = string.Empty;
                }
        }

        #region Nested type: TabPageInfo

        private sealed class TabPageInfo
        {
            internal TabControl Parent { get; }
            internal TabPage TabPage { get; }
            internal int Index { get; }

            internal TabPageInfo(TabControl parent, TabPage tabPage, int index)
            {
                this.Parent = parent;
                this.TabPage = tabPage;
                this.Index = index;
            }
        }

        #endregion
    }
}