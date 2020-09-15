using System;
using System.Collections.Generic;
using System.Linq;

namespace Mohammad.Helpers.Console.Menu
{
    public class ConsoleMenuItemHotKeyPressedEventArgs : EventArgs
    {
        public ConsoleMenuItemHotKeyPressedEventArgs(object extraArgs) => this.ExtraArgs = extraArgs;
        public bool Back { get; set; }
        public bool Exit { get; set; }
        public object ExtraArgs { get; set; }
    }

    public class ConsoleMenuItem : IEquatable<ConsoleMenuItem>
    {
        private readonly object _ExtraArgs;

        public ConsoleMenuItem(char hotKey, string text, IEnumerable<ConsoleMenuItem> children)
            : this(hotKey, text, children, null, null)
        {
        }

        public ConsoleMenuItem(char hotKey, string text, EventHandler<ConsoleMenuItemHotKeyPressedEventArgs> hotKeyPressed, object extraArgs)
            : this(hotKey, text, null, hotKeyPressed, extraArgs)
        {
        }

        public ConsoleMenuItem(char hotKey, string text, EventHandler<ConsoleMenuItemHotKeyPressedEventArgs> hotKeyPressed)
            : this(hotKey, text, null, hotKeyPressed, null)
        {
        }

        public ConsoleMenuItem(char hotKey,
            string text,
            IEnumerable<ConsoleMenuItem> children,
            EventHandler<ConsoleMenuItemHotKeyPressedEventArgs> hotKeyPressed,
            object extraArgs)
        {
            this._ExtraArgs = extraArgs;
            this.HotKey = hotKey;
            this.Text = text;
            if (children != null)
            {
                this.Children = new ConsoleMenuItemList(children);
            }

            if (hotKeyPressed != null)
            {
                this.HotKeyPressed += hotKeyPressed;
            }
        }

        public ConsoleMenuItemList Children { get; } = new ConsoleMenuItemList();
        public char HotKey { get; internal set; }

        public string Text { get; }

        public bool Equals(ConsoleMenuItem other) => other == this;

        public static bool operator ==(ConsoleMenuItem menu1, ConsoleMenuItem menu2)
        {
            // ReSharper disable PossibleNullReferenceException
            var key1 = CodeHelper.CatchFunc(() => menu1.HotKey, ' ');
            var key2 = CodeHelper.CatchFunc(() => menu2.HotKey, ' ');
            // ReSharper restore PossibleNullReferenceException
            return key1 == key2;
        }

        public static bool operator !=(ConsoleMenuItem menu1, ConsoleMenuItem menu2) => !(menu1 == menu2);

        public static ConsoleMenuItem Show(IEnumerable<ConsoleMenuItem> menuItems,
            string menuTitle,
            string choisePrompt = null,
            bool? showSelectedKey = null,
            bool autoAssignKey = true)
        {
            var items = menuItems as IList<ConsoleMenuItem> ?? menuItems.ToList();
            var isExitRequested = false;
            ConsoleMenuItem selectedItem = null;
            while (true)
            {
                if (isExitRequested)
                {
                    return selectedItem;
                }

                menuTitle.WriteLine(ConsoleColor.White);
                ConsoleHelper.WriteSeparatorLine();
                var index = 0;
                foreach (var child in items)
                {
                    if (child.HotKey == '_')
                    {
                        AddSeparator();
                        continue;
                    }

                    "\t".Write();
                    if (autoAssignKey && (child.HotKey == ' ' || child.HotKey == char.MinValue))
                    {
                        var hotKey = Convert.ToChar(index + 65);
                        while (items.Any(item => char.ToLower(item.HotKey) == char.ToLower(hotKey)))
                        {
                            index++;
                            hotKey = Convert.ToChar(index + 65);
                        }

                        child.HotKey = hotKey;
                    }

                    child.HotKey.Write(ConsoleColor.Green);
                    " - ".Write();
                    ConsoleHelper.Inform(child.Text);
                }

                var response = ConsoleHelper.AskKey(choisePrompt + " ", !showSelectedKey ?? string.IsNullOrEmpty(choisePrompt));
                var key = response.KeyChar;
                ConsoleHelper.LineFeed();
                selectedItem = items.FirstOrDefault(c => char.ToLower(c.HotKey) == char.ToLower(key));
                if (selectedItem != null)
                {
                    if (selectedItem.Children.Any())
                    {
                        Show(selectedItem.Children, selectedItem.Text, choisePrompt, showSelectedKey);
                        continue;
                    }

                    var e = new ConsoleMenuItemHotKeyPressedEventArgs(selectedItem._ExtraArgs);
                    selectedItem.OnHotKeyPressed(e);
                    if (e.Exit)
                    {
                        isExitRequested = true;
                    }

                    if (e.Back)
                    {
                        return selectedItem;
                    }
                }
                else
                {
                    ConsoleHelper.Error("Invalid key.");
                }
            }
        }

