using System;
using System.Linq;
using Mohammad.Collections.ObjectModel;

namespace Mohammad.Helpers.Console.Controls
{
    public class MenuItem
    {
        private Menu _DropDownMenu;
        public ConsoleKey Key { get; set; }
        public EventHandler Handler { get; set; }
        public string Text { get; set; }
        public Menu DropDownMenu { get { return this._DropDownMenu ?? (this._DropDownMenu = new Menu()); } set { this._DropDownMenu = value; } }

        public MenuItem(string text, ConsoleKey key, EventHandler handler)
        {
            this.Text = text;
            this.Key = key;
            this.Handler = handler;
        }
    }

    public class Menu
    {
        private EventualCollection<MenuItem> _Items;
        public EventualCollection<MenuItem> Items => this._Items ?? (this._Items = new EventualCollection<MenuItem>());

        public MenuItem AddItem(string text, ConsoleKey key, EventHandler handler)
        {
            var result = new MenuItem(text, key, handler);
            this.Items.Add(result);
            return result;
        }

        public void Show()
        {
            var selectedKey = ConsoleHelper.PressKeyInRange(this.Display, this.Items.Select(i => i.Key).ToArray());
            var selectedItem = this.Items.First(item => item.Key == selectedKey);
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

        private void OnDisplaying() { this.Displaying.RaiseAsync(this); }
        public event EventHandler Displayed;
        public event EventHandler Displaying;
        private void OnDisplayed() { this.Displayed.RaiseAsync(this); }
    }
}