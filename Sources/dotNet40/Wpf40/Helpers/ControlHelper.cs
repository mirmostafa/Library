#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using Library40.Helpers;

namespace Library40.Wpf.Helpers
{
	public static class ControlHelper
	{
		public static DispatcherOperation Run(this Control control, Action action, bool disableControlWhileOperation = true)
		{
			var result = control.Dispatcher.BeginInvoke(DispatcherPriority.Background,
				new Action(() =>
				           {
					           control.IsEnabled = false;
					           try
					           {
						           action();
					           }
					           finally
					           {
						           control.IsEnabled = true;
					           }
				           }));
			return result;
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
			var itemsSource = control.ItemsSource;
			control.ItemsSource = null;
			control.ItemsSource = itemsSource;
		}

		public static void RebindItemsSource(this TreeView control)
		{
			var selectedItem = control.SelectedItem;
			var itemsSource = control.ItemsSource;
			control.ItemsSource = null;
			control.ItemsSource = itemsSource;
			control.SetSelectedItem(selectedItem);
		}

		public static IEnumerable<TControl> GetControls<TControl>(this Visual control, bool loopThrouth = true) where TControl : class
		{
			var result = new List<TControl>();
			EnumerableHelper.ForEachTreeNode(control,
				c =>
				{
					if (loopThrouth || c == control)
					{
						if (c is TabItem)
							if (c.As<TabItem>().Content is Visual)
								return new[]
								       {
									       c.As<TabItem>().Content.As<Visual>(), c.As<TabItem>().Header.As<Visual>()
								       };
						return Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(c)).Select(i => (Visual)VisualTreeHelper.GetChild(c, i));
					}
					return Enumerable.Empty<Visual>();
				},
				c =>
				{
					if (c is TControl)
						result.Add(c as TControl);
				},
				null);
			return result;
		}

		public static string GetText(this RichTextBox rtb)
		{
			var textRange = new TextRange( // TextPointer to the start of content in the RichTextBox.
				rtb.Document.ContentStart,
				// TextPointer to the end of content in the RichTextBox.
				rtb.Document.ContentEnd);

			// The Text property on a TextRange object returns a string
			// representing the plain text content of the TextRange.
			return textRange.Text;
		}
	}
}