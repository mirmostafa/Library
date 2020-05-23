#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Linq;
using Library40.Collections.ObjectModel;

namespace Library40.Helpers.Console.Controls
{
	public class MenuItem
	{
		private Menu _DropDownMenu;

		public MenuItem(string text, ConsoleKey key, EventHandler handler)
		{
			this.Text = text;
			this.Key = key;
			this.Handler = handler;
		}

		public ConsoleKey Key { get; set; }

		public EventHandler Handler { get; set; }

		public string Text { get; set; }

		public Menu DropDownMenu
		{
			get
			{
				if (this._DropDownMenu == null)
					this._DropDownMenu = new Menu();
				return this._DropDownMenu;
			}
			set { this._DropDownMenu = value; }
		}
	}

	public class Menu
	{
		private EventualCollection<MenuItem> _Items;

		public EventualCollection<MenuItem> Items
		{
			get
			{
				if (this._Items == null)
					this._Items = new EventualCollection<MenuItem>();
				return this._Items;
			}
		}

		public MenuItem AddItem(string text, ConsoleKey key, EventHandler handler)
		{
			var result = new MenuItem(text, key, handler);
			this.Items.Add(result);
			return result;
		}

		public void Show()
		{
			var selectedKey = Helper.PressKeyInRange(this.Display, this.Items.Select(i => i.Key).ToArray());
			var selectedItem = this.Items.Where(item => item.Key == selectedKey).First();
			if (selectedItem.DropDownMenu.Items.Count > 0)
				selectedItem.DropDownMenu.Show();
			else
				selectedItem.Handler(this, EventArgs.Empty);
		}

		private void Display()
		{
			lock (this)
			{
				this.OnDisplaying();
				var formatter = string.Concat("{0,-", System.Console.WindowWidth - 2, "}");
				var itemFormatter = string.Concat("{0,10} {1,-", System.Console.WindowWidth - 13, "}");
				string.Format(formatter, "Main menu").WriteLine();
				string.Format(formatter, "==========================").WriteLine();
				foreach (var item in this.Items)
					string.Format(itemFormatter, item.Key, item.Text).WriteLine();
				string.Format(formatter, "Choose an item").WriteLine();
				this.OnDisplayed();
			}
		}

		private void OnDisplaying()
		{
			this.Displaying.Raise(this);
		}

		public event EventHandler Displayed;
		public event EventHandler Displaying;

		private void OnDisplayed()
		{
			this.Displayed.Raise(this);
		}
	}
}