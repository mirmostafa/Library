using Library.Coding;
using Library.Helpers;
using Library.Validations;
using Library.Wpf.Media;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using static Library.Coding.CodeHelper;

namespace Library.Wpf.Helpers;

public static class ControlHelper
{
    public static THeaderedItemsControl BindDataContext<THeaderedItemsControl>(this THeaderedItemsControl itemsControl, object datacontext, string? header = null)
        where THeaderedItemsControl : HeaderedItemsControl
    {
        Check.IfArgumentNotNull(itemsControl, nameof(itemsControl));

        itemsControl.DataContext = datacontext;
        itemsControl.Header = header ?? datacontext?.ToString();
        return itemsControl;
    }

    public static TItemsControl BindItems<TItemsControl>(this TItemsControl itemsControl, IEnumerable items)
        where TItemsControl : ItemsControl
    {
        Check.IfArgumentNotNull(itemsControl, nameof(itemsControl));

        itemsControl.ItemsSource = null;
        itemsControl.Items.Clear();
        if (items is not null)
        {
            foreach (var item in items)
            {
                _ = itemsControl.Items.Add(new TreeViewItem { DataContext = item, Header = item?.ToString() });
            }
        }
        return itemsControl;
    }

    public static void BindItemsSource<TSelector>(this TSelector selector, IEnumerable items, string? displayMemebrPath = null, int? selectedIndex = null)
        where TSelector : Selector
    {
        BindItemsSourceInner(selector, items, displayMemebrPath);

        if (selectedIndex is not null)
        {
            selector.SelectedIndex = selectedIndex.Value;
        }
    }

    public static TSelector BindItemsSource<TSelector>(this TSelector selector, IEnumerable items, string? displayMemebrPath, object? selectedItem)
        where TSelector : Selector
    {
        BindItemsSourceInner(selector, items, displayMemebrPath);

        if (selectedItem is not null)
        {
            selector.SelectedValue = selectedItem;
        }
        else if (items.As<IEnumerator>()?.Current is { } current)
        {
            selector.SelectedValue = current;
        }
        return selector;
    }

    public static Selector BindItemsSourceToEnum<TEnum>(this Selector selector, TEnum? selectedItem = null) where TEnum : struct
        => BindItemsSource(selector, Enum.GetValues(typeof(TEnum)), null, selectedItem);

    public static TreeViewItem BindNewItem(object datacontext, string? header = null)
        => new TreeViewItem().BindDataContext(datacontext, header);

    public static TreeViewItem BindNewItems(this TreeViewItem parentItem, IEnumerable items)
    {
        Check.IfArgumentNotNull(parentItem, nameof(parentItem));
        if (items is not null)
        {
            foreach (var item in items)
            {
                _ = parentItem.Items.Add(BindNewItem(item));
            }
        }

        return parentItem;
    }

    public static void BindToElementPath(this UIElement source,
        FrameworkElement target,
        DependencyProperty targetDependencyProperty,
        string path,
        BindingMode bindingMode = BindingMode.TwoWay)
        => target?.SetBinding(targetDependencyProperty, new Binding { Source = source, Path = new PropertyPath(path), Mode = bindingMode });

    public static void EnsureVisible(this ListViewItem item)
        => item.ArgumentNotNull(nameof(item)).Parent.As<ListView>()?.ScrollIntoView(item);

    public static void EnsureVisibleItem(this DataGrid dg, object item)
        => dg.ArgumentNotNull(nameof(dg)).ScrollIntoView(item);

    public static void EnsureVisibleItem(this ListBox lv, object item)
        => lv.ArgumentNotNull(nameof(lv)).ScrollIntoView(item);

    public static void ExpandAll(this TreeViewItem item, bool isExpanded = true)
    {
        Check.IfArgumentNotNull(item, nameof(item));

        var parent = item;
        do
        {
            parent.IsExpanded = isExpanded;
        } while ((parent = parent?.Parent.As<TreeViewItem>()) is not null);

        item.BringIntoView();
    }

