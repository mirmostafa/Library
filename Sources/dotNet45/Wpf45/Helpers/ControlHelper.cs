using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Mohammad.Helpers;
using Mohammad.Win32.Natives;
using Mohammad.Win32.Natives.IfacesEnumsStructsClasses;
using Mohammad.Wpf.Windows;
using Mohammad.Wpf.Windows.Media;
using static Mohammad.Helpers.CodeHelper;

namespace Mohammad.Wpf.Helpers
{
    public static class ControlHelper
    {
        public static TParent GetParent<TParent>(this DependencyObject dp) where TParent : DependencyObject
        {
            var parent = dp;
            while ((parent = VisualTreeHelper.GetParent(parent)) != null)
                if (parent is TParent)
                    return parent as TParent;
            return null;
        }

        public static void Rebind(this DependencyObject target, DependencyProperty dependencyProperty)
        {
            BindingOperations.GetBindingExpressionBase(target, dependencyProperty)?.UpdateTarget();
        }

        /// <summary>
        ///     Determines whether [is design time].
        /// </summary>
        /// <returns>
        ///     <c>true</c> if [is design time]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDesignTime()
        {
            return Application.Current.MainWindow == null;
        }

        public static void Run(this DispatcherObject control, Action action)
        {
            //var result = control.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(action));
            //return result;
            control.Dispatcher.Invoke(action);
        }

        public static ItemsControl GetParent(this TreeViewItem item)
        {
            var parent = VisualTreeHelper.GetParent(item);

            while (!(parent is TreeViewItem || parent is TreeView))
                parent = VisualTreeHelper.GetParent(parent);
            return parent as ItemsControl;
        }

        public static void RebindDataContext(this FrameworkElement element)
        {
            var dataContext = element.DataContext;
            element.DataContext = null;
            element.DataContext = dataContext;
        }

        public static void RebindItemsSource(this ItemsControl control)
        {
            control.Rebind(ItemsControl.ItemsSourceProperty);
            var itemsSource = control.ItemsSource;
            control.ItemsSource = null;
            control.ItemsSource = itemsSource;
        }

        public static void RebindItemsSource(this TreeView control)
        {
            control.Rebind(ItemsControl.ItemsSourceProperty);
            var selectedItem = control.SelectedItem;
            var itemsSource = control.ItemsSource;
            control.ItemsSource = null;
            control.ItemsSource = itemsSource;
            control.SetSelectedItem(selectedItem);
        }

        public static IEnumerable<TControl> GetControls<TControl>(this Visual control, bool loopThrouth = true) where TControl : class
            => GetControls(control, loopThrouth).OfType<TControl>();

