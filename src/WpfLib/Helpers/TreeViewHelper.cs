using System.IO;

namespace Library.Wpf.Helpers;

public static class TreeViewHelper
{
    /// <summary>
    /// Selects an item in a TreeView using a path
    /// </summary>
    /// <param name="treeView">The TreeView to select an item in</param>
    /// <param name="path">
    /// The path to the selected item. Components of the path are separated with
    /// Path.DirectorySeparatorChar. Items in the control are converted by calling the ToString method.
    /// </param>
    public static void SetSelectedItem(this TreeView treeView, string path)
        => treeView.SetSelectedItem(path, item => item.ToString() ?? string.Empty);

    /// <summary>
    /// Selects an item in a TreeView using a path and a custom conversion method
    /// </summary>
    /// <param name="treeView">The TreeView to select an item in</param>
    /// <param name="path">
    /// The path to the selected item. Components of the path are separated with Path.DirectorySeparatorChar.
    /// </param>
    /// <param name="convertMethod">
    /// A custom method that converts items in the control to their respective path component
    /// </param>
    public static void SetSelectedItem(this TreeView treeView, string path, Func<object, string> convertMethod)
        => treeView.SetSelectedItem(path, convertMethod, Path.DirectorySeparatorChar);

    /// <summary>
    /// Selects an item in a TreeView using a path and a custom path separator character.
    /// </summary>
    /// <param name="treeView">The TreeView to select an item in</param>
    /// <param name="path">The path to the selected item</param>
    /// <param name="separatorChar">The character that separates path components</param>
    public static void SetSelectedItem(this TreeView treeView, string path, char separatorChar)
        => treeView.SetSelectedItem(path, item => item?.ToString() ?? string.Empty, separatorChar);

    /// <summary>
    /// Selects an item in a TreeView using a path, a custom conversion method, and a custom path
    /// separator character.
    /// </summary>
    /// <param name="treeView">The TreeView to select an item in</param>
    /// <param name="path">The path to the selected item</param>
    /// <param name="convertMethod">
    /// A custom method that converts items in the control to their respective path component
    /// </param>
    /// <param name="separatorChar">The character that separates path components</param>
    public static void SetSelectedItem(this TreeView treeView, string path, Func<object, string> convertMethod, char separatorChar)
        => treeView.SetSelectedItem(path.ArgumentNotNull().Split(new[] { separatorChar }, StringSplitOptions.RemoveEmptyEntries), (x, y) => x == y, convertMethod);

    /// <summary>
    /// Selects an item in a TreeView using a custom item chain
    /// </summary>
    /// <typeparam name="T">The type of the items present in the control and the chain</typeparam>
    /// <param name="treeView">The TreeView to select an item in</param>
    /// <param name="items">The chain of items to walk. The last item in the chain will be selected</param>
    public static void SetSelectedItem<T>(this TreeView treeView, IEnumerable<T> items)
        where T : class => treeView.SetSelectedItem(items, (x, y) => x == y);

    /// <summary>
    /// Selects an item in a TreeView using a custom item chain
    /// </summary>
    /// <typeparam name="TItem">The type of the items present in the control and the chain</typeparam>
    /// <param name="treeView">The TreeView to select an item in</param>
    /// <param name="items">The chain of items to walk. The last item in the chain will be selected</param>
    public static TTreeView SetSelectedItem<TTreeView, TItem>(this TTreeView treeView, params TItem?[] items)
        where TTreeView : TreeView
        where TItem : class => treeView.Fluent(() => treeView.SetSelectedItem(items, (x, y) => x == y));

    /// <summary>
    /// Selects an item in a TreeView using a custom item chain and item comparison method
    /// </summary>
    /// <typeparam name="T">The type of the items present in the control and the chain</typeparam>
    /// <param name="treeView">The TreeView to select an item in</param>
    /// <param name="items">The chain of items to walk. The last item in the chain will be selected</param>
    /// <param name="compareMethod">
    /// The method used to compare items in the control with items in the chain
    /// </param>
    public static void SetSelectedItem<T>(this TreeView treeView, IEnumerable<T> items, Func<T, T, bool> compareMethod)
        => treeView.SetSelectedItem(items, compareMethod, null);

    /// <summary>
    /// Selects an item in a TreeView using a custom item chain, an item comparison method, and an
    /// item conversion method.
    /// </summary>
    /// <typeparam name="T">The type of the items present in the control and the chain</typeparam>
    /// <param name="treeView">The TreeView to select an item in</param>
    /// <param name="items">The chain of items to walk. The last item in the chain will be selected</param>
    /// <param name="compareMethod">
    /// The method used to compare items in the control with items in the chain
    /// </param>
    /// <param name="convertMethod">
    /// The method used to convert items in the control to be compared with items in the chain
    /// </param>
    public static void SetSelectedItem<T>(this TreeView treeView, IEnumerable<T> items, Func<T, T, bool> compareMethod, Func<object, T>? convertMethod) =>
        ControlHelper.SetSelectedItem(treeView,
            new SetSelectedInfo<T>
            {
                Items = items,
                CompareMethod = compareMethod,
                ConvertMethod = convertMethod,
                OnSelected = delegate (ItemsControl container, SetSelectedInfo<T> info)
                {
                    var treeItem = (TreeViewItem)container;
                    treeItem.IsSelected = true;
                    treeItem.BringIntoView();
                },
                OnNeedMoreItems =
                    delegate (ItemsControl container, SetSelectedInfo<T> info)
                    {
                        ((TreeViewItem)container).IsExpanded = true;
                    }
            });

    public static TTreeView SetSelectedItemIndex<TTreeView>(this TTreeView treeView, int index)
        where TTreeView : TreeView =>
        treeView.Fluent(treeView.Items
             .ToEnumerable()
             .ElementAt(index).Cast().As<TreeViewItem>().NotNull(() => "TreeView has not items, or the items are not `TreeViewItem`.")
             .IsSelected = true);
}