    public static void Flick(FrameworkElement element, int duration = 500)
        => Catch(() => Animations.Flick(element, duration));

    public static IEnumerable<TreeViewItem> GetAllItems([DisallowNull] this TreeView tree)
        => EnumerableHelper.GetAll(() => tree.ArgumentNotNull(nameof(tree)).Items.Cast<TreeViewItem>(), item => item.Items.Cast<TreeViewItem>());

    public static DataGridCell? GetCell(this DataGrid grid, int row, int column)
    {
        Check.IfArgumentNotNull(grid, nameof(grid));
        var rowContainer = grid.GetRow(row);
        if (rowContainer is null)
        {
            return null;
        }

        var presenter = rowContainer.GetVisualChild<DataGridCellsPresenter>();
        if (presenter is null)
        {
            return null;
        }
        var cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
        if (cell is not null)
        {
            return cell;
        }

        grid.ScrollIntoView(rowContainer, grid.Columns[column]);
        cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
        return cell;
    }

    public static IEnumerable<TControl> GetChildren<TControl>(this Visual visual, bool loopThrouth = true)
        where TControl : class
        => visual.ArgumentNotNull(nameof(visual)).GetChildren(loopThrouth).OfType<TControl>();

    public static IEnumerable<Visual> GetChildren(this Visual visual, bool loopThrouth = true)
    {
        var result = new List<Visual>();
        EnumerableHelper.ForEachTreeNode(visual,
            c =>
            {
                if (!loopThrouth && !Equals(c, visual))
                {
                    return Enumerable.Empty<Visual>();
                }
                else if (c is not TabItem tabItem)
                {
                    return Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(c)).Select(i => (Visual)VisualTreeHelper.GetChild(c, i));
                }
                else if (tabItem.Content is Visual)
                {
                    return new[] { tabItem.Content.As<Visual>()!, tabItem.Header.As<Visual>()! };
                }
                else
                {
                    return Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(c)).Select(i => (Visual)VisualTreeHelper.GetChild(c, i));
                }
            },
            result.Add,
            null);
        return result.RemoveNulls();
    }

    public static DependencyProperty GetDependencyProperty<TType, TOwnerType>(string propertyName,
        Action<TOwnerType, DependencyPropertyChangedEventArgs>? onDependencyPropertyChanged = null,
        Action<TOwnerType, string>? onPropertyChanged = null,
        Func<TType, bool>? validateValue = null,
        CoerceValueCallback? coerceValueCallback = null,
        TType? defaultValue = default) where TOwnerType : class
    {

        return coerceValueCallback is null
            ? DependencyProperty.Register(
                propertyName,
                typeof(TType),
                typeof(TOwnerType),
                new FrameworkPropertyMetadata(
                    defaultValue ?? default,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
                    callback)
                { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged },
                value => validateValue is null || validateValue((TType)value))
            : DependencyProperty.Register(
                propertyName,
                typeof(TType),
                typeof(TOwnerType),
                new FrameworkPropertyMetadata(
                    defaultValue ?? default,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
                    callback,
                    coerceValueCallback));
        void callback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not TOwnerType owner)
            {
                return;
            }

            onDependencyPropertyChanged?.Invoke(owner, e);
            onPropertyChanged?.Invoke(owner, e.Property.Name);
        }
    }

    public static ItemsControl? GetGrandParent(this TreeViewItem item)
    {
        var parent = VisualTreeHelper.GetParent(item);

        while (parent is not (TreeViewItem or TreeView))
        {
            parent = VisualTreeHelper.GetParent(parent);
        }

        return parent as ItemsControl;
    }

    public static IntPtr GetHandle(this Window window)
        => new WindowInteropHelper(window).Handle;

    public static TParent? GetParentByType<TParent>(this DependencyObject depObj)
        where TParent : DependencyObject
    {
        var parent = depObj;
        while ((parent = VisualTreeHelper.GetParent(parent)) is not null)
        {
            if (parent is TParent)
            {
                return parent as TParent;
            }
        }

        return null;
    }

    public static Window GetParentWindow(this DependencyObject dependencyObject)
        => Window.GetWindow(dependencyObject);

    public static DataGridRow GetRow(this DataGrid grid, int index)
    {
        Check.IfArgumentNotNull(grid, nameof(grid));

        var row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
        if (row is not null)
        {
            return row;
        }

        grid.UpdateLayout();
        grid.ScrollIntoView(grid.Items[index]);
        row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
        return row;
    }

    public static T? GetSelectedValue<T>(this TreeView treeView)
        where T : class
        => treeView.ArgumentNotNull(nameof(treeView)).SelectedItem.As<TreeViewItem>()?.DataContext.As<T>();

    [Obsolete("Use listView.GetSelection, instead.")]
    public static T? GetSelection<T>(this SelectionChangedEventArgs e)
        => e.ArgumentNotNull(nameof(e)).AddedItems.Cast<object?>().FirstOrDefault().ToNullable<T>();

    public static IEnumerable<T?> GetSelections<T>(this ListView listView) =>
        listView.ArgumentNotNull(nameof(listView)).SelectedItems.Cast<object?>().Select(x => x.ToNullable<T>());
    public static T? GetSelection<T>(this ListView listView, SelectionChangedEventArgs e) =>
        e?.AddedItems.Any() is true ? e.AddedItems[0].ToNullable<T>() : GetSelections<T>(listView).FirstOrDefault();

    public static string GetText(this RichTextBox rtb)
    {
        Check.IfArgumentNotNull(rtb);

        var textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
        return textRange.Text;
    }

    public static T? GetVisualChild<T>(this Visual parent) where T : Visual
    {
        var numVisuals = VisualTreeHelper.GetChildrenCount(parent);
        for (var index = 0; index < numVisuals; index++)
        {
            var v = (Visual)VisualTreeHelper.GetChild(parent, index);
            var child = v as T ?? GetVisualChild<T>(v);
            if (child is not null)
            {
                return child;
            }
        }

        return null;
    }

    public static void HandleKeyDown(this MultiSelector multiSelector, KeyEventArgs e)
    {
        Check.IfArgumentNotNull(multiSelector, nameof(multiSelector));

        Check.IfArgumentNotNull(e, nameof(e));

        switch (e.Key)
        {
            case Key.Home when Keyboard.Modifiers == ModifierKeys.Control:
                multiSelector.SelectedIndex = 0;
                break;
            case Key.End when Keyboard.Modifiers == ModifierKeys.Control:
                multiSelector.SelectedIndex = multiSelector.Items.Count;
                break;
            case Key.A when Keyboard.Modifiers == ModifierKeys.Control:
                multiSelector.SelectAll();
                break;
            case Key.Space:
                {
                    var items = multiSelector.SelectedItems.Cast<dynamic>().ToArray();
                    _ = items.ForEachItem(instance => instance.IsChecked = items.Any(o => !o.IsChecked));
                    break;
                }

            default:
                break;
        }
    }

    public static bool IsDesignTime()
        => Application.Current?.MainWindow is null;

    public static bool MoveToNextUIElement()
        => Keyboard.FocusedElement is UIElement elementWithFocus && elementWithFocus.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

    public static void PerformClick(this Button button)
    {
        var peer = new ButtonAutomationPeer(button);
        var invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
        invokeProv?.Invoke();
    }

    public static void Rebind(this DependencyObject target, in DependencyProperty dependencyProperty)
        => BindingOperations.GetBindingExpressionBase(target, dependencyProperty)?.UpdateTarget();

    public static void Rebind(this TextBox target)
        => Rebind(target, TextBox.TextProperty);

    public static void RebindDataContext(this FrameworkElement element)
    {
        Check.IfArgumentNotNull(element, nameof(element));

        var dataContext = element.DataContext;
        element.DataContext = null;
        element.DataContext = dataContext;
    }

    public static void RebindItemsSource(this ItemsControl control)
    {
        Check.IfArgumentNotNull(control, nameof(control));

        control.Rebind(ItemsControl.ItemsSourceProperty);
        var itemsSource = control.ItemsSource;
        control.ItemsSource = null;
        control.ItemsSource = itemsSource;
    }

    public static void RebindItemsSource(this TreeView control)
    {
        Check.IfArgumentNotNull(control, nameof(control));

        control.Rebind(ItemsControl.ItemsSourceProperty);
        var selectedItem = control.SelectedItem;
        var itemsSource = control.ItemsSource;
        control.ItemsSource = null;
        control.ItemsSource = itemsSource;
        control.SetSelectedItem(selectedItem);
    }

    public static void Refresh(this UIElement uiElement)
    {
        Check.IfArgumentNotNull(uiElement, nameof(uiElement));

        _ = uiElement.Dispatcher.Invoke(DispatcherPriority.Render, Methods.Empty);
    }

    public static IEnumerable<dynamic>? RetieveCheckedItems(this MultiSelector dg)
        => dg?.Items.Cast<dynamic>().Where(item => item.IsChecked == true).Cast<object>().Select(item => item.As<dynamic>()).Compact();

    public static IEnumerable<TItem>? RetieveCheckedItems<TItem>(this MultiSelector dg)
        => dg?.Items.Cast<dynamic>().Where(item => item.IsChecked == true).Cast<object>().Select(item => item.To<TItem>());

    public static void RunInControlThread(this DispatcherObject control, in Action action)
        => control?.Dispatcher.Invoke(action);

    public static bool SetProperty<TValue>(
        this INotifyPropertyChanged item,
        ref TValue field,
        in TValue newValue,
        in Action<PropertyChangedEventArgs> invokePropertyChanged,
        [CallerMemberName] in string? propertyName = null)
    {
        if (!Equals(field, newValue))
        {
            field = newValue;
            invokePropertyChanged?.Invoke(new(propertyName));
            return true;
        }
        return false;
    }

    public static TViewModel SetProperty<TViewModel, TValue>(
        this TViewModel item,
        ref TValue field,
        in TValue newValue,
        in Action<string?> invokePropertyChanged,
        [CallerMemberName] in string? propertyName = null)
        where TViewModel : INotifyPropertyChanged
    {
        //if (!Equals(field, newValue))
        {
            field = newValue;
            invokePropertyChanged?.Invoke(propertyName);
        }
        return item;
    }

    public static void SetValue(this RangeBase pb, double step)
    {
        Check.IfArgumentNotNull(pb, nameof(pb));

        Animations.AnimateDouble(pb, RangeBase.ValueProperty, pb.Value, step, 100);
    }

    public static bool? ShowDialog<TWindow>(this Window owner)
        where TWindow : Window, new()
        => owner.ShowDialog(() => new TWindow(), out var window);

    public static (bool? Result, TWindow Window) ShowDialog<TWindow>(this Window owner, Func<TWindow> creator)
        where TWindow : Window
    {
        var result = owner.ShowDialog(creator, out var window);
        return (result, window);
    }

    public static bool? ShowDialog<TWindow>(this Window owner, Func<TWindow> creator, out TWindow window)
        where TWindow : Window
    {
        Check.IfArgumentNotNull(creator, nameof(creator));

        window = creator();
        window.Owner = owner;
        return window.ShowDialog();
    }

    public static bool? ShowDialog<TWindow>(this Window owner, out TWindow window)
        where TWindow : Window, new()
        => owner.ShowDialog(() => new TWindow(), out window);

    private static void BindItemsSourceInner<TSelector>(TSelector selector, IEnumerable items, string? displayMemebrPath) where TSelector : Selector
    {
        Check.IfArgumentNotNull(selector, nameof(selector));
        //selector.ItemsSource = null;
        selector.ItemsSource = items;
        if (displayMemebrPath is not null)
        {
            selector.DisplayMemberPath = displayMemebrPath;
        }
    }
}