        public static IEnumerable<Visual> GetControls(this Visual control, bool loopThrouth = true)
        {
            var result = new List<Visual>();
            EnumerableHelper.ForEachTreeNode(control,
                c =>
                {
                    if (!loopThrouth && !Equals(c, control))
                        return Enumerable.Empty<Visual>();
                    if (!(c is TabItem))
                        return Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(c)).Select(i => (Visual) VisualTreeHelper.GetChild(c, i));
                    if (c.As<TabItem>().Content is Visual)
                        return new[] {c.As<TabItem>().Content.As<Visual>(), c.As<TabItem>().Header.As<Visual>()};
                    return Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(c)).Select(i => (Visual) VisualTreeHelper.GetChild(c, i));
                },
                result.Add,
                null);
            return result.RemoveNulls();
        }

        public static string GetText(this RichTextBox rtb)
        {
            var textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);

            return textRange.Text;
        }

        public static void Bind<TEnum>(this ComboBox comboBox, TEnum selectedItem = default(TEnum)) where TEnum : struct
        {
            //var members = EnumHelper.GetMembers<TEnum>().Where(item => EnumHelper.GetItemAttribute<TEnum, NonSerializedAttribute>(item.ValueMember) == null).ToCollection();
            //comboBox.ItemsSource = members;
            //comboBox.DisplayMemberPath = "DisplayMember";
            //comboBox.SelectedValuePath = "Value";
            //comboBox.SelectedValue = selectedItem;
            comboBox.ItemsSource = Enum.GetValues(typeof(TEnum));
            comboBox.SelectedItem = selectedItem;
        }

        public static void BindToElementPath(this UIElement source, FrameworkElement target, DependencyProperty targetDependencyProperty, string path,
            BindingMode bindingMode = BindingMode.TwoWay)
        {
            target?.SetBinding(targetDependencyProperty, new Binding {Source = source, Path = new PropertyPath(path), Mode = bindingMode});
        }

        public static void EnsureVisible(this ListViewItem item) { item.Parent.As<ListView>()?.ScrollIntoView(item); }
        public static void EnsureVisibleItem(this ListView lv, object item) { lv?.ScrollIntoView(item); }

        public static void EnsureVisibleItem(this DataGrid dg, object item) { dg?.ScrollIntoView(item); }

        public static void Expand(this TreeViewItem item, bool isExpanded = true)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var parent = item;
            do
            {
                parent.IsExpanded = isExpanded;
            } while ((parent = parent.Parent.As<TreeViewItem>()) != null);
            item.BringIntoView();
        }

        public static IEnumerable<TreeViewItem> GetAllItems(this TreeView tree)
            => EnumerableHelper.GetAll(() => tree.Items.Cast<TreeViewItem>(), item => item.Items.Cast<TreeViewItem>());

        public static bool? ShowDialog<TWindow>(this Window owner, out TWindow window) where TWindow : Window, new()
            => ShowDialog(owner, () => new TWindow(), out window);

        public static bool? ShowDialog<TWindow>(this Window owner, Func<TWindow> creator, out TWindow window) where TWindow : Window
        {
            if (creator == null)
                throw new ArgumentNullException(nameof(creator));
            window = creator();
            window.Owner = owner;
            return window.ShowDialog();
        }

        public static bool? ShowDialog<TWindow>(this Window owner) where TWindow : Window, new()
        {
            TWindow window;
            return ShowDialog(owner, () => new TWindow(), out window);
        }

        public static void RemoveIcon(this Window window)
        {
            var hwnd = GetHandle(window);
            Catch(
                () =>
                    Api.SetWindowLong(hwnd, WindowsMessages.GWL_EXSTYLE, Api.GetWindowLong(hwnd, WindowsMessages.GWL_EXSTYLE) | WindowsMessages.WS_EX_DLGMODALFRAME));
            Catch(
                () =>
                    Api.SetWindowPos(hwnd,
                        IntPtr.Zero,
                        0,
                        0,
                        0,
                        0,
                        WindowsMessages.SWP_NOMOVE | WindowsMessages.SWP_NOSIZE | WindowsMessages.SWP_NOZORDER | WindowsMessages.SWP_FRAMECHANGED));
        }

        public static IntPtr GetHandle(this Window window) { return new WindowInteropHelper(window).Handle; }
        public static void Flick(this IFlickable element, int duration = 500) { Catch(() => Animations.Flick(element.FlickerTextBlock, duration)); }
        public static void Flick(FrameworkElement element, int duration = 500) { Catch(() => Animations.Flick(element, duration)); }
        public static void Rebind(this TextBox target) { BindingOperations.GetBindingExpressionBase(target, TextBox.TextProperty)?.UpdateTarget(); }

        public static void MoveToNextUIElement(RoutedEventArgs e)
        {
            var request = new TraversalRequest(FocusNavigationDirection.Next);
            var elementWithFocus = Keyboard.FocusedElement as UIElement;

            if (elementWithFocus == null)
                return;
            if (elementWithFocus.MoveFocus(request))
                if (e != null)
                    e.Handled = true;
        }

        public static DependencyProperty GetDependencyProperty<TType, TOwnerType>(string propertyName, Action<TOwnerType, TType> onDependencyPropertyChanged = null,
            Action<TOwnerType, string> onPropertyChanged = null, Func<TType, bool> validateValue = null, CoerceValueCallback coerceValueCallback = null,
            object defaultValue = null) where TOwnerType : class
        {
            PropertyChangedCallback callback = (sender, e) =>
            {
                var me = sender as TOwnerType;
                if (me == null)
                    return;
                onDependencyPropertyChanged?.Invoke(me, (TType) e.NewValue);
                onPropertyChanged?.Invoke(me, e.Property.Name);
            };

            return coerceValueCallback != null
                ? DependencyProperty.Register(propertyName,
                    typeof(TType),
                    typeof(TOwnerType),
                    new FrameworkPropertyMetadata(defaultValue ?? default(TType),
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
                        callback,
                        coerceValueCallback))
                : DependencyProperty.Register(propertyName,
                    typeof(TType),
                    typeof(TOwnerType),
                    new FrameworkPropertyMetadata(defaultValue ?? default(TType),
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
                        callback) {BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged},
                    value => validateValue == null || validateValue((TType) value));
        }

        public static T GetVisualChild<T>(this Visual parent) where T : Visual
        {
            var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (var index = 0; index < numVisuals; index++)
            {
                var v = (Visual) VisualTreeHelper.GetChild(parent, index);
                var child = v as T ?? GetVisualChild<T>(v);
                if (child != null)
                    return child;
            }
            return null;
        }

        public static DataGridCell GetCell(this DataGrid grid, int row, int column)
        {
            var rowContainer = GetRow(grid, row);
            if (rowContainer == null)
                return null;
            var presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
            var cell = (DataGridCell) presenter.ItemContainerGenerator.ContainerFromIndex(column);
            if (cell != null)
                return cell;
            grid.ScrollIntoView(rowContainer, grid.Columns[column]);
            cell = (DataGridCell) presenter.ItemContainerGenerator.ContainerFromIndex(column);
            return cell;
        }

        public static DataGridRow GetRow(this DataGrid grid, int index)
        {
            var row = (DataGridRow) grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row != null)
                return row;
            grid.UpdateLayout();
            grid.ScrollIntoView(grid.Items[index]);
            row = (DataGridRow) grid.ItemContainerGenerator.ContainerFromIndex(index);
            return row;
        }

        public static void Refresh(this UIElement uiElement) { uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate); }

        public static void SetValue(this RangeBase pb, double step) { Animations.AnimateDouble(pb, RangeBase.ValueProperty, pb.Value, step, 100); }
        //public static void SetValue(this RangeBase pb, double step) { pb.Value = step; }

        public static IEnumerable<dynamic> RetieveCheckedItems(this MultiSelector dg)
            => dg.Items.Cast<dynamic>().Where(item => item.IsChecked == true).Cast<object>().Select(item => item.As<dynamic>());

        public static IEnumerable<TItem> RetieveCheckedItems<TItem>(this MultiSelector dg)
            => dg.Items.Cast<dynamic>().Where(item => item.IsChecked == true).Cast<object>().Select(item => item.To<TItem>());

        public static void HandleKeyDown(this MultiSelector multiSelector, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.A)
            {
                multiSelector.SelectAll();
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Home)
            {
                multiSelector.SelectedIndex = 0;
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.End)
            {
                multiSelector.SelectedIndex = multiSelector.Items.Count;
            }
            else if (e.Key == Key.Space)
            {
                var items = multiSelector.SelectedItems.Cast<dynamic>().ToArray();
                items.ForEach(instance => instance.IsChecked = items.Any(o => !o.IsChecked));
            }
        }
    }
}