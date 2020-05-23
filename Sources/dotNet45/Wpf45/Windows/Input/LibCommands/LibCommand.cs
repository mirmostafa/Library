using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.Wpf.Windows.Controls;
using Mohammad.Wpf.Windows.Controls.Tiles;

// ReSharper disable TryCastAlwaysSucceeds

namespace Mohammad.Wpf.Windows.Input.LibCommands
{
    public class LibCommand : RoutedUICommand, IEnumerable<UIElement>, INotifyPropertyChanged, ILibCommand
    {
        private readonly CommandBinding _CommandBinding;
        protected readonly List<UIElementController> Controllers = new List<UIElementController>();
        private string _Body;
        private string _Category;
        private object _Content;
        private string _Header;
        private bool _IsCancel;
        private bool? _IsChecked;
        private bool _IsDefault;
        private bool _IsEnabled = true;
        private Path _Path;
        private Brush _PathFill;
        private Style _PathStyle;
        private string _ToolTip;
        private Visibility _Visibility = Visibility.Visible;

        public string Category
        {
            get { return this._Category; }
            set
            {
                this._Category = value;
                this.OnPropertyChanged();
            }
        }

        public string CommandName { get; set; }

        public Style PathStyle
        {
            get { return this._PathStyle; }
            set
            {
                if (value == this._PathStyle)
                    return;
                this._PathStyle = value;
                this.OnPropChanged(value);
            }
        }

        public bool IsDefault
        {
            get { return this._IsDefault; }
            set
            {
                this._IsDefault = value;
                this.OnPropChanged(value);
            }
        }

        public bool IsCancel
        {
            get { return this._IsCancel; }
            set
            {
                this._IsCancel = value;
                this.OnPropChanged(value);
            }
        }

        public string ToolTip
        {
            get { return this._ToolTip; }
            set
            {
                if (value.Equals(this._ToolTip))
                    return;
                this._ToolTip = value;
                this.OnPropChanged(value);
            }
        }

        public LibCommand MyCommand { get; set; }

        public Path Path
        {
            get { return this._Path; }
            set
            {
                if (value.Equals(this._Path))
                    return;
                this._Path = value;
                this.OnPropChanged(value);
            }
        }

        public Brush PathFill
        {
            get { return this._PathFill; }
            set
            {
                if (value.Equals(this._PathFill))
                    return;
                this._PathFill = value;
                this.OnPropChanged(value);
            }
        }

        public string Header
        {
            get { return this._Header; }
            set
            {
                if (this._Header == value)
                    return;
                this._Header = value;
                this.OnPropChanged(value);
            }
        }

        public string Body
        {
            get { return this._Body; }
            set
            {
                if (this._Body == value)
                    return;
                this._Body = value;
                this.OnPropChanged(value);
            }
        }

        protected UIElement Parent { get; set; }

        public bool? IsChecked
        {
            get { return this._IsChecked; }
            set
            {
                if (this._IsChecked == value)
                    return;
                this._IsChecked = value;
                this.OnPropChanged(value);
                this.OnIsCheckedChanged(new ItemActedEventArgs<bool?>(this._IsChecked));
            }
        }

        public KeyGesture KeyGesture { get; set; }

        public object Parameter { get; set; }

        public LibCommand(string text)
            : this(text, null) {}

        public LibCommand(string text, string commandName)
        {
            this.Initailize(text, commandName);
            this._CommandBinding = new CommandBinding(this);
        }

        public LibCommand() { this._CommandBinding = new CommandBinding(this); }

        public LibCommand(string text, string name, Type ownerType)
            : base(text, name, ownerType)
        {
            this.Initailize(text, name);
            this._CommandBinding = new CommandBinding(this);
        }

        public LibCommand(string text, string name, Type ownerType, InputGestureCollection inputGestures)
            : base(text, name, ownerType, inputGestures)
        {
            this.Initailize(text, name);
            this._CommandBinding = new CommandBinding(this);
        }

        public event EventHandler<ItemActedEventArgs<bool?>> IsCheckedChanged;
        public static void SetMyCommand(UIElement element, LibCommand value) { value.AddElement(element); }

        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static LibCommand GetMyCommand(UIElement element) => null;

        private void Initailize(string content, string commandName)
        {
            this.Content = content;
            this.CommandName = commandName;
        }

        public bool CanExecute() => this.OnCanExecute();

