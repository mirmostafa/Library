//using System.Collections.Generic;
//using System.Windows;
//using System.Windows.Controls;
//using Library.Helpers;

//namespace Library.Wpf.Windows.Controls;

//public sealed class TreeViewExtender
//{
//    private static readonly Dictionary<DependencyObject, TreeViewSelectedItemBehavior> _Behaviors = new();

//    public static object? GetSelectedItem(DependencyObject obj) => obj?.GetValue(SelectedItemProperty);

//    public static void SetSelectedItem(DependencyObject obj, object value) => obj?.SetValue(SelectedItemProperty, value);

//    // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
//    public static readonly DependencyProperty SelectedItemProperty =
//        DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(TreeViewExtender), new UIPropertyMetadata(null, SelectedItemChanged));

//    private static void SelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
//    {
//        if (obj is not TreeView)
//        {
//            return;
//        }

//        if (!_Behaviors.ContainsKey(obj))
//        {
//            _Behaviors.Add(obj, new TreeViewSelectedItemBehavior(obj.As<TreeView>()!));
//        }

//        var view = _Behaviors[obj];
//        view.ChangeSelectedItem(e.NewValue);
//    }

//    private class TreeViewSelectedItemBehavior
//    {
//        private readonly TreeView _View;
//        public TreeViewSelectedItemBehavior(TreeView view)
//        {
//            this._View = view;
//            view.SelectedItemChanged += (sender, e) => SetSelectedItem(view, e.NewValue);
//        }

//        internal void ChangeSelectedItem(object p)
//        {
//            var item = (TreeViewItem)this._View.ItemContainerGenerator.ContainerFromItem(p);
//            item.IsSelected = true;
//        }
//    }
//}
