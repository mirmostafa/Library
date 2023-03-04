using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

using Library.ComponentModel;
using Library.Data.Models;
using Library.Wpf.Windows;

namespace Library.Wpf.Helpers;

public static class ControlHelper
{
    public static DataGrid AddColumns(this DataGrid dataGrid, IEnumerable<IDataColumnBindingInfo> dataColumns)
    {
        Check.IfArgumentNotNull(dataGrid);
        _ = dataColumns.ToDataGridColumn().ForEach(dataGrid.Columns.Add).Build();
        return dataGrid;
    }

    public static void ApplyFilter(this ItemCollection items, Predicate<object> predicate)
    {
        foreach (var item in items.Cast<TreeViewItem>())
        {
            ApplyFilter(item.Items, predicate);
        }
        items.Filter = predicate;
    }

    public static THeaderedItemsControl BindDataContext<THeaderedItemsControl>(this THeaderedItemsControl itemsControl, object datacontext, string? header = null)
                where THeaderedItemsControl : HeaderedItemsControl
    {
        Check.IfArgumentNotNull(itemsControl);

        itemsControl.DataContext = datacontext;
        itemsControl.Header = header ?? datacontext?.ToString();
        return itemsControl;
    }

    public static TItemsControl BindItems<TItemsControl>(this TItemsControl itemsControl, IEnumerable? items)
        where TItemsControl : ItemsControl
    {
        Check.IfArgumentNotNull(itemsControl);

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

    public static TSelector BindItemsSource<TSelector>(this TSelector selector, IEnumerable? items)
        where TSelector : Selector
    {
        BindItemsSourceInner(selector, items, null);
        return selector;
    }

    public static TSelector BindItemsSource<TSelector>(this TSelector selector, IEnumerable? items, string? displayMemebrPath)
        where TSelector : Selector
    {
        BindItemsSourceInner(selector, items, displayMemebrPath);
        return selector;
    }

    public static TSelector BindItemsSource<TSelector>(this TSelector selector, IEnumerable? items, string? displayMemebrPath, int? selectedIndex)
        where TSelector : Selector
    {
        BindItemsSourceInner(selector, items, displayMemebrPath);

        if (selectedIndex is not null)
        {
            selector.SelectedIndex = selectedIndex.Value;
        }
        return selector;
    }

    public static TSelector BindItemsSource<TSelector>(this TSelector selector, IEnumerable? items, string? displayMemebrPath, object? selectedValue)
        where TSelector : Selector
    {
        BindItemsSourceInner(selector, items, displayMemebrPath);

        if (selectedValue is not null)
        {
            selector.SelectedValue = selectedValue;
        }
        return selector;
    }

    public static Selector BindItemsSourceToEnum<TEnum>(this Selector selector, TEnum? selectedItem = null) where TEnum : struct => BindItemsSource(selector, Enum.GetValues(typeof(TEnum)), null, selectedItem);

    public static TreeViewItem BindNewItem(object datacontext, string? header = null) => new TreeViewItem().BindDataContext(datacontext, header);

    public static TreeViewItem BindNewItems(this TreeViewItem parentItem, IEnumerable items)
    {
        Check.IfArgumentNotNull(parentItem);
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
        BindingMode bindingMode = BindingMode.TwoWay) => target?.SetBinding(targetDependencyProperty, new Binding { Source = source, Path = new PropertyPath(path), Mode = bindingMode });

    public static void EnsureVisible(this ListViewItem item) => item.ArgumentNotNull(nameof(item)).Parent.As<ListView>()?.ScrollIntoView(item);

    public static void EnsureVisibleItem(this DataGrid dg, object item) => dg.ArgumentNotNull(nameof(dg)).ScrollIntoView(item);

    public static void EnsureVisibleItem(this ListBox lv, object item) => lv.ArgumentNotNull(nameof(lv)).ScrollIntoView(item);

    public static void ExpandAll(this TreeViewItem item, bool isExpanded = true)
    {
        Check.IfArgumentNotNull(item);

        var parent = item;
        do
        {
            parent.IsExpanded = isExpanded;
        } while ((parent = parent?.Parent.As<TreeViewItem>()) is not null);

        item.BringIntoView();
    }

    public static void Flick(FrameworkElement element, int duration = 500) => Catch(() => Animations.Flick(element, duration));

    public static IEnumerable<TreeViewItem> GetAllItems([DisallowNull] this TreeView tree) => EnumerableHelper.GetAll(() => tree.ArgumentNotNull(nameof(tree)).Items.Cast<TreeViewItem>(), item => item.Items.Cast<TreeViewItem>());

    public static DataGridCell? GetCell(this DataGrid grid, int row, int column)
    {
        Check.IfArgumentNotNull(grid);
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
        where TControl : class => visual.ArgumentNotNull(nameof(visual)).GetChildren(loopThrouth).OfType<TControl>();

    public static IEnumerable<Visual> GetChildren(this Visual visual, bool loopThrouth = true)
    {
        var result = new List<Visual>();
        EnumerableHelper.ForEachTreeNode(visual,
            c =>
            {
                return !loopThrouth && !Equals(c, visual)
                    ? Enumerable.Empty<Visual>()
                    : c is not TabItem tabItem
                        ? Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(c)).Select(i => (Visual)VisualTreeHelper.GetChild(c, i))
                        : tabItem.Content is Visual
                                            ? (IEnumerable<Visual>)(new[] { tabItem.Content.As<Visual>()!, tabItem.Header.As<Visual>()! })
                                            : Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(c)).Select(i => (Visual)VisualTreeHelper.GetChild(c, i));
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

    public static IntPtr GetHandle(this Window window) => new WindowInteropHelper(window).Handle;

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

    public static Window GetParentWindow(this DependencyObject dependencyObject) => Window.GetWindow(dependencyObject);

    public static DataGridRow GetRow(this DataGrid grid, int index)
    {
        Check.IfArgumentNotNull(grid);

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

    public static T? GetSelectedValue<T>(this TreeView treeView) where T : class
        => treeView.ArgumentNotNull(nameof(treeView)).SelectedItem.As<TreeViewItem>()?.DataContext.As<T>();

    [Obsolete("Use listView.GetSelection, instead.")]
    public static T? GetSelection<T>(this SelectionChangedEventArgs e)
        => e.ArgumentNotNull(nameof(e)).AddedItems.Cast<object?>().FirstOrDefault().To<T?>();

    public static T? GetSelection<T>([DisallowNull] this ListView listView, SelectionChangedEventArgs e)
        => e?.AddedItems.Any() is true ? e.AddedItems[0].To<T?>() : GetSelections<T>(listView).FirstOrDefault();

    public static IEnumerable<T?> GetSelections<T>([DisallowNull] this ListView listView)
        => listView.ArgumentNotNull().SelectedItems.Cast<object?>().Select(x => x.To<T?>());

    public static string GetText([DisallowNull] this RichTextBox rtb)
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

    public static TItemsControl HandleChanges<TItemsControl, TNotifyCollectionChanged>(this TItemsControl control, TNotifyCollectionChanged collection, Action<TItemsControl, TNotifyCollectionChanged, NotifyCollectionChangedEventArgs> handler)
        where TItemsControl : ItemsControl
        where TNotifyCollectionChanged : INotifyCollectionChanged
    {
        collection.CollectionChanged += (e1, e2) => handler?.Invoke(control, collection, e2);
        return control;
    }

    public static void HandleKeyDown(this MultiSelector multiSelector, KeyEventArgs e)
    {
        Check.IfArgumentNotNull(multiSelector);

        Check.IfArgumentNotNull(e);

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

    public static void InitializeChildren(Visual parent)
    {
        var children = parent.GetChildren();
        foreach (var child in children)
        {
            if (child is IInitialzable initialzable)
            {
                initialzable.Initialize();
            }
            InitializeChildren(child);
        }
    }
    public static async Task InitializeChildrenAsync(Visual parent)
    {
        var children = parent.GetChildren();
        foreach (var child in children)
        {
            if (child is IAsyncInitialzable initialzable)
            {
                await initialzable.InitializeAsync();
            }
            InitializeChildren(child);
        }
    }

    public static bool IsDesignTime() => Application.Current?.MainWindow is null;

    public static bool MoveToNextUIElement() => Keyboard.FocusedElement is UIElement elementWithFocus && elementWithFocus.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

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

    public static void RebindDataContext([DisallowNull] this FrameworkElement element)
        => RebindDataContext(element, element?.DataContext);

    public static void RebindDataContext([DisallowNull] this FrameworkElement element, object? dataContext)
    {
        Check.IfArgumentNotNull(element);

        element.DataContext = null;
        element.DataContext = dataContext;
    }

    public static ItemsControl RebindItemsSource(this ItemsControl control)
        => RebindItemsSource(control, control.ItemsSource);

    public static ItemsControl RebindItemsSource(this ItemsControl control, in IEnumerable items)
    {
        Check.IfArgumentNotNull(control);

        control.Rebind(ItemsControl.ItemsSourceProperty);
        control.ItemsSource = null;
        control.ItemsSource = items;
        return control;
    }

    public static void RebindItemsSource(this TreeView control)
    {
        Check.IfArgumentNotNull(control);

        control.Rebind(ItemsControl.ItemsSourceProperty);
        var selectedItem = control.SelectedItem;
        var itemsSource = control.ItemsSource;
        control.ItemsSource = null;
        control.ItemsSource = itemsSource;
        control.SetSelectedItem(selectedItem);
    }

    public static void Refresh(this UIElement uiElement)
        => uiElement?.Dispatcher.Invoke(DispatcherPriority.Render, Methods.Empty);

    public static IEnumerable<dynamic>? RetieveCheckedItems(this MultiSelector dg)
        => dg?.Items.Cast<dynamic>().Where(item => item.IsChecked == true).Cast<object>().Select(item => item.As<dynamic>()).Compact();

    public static IEnumerable<TItem>? RetieveCheckedItems<TItem>(this MultiSelector dg)
        => dg?.Items.Cast<dynamic>().Where(item => item.IsChecked == true).Cast<object>().Select(item => item.To<TItem>());

    public static TResult RunCodeBlock<TResult>(this FrameworkElement element, [DisallowNull] in Func<TResult> action, [DisallowNull] in ILogger logger, in string? start, in string? end = null, in string? error = null, bool changeMousePointer = true)
    {
        Check.IfArgumentNotNull(action);
        Check.IfArgumentNotNull(element);
        Check.IfArgumentNotNull(logger);
        Check.IfArgumentNotNull(action);

        var cursor = element.Cursor;
        try
        {
            if (!start.IsNullOrEmpty())
            {
                logger.Debug(start);
            }
            if (changeMousePointer)
            {
                element.Cursor = Cursors.Wait;
            }

            var result = action();
            if (!end.IsNullOrEmpty())
            {
                logger.Info(end);
            }
            return result;
        }
        catch
        {
            if (!error.IsNullOrEmpty())
            {
                logger.Error(error);
            }

            throw;
        }
        finally
        {
            element.Cursor = cursor;
        }
    }

    public static FrameworkElement RunCodeBlock(this FrameworkElement element, [DisallowNull] Action action, [DisallowNull] in ILogger logger, in string? start, in string? end = null, in string? error = null, bool changeMousePointer = true) => RunCodeBlock(element, () =>
                                                                                                                                                                                                                                                         {
                                                                                                                                                                                                                                                             action();
                                                                                                                                                                                                                                                             return element;
                                                                                                                                                                                                                                                         }, logger, start, end, error, changeMousePointer);

    public static async Task<TResult> RunCodeBlockAsync<TResult>(this FrameworkElement element, [DisallowNull] Func<Task<TResult>> action, [DisallowNull] ILogger logger, string? start, string? end = null, string? error = null, bool changeMousePointer = true) => await RunCodeBlock(element, action, logger, start, end, error, changeMousePointer);

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
        Check.IfArgumentNotNull(pb);

        Animations.AnimateDouble(pb, RangeBase.ValueProperty, pb.Value, step, 100);
    }

    public static bool? ShowDialog<TWindow>(this Window owner)
        where TWindow : Window, new() => owner.ShowDialog(() => new TWindow(), out var window);

    public static (bool? Result, TWindow Window) ShowDialog<TWindow>(this Window owner, Func<TWindow> creator)
        where TWindow : Window
    {
        var result = owner.ShowDialog(creator, out var window);
        return (result, window);
    }

    public static bool? ShowDialog<TWindow>(this Window owner, Func<TWindow> creator, out TWindow window)
        where TWindow : Window
    {
        Check.IfArgumentNotNull(creator);

        window = creator();
        window.Owner = owner;
        return window.ShowDialog();
    }

    public static bool? ShowDialog<TWindow>(this Window owner, out TWindow window)
        where TWindow : Window, new() => owner.ShowDialog(() => new TWindow(), out window);

    private static void BindItemsSourceInner<TSelector>(TSelector selector, IEnumerable? items, string? displayMemebrPath)
        where TSelector : Selector
    {
        Check.IfArgumentNotNull(selector);

        selector.ItemsSource = null;
        selector.ItemsSource = items;
        if (displayMemebrPath is not null)
        {
            selector.DisplayMemberPath = displayMemebrPath;
        }
    }
    
    public static TStatefulPage SetIsViewModelChanged<TStatefulPage>(this TStatefulPage page, bool isViewModelChanged)
        where TStatefulPage : IStatefulPage
    {
        page.IsViewModelChanged = isViewModelChanged;
        return page;
    }

}