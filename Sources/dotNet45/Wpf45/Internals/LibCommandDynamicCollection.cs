﻿using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mohammad.Helpers;
using Mohammad.Wpf.Helpers;
using Mohammad.Wpf.Windows.Controls;
using Mohammad.Wpf.Windows.Input.LibCommands;

namespace Mohammad.Wpf.Internals
{
    public class LibUserControlDynamicCollection : DynamicObject, IEnumerable<LibUserControl>
    {
        internal LibUserControlDynamicCollection(FrameworkElement element) => this.Element = element;
        private FrameworkElement Element { get; }
        public dynamic this[string index] => this.Element.FindResource(index).As<LibUserControl>();
        public IEnumerator<LibUserControl> GetEnumerator() => this.GetAll().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this.GetAll().FirstOrDefault(cmd => cmd.Name == binder.Name || cmd.InnerName == binder.Name);
            return result != null || base.TryGetMember(binder, out result);
        }

        private IEnumerable<LibUserControl> GetAll() => this.Element.GetControls<LibUserControl>();
    }

    public class LibCommandDynamicCollection : DynamicObject, IEnumerable<LibCommand>
    {
        internal LibCommandDynamicCollection(FrameworkElement element) => this.Element = element;
        private FrameworkElement Element { get; }
        public LibCommand this[string index] => this.Element.FindResource(index).As<LibCommand>();
        public IEnumerator<LibCommand> GetEnumerator() => this.GetAll().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this.GetAll().FirstOrDefault(cmd => cmd.Name == binder.Name || cmd.CommandName == binder.Name);
            return result != null || base.TryGetMember(binder, out result);
        }

        public IEnumerable<LibCommand> GetByCategory(string category)
        {
            return this.GetAll().Where(c => c.Category == category);
        }

        private IEnumerable<LibCommand> GetAll()
        {
            foreach (
                var command in
                this.Element.Resources.Keys.Cast<object>().Select(key => this.Element.FindResource(key).As<LibCommand>()).Where(resource => resource != null))
            {
                yield return command;
            }

            foreach (var commandBar in this.Element.GetControls<CommandBar>())
            {
                foreach (var command in commandBar.AppCommands)
                {
                    yield return command;
                }

                foreach (var command in commandBar.PageCommands)
                {
                    yield return command;
                }
            }

            foreach (var control in this.Element.GetControls<ICommandSource>().Where(control => control.Command is LibCommand))
            {
                yield return control.Command.As<LibCommand>();
            }

            foreach (var item in from notifyIcon in this.Element.GetControls<NotifyIcon>()
                                 where notifyIcon?.ContextMenu != null
                                 from item in notifyIcon.ContextMenu.Items.Cast<MenuItem>().Where(item => item != null && item.Command is LibCommand)
                                 select item)
            {
                yield return item.Command.As<LibCommand>();
            }
        }
    }
}