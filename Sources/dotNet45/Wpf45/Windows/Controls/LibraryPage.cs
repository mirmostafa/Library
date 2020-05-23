using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Mohammad.Helpers;
using Mohammad.Wpf.Helpers;
using Mohammad.Wpf.Internals;
using Mohammad.Wpf.Windows.Input.LibCommands;

namespace Mohammad.Wpf.Windows.Controls
{
    public class LibraryPage : PageBase
    {
        private Type _CommandsStaticClassType;
        private string _Description;

        protected Type CommandsStaticClassType
        {
            get { return this._CommandsStaticClassType; }
            set
            {
                this._CommandsStaticClassType = value;
                if (value != null)
                    LibCommandManager.Initialize(this, this.CommandsStaticClassType);
            }
        }

        protected LibCommandDynamicCollection Commands { get; }
        protected LibUserControlDynamicCollection Controls { get; set; }

        public new string Title
        {
            get { return base.Title; }
            set
            {
                if (value == base.Title)
                    return;
                base.Title = value;
                this.OnTitleChanged();
                this.OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return this._Description; }
            set
            {
                if (value == this._Description)
                    return;
                this._Description = value;
                this.OnPropertyChanged();
            }
        }

        public Window Owner
        {
            get { return this.Window; }
            set
            {
                if (Equals(value, this.Window))
                    return;
                this.Window = value;
                this.OnPropertyChanged();
            }
        }

        public LibraryPage()
        {
            if (LibraryApplication.InnerCommandsStaticClassType != null)
                LibCommandManager.Initialize(this, LibraryApplication.InnerCommandsStaticClassType);

            this.Commands = new LibCommandDynamicCollection(this);
            this.Controls = new LibUserControlDynamicCollection(this);
            this.Loaded += this.LibraryPage_OnLoaded;
        }

        protected LibraryPage(Window window)
            : base(window) { }

        public override void OnApplyTemplate()
        {
            //this.MoveBarsToParent();
            base.OnApplyTemplate();
        }

        private void LibraryPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this.MoveBarsToParent();
            foreach (var command in this.Commands)
                command.Initialize(this);
            foreach (var commandBar in this.GetControls<CommandBar>())
            {
                foreach (var command in commandBar.AppCommands)
                    command.Initialize(this);
                foreach (var command in commandBar.PageCommands)
                    command.Initialize(this);
            }
        }

        private void MoveBarsToParent()
        {
            if (this.Window == null)
                return;
            var windowCommandBar = this.Window.GetControls<CommandBar>().FirstOrDefault();
            if (windowCommandBar != null)
            {
                var commandBar = this.GetControls<CommandBar>().FirstOrDefault();
                if (commandBar != null)
                {
                    windowCommandBar.AppCommands.AddRange(commandBar.AppCommands);
                    windowCommandBar.PageCommands.AddRange(commandBar.PageCommands);
                    windowCommandBar.AddCommands(commandBar.AppCommands, commandBar.PageCommands);
                    commandBar.Visibility = Visibility.Collapsed;
                }
            }

            var windowButtonBar = this.Window.GetControls<ButtonBar>().FirstOrDefault();
            if (windowButtonBar == null)
                return;
            var myButtonBar = this.Controls.FirstOrDefault(c => c is ButtonBar).As<ButtonBar>();
            if (myButtonBar == null)
                return;
            windowButtonBar.AppButtons.AddRange(myButtonBar.AppButtons);
            windowButtonBar.PageButtons.AddRange(myButtonBar.PageButtons);

            //myButtonBar.AppButtons.ForEach(btn => btn.Parent.As<ContainerVisual>().Children.Remove(btn));
            myButtonBar.PageButtons.ForEach(btn =>
            {
                btn.Parent?.As<StackPanel>().Children.Remove(btn);
                windowButtonBar.PageButtonPanel.Children.Add(btn);
            });
            myButtonBar.AppButtons.RemoveAll(_ => true);
            myButtonBar.PageButtons.RemoveAll(_ => true);
            myButtonBar.Visibility = Visibility.Collapsed;

            windowButtonBar.AddButtons(myButtonBar.AppButtons, myButtonBar.PageButtons);
        }

        protected TResource FindResource<TResource>(string resourceKey) => this.FindResource(resourceKey).To<TResource>();
    }
}