using System;
using Library45.Wpf.Windows.Controls;

namespace Library45.Wpf.EventsArgs
{
	public class DialogClosedEventArgs : EventArgs
	{
		public DialogClosedEventArgs(LibraryCommonPage page, bool? dialogResult)
		{
			this.DialogResult = dialogResult;
			this.Page = page;
		}
		public LibraryCommonPage Page { get; private set; }
		public bool? DialogResult { get; private set; }
	}
}