        public void Execute()
        {
            if (!this.CanExecute())
                return;
            try
            {
                var ing = new ActingEventArgs();
                this.Executing.Raise(this, ing);
                if (ing.Handled)
                    return;
                this.OnExecuted();
                this.Executed.Raise(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                var exin = new ItemActingEventArgs<Exception>(ex);
                this.ExceptionOccurred.Raise(this, exin);
                if (exin.Handled)
                    return;
                throw;
            }
        }

        protected virtual bool OnCanExecute() { return this.IsEnabled; }

        protected virtual void HandleEvents(UIElement element)
        {
            if (element is RibbonApplicationMenuItem)
            {
                var btn = element as RibbonApplicationMenuItem;
                btn.Click += (_, __) => this.Execute();
                btn.Command = this;
            }
            else if (element is RibbonButton)
            {
                var btn = element as RibbonButton;
                btn.Click += (_, __) => this.Execute();
                btn.Command = this;
            }
            else if (element is ToggleButton)
            {
                var btn = element as ToggleButton;
                btn.Command = this;
                btn.Checked += (sender, _) => { this.IsChecked = ((ToggleButton) sender).IsChecked; };
                btn.Unchecked += (sender, _) => { this.IsChecked = ((ToggleButton) sender).IsChecked; };
            }
            else if (element is ButtonBase)
            {
                var btn = element as ButtonBase;
                btn.Click += (_, __) => this.Execute();
                btn.Command = this;
            }
            else if (element is MenuItem)
            {
                var btn = element as MenuItem;
                btn.Click += (_, __) => this.Execute();
                btn.Command = this;
            }
            else if (element is BaseTile)
            {
                var btn = element as BaseTile;
                btn.Click += (_, __) => this.Execute();
                btn.Command = this;
            }
            else if (element is TextBoxButton)
            {
                var btn = element as TextBoxButton;
                btn.ButtonClick += (_, __) => this.Execute();
                btn.Button.Command = this;
            }
        }

        public override string ToString() => $"CommandName: {this.CommandName}, Content: {this.Content}";

        protected virtual void OnPropChanged(object value, [CallerMemberName] string propertyName = null)
        {
            if (propertyName.IsNullOrEmpty())
                return;
            this.ForEach(c =>
            {
                var prop = typeof(UIElementController).GetProperty(propertyName);
                if (prop == null)
                    return;
                foreach (var controller in this.Controllers)
                    prop.SetValue(controller, value);
            });
            this.OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged.Raise(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddElement(params UIElement[] elements)
        {
            if (elements == null)
                throw new ArgumentNullException(nameof(elements));
            foreach (var element in elements)
                this.OnAddingElement(element);
        }

        public void AddElement(IEnumerable<UIElement> elements) { this.AddElement(elements.ToArray()); }

        protected virtual void OnAddingElement(UIElement element)
        {
            var controller = new UIElementController(element)
                             {
                                 IsEnabled = this.IsEnabled,
                                 Visibility = this.Visibility,
                                 Content = this.Content,
                                 PathStyle = this.PathStyle,
                                 PathFill = this.PathFill,
                                 Path = this.Path,
                                 ToolTip = this.ToolTip,
                                 IsCancel = this.IsCancel,
                                 IsDefault = this.IsDefault,
                                 IsChecked = this.IsChecked
                             };
            this.Controllers.Add(controller);
            this.HandleEvents(element);
        }

        public event EventHandler<EventArgs> Executed;
        public event EventHandler<ActingEventArgs> Executing;
        public event EventHandler<ItemActingEventArgs<Exception>> ExceptionOccurred;
        protected virtual void OnExecuted() { }
        public void Initialize(UIElement parent, params UIElement[] elements) { this.Initialize(parent, null, elements); }

        public void Initialize(UIElement parent, EventHandler<EventArgs> executed = null, params UIElement[] elements)
        {
            if (executed != null)
                this.Executed += executed;
            this.AddElement(elements);

            this._CommandBinding.Command = this;
            this._CommandBinding.Executed -= this.OnCommandBindingOnExecuted;
            this._CommandBinding.Executed += this.OnCommandBindingOnExecuted;
            parent.CommandBindings.Add(this._CommandBinding);

            var keyGesture = this.KeyGesture;
            if (keyGesture != null)
                parent.InputBindings.Add(new KeyBinding(this, keyGesture));

            this.Parent = parent;
        }

        private void OnCommandBindingOnExecuted(object _, ExecutedRoutedEventArgs __) { this.OnExecuted(); }

        protected virtual void OnIsCheckedChanged(ItemActedEventArgs<bool?> e) { this.IsCheckedChanged?.Invoke(this, e); }
        public IEnumerator<UIElement> GetEnumerator() { return this.Controllers.Select(c => c.Element).GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

        public object Content
        {
            get { return this._Content; }
            set
            {
                if (value == this._Content)
                    return;
                this._Content = value;
                this.OnPropChanged(value);
            }
        }

        public bool IsEnabled
        {
            get { return this._IsEnabled; }
            set
            {
                if (value.Equals(this._IsEnabled))
                    return;
                this._IsEnabled = value;
                this.OnPropChanged(value);
            }
        }

        public Visibility Visibility
        {
            get { return this._Visibility; }
            set
            {
                if (value.Equals(this._Visibility))
                    return;
                this._Visibility = value;
                this.OnPropChanged(value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}