        public static ConsoleKeyInfo ShowSimpleMenu(IDictionary<char, string> items,
            string caption = null,
            string prompt = null,
            bool? showSelectedKey = null)
        {
            caption.WriteLine(ConsoleColor.White);
            ConsoleHelper.WriteSeparatorLine();
            foreach (var item in items)
            {
                if (item.Key == '_')
                {
                    AddSeparator();
                    continue;
                }

                DrawItem(item);
            }

            while (true)
            {
                var result = ConsoleHelper.AskKey(prompt + " ", !showSelectedKey ?? string.IsNullOrEmpty(prompt));
                var key = result.KeyChar;
                ConsoleHelper.LineFeed();
                if (key != '_')
                {
                    if (items.ContainsKey(key))
                    {
                        return result;
                    }
                }

                ConsoleHelper.Error("Invalid key.");
            }
        }

        private static void AddSeparator() => ConsoleHelper.Inform(" ---------------------------------");

        private static void DrawItem(KeyValuePair<char, string> item)
        {
            "\t".Write();
            item.Key.Write(ConsoleColor.Green);
            " - ".Write();
            ConsoleHelper.Inform(item.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((ConsoleMenuItem)obj);
        }

        public override int GetHashCode() => this.HotKey;

        protected virtual void OnHotKeyPressed(ConsoleMenuItemHotKeyPressedEventArgs e) => this.HotKeyPressed?.Invoke(this, e);

        public event EventHandler<ConsoleMenuItemHotKeyPressedEventArgs> HotKeyPressed;
    }

    public class ConsoleMenuItemList : List<ConsoleMenuItem>
    {
        public ConsoleMenuItemList()
        {
        }

        public ConsoleMenuItemList(IEnumerable<ConsoleMenuItem> children)
            : base(children)
        {
        }

        public ConsoleMenuItem Add(char hotKey,
            string text,
            IEnumerable<ConsoleMenuItem> children,
            EventHandler<ConsoleMenuItemHotKeyPressedEventArgs> hotKeyPressed,
            object extraArgs)
        {
            var result = new ConsoleMenuItem(hotKey, text, children, hotKeyPressed, extraArgs);
            this.Add(result);
            return result;
        }

        public ConsoleMenuItem Add(char hotKey, string text, EventHandler<ConsoleMenuItemHotKeyPressedEventArgs> hotKeyPressed, object extraArgs)
        {
            var result = new ConsoleMenuItem(hotKey, text, hotKeyPressed, extraArgs);
            this.Add(result);
            return result;
        }

        public ConsoleMenuItem Add(char hotKey, string text, EventHandler<ConsoleMenuItemHotKeyPressedEventArgs> hotKeyPressed)
        {
            var result = new ConsoleMenuItem(hotKey, text, hotKeyPressed);
            this.Add(result);
            return result;
        }

        public ConsoleMenuItem Add(char hotKey, string text, IEnumerable<ConsoleMenuItem> children)
        {
            var result = new ConsoleMenuItem(hotKey, text, children);
            this.Add(result);
            return result;
        }

        public void Add(char separator = '-') => this.Add(new ConsoleMenuItem('_', "_", (IEnumerable<ConsoleMenuItem>)null));

        public void AddSeparator(char separator = '-') => this.Add(new ConsoleMenuItem('_', "_", (IEnumerable<ConsoleMenuItem>)null));
    }
}