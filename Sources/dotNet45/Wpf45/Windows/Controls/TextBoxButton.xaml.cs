using System;
using System.ComponentModel;
using System.Windows;
using Mohammad.Helpers;
using Mohammad.Wpf.Helpers;
using Mohammad.Wpf.Windows.Input;
using Mohammad.Wpf.Windows.Input.LibCommands;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for TextBoxButton.xaml
    /// </summary>
    public partial class TextBoxButton : ILibCommandSource
    {
        public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register("ButtonContent",
            typeof(object),
            typeof(TextBoxButton),
            new PropertyMetadata(default(object)));

        public object ButtonContent { get { return this.GetValue(ButtonContentProperty); } set { this.SetValue(ButtonContentProperty, value); } }

        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register("ButtonStyle",
            typeof(Style),
            typeof(TextBoxButton),
            new PropertyMetadata(default(Style)));

        public Style ButtonStyle { get { return (Style) this.GetValue(ButtonStyleProperty); } set { this.SetValue(ButtonStyleProperty, value); } }

        public static readonly DependencyProperty CommandTargetProperty = ControlHelper.GetDependencyProperty<ILibInputElement, TextBoxButton>("CommandTarget",
            defaultValue: new PropertyMetadata(default(ILibInputElement)));

        public static readonly DependencyProperty IsTextBoxReadOnlyProperty = DependencyProperty.Register("IsTextBoxReadOnly",
            typeof(bool),
            typeof(TextBoxButton),
            new PropertyMetadata(default(bool)));

        public bool IsTextBoxReadOnly { get { return (bool) this.GetValue(IsTextBoxReadOnlyProperty); } set { this.SetValue(IsTextBoxReadOnlyProperty, value); } }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string),
            typeof(TextBoxButton),
            new PropertyMetadata(default(string)));

        public string Text { get { return (string) this.GetValue(TextProperty); } set { this.SetValue(TextProperty, value); } }

        public TextBoxButton()
        {
            this.InitializeComponent();
            this.ButtonContent = "…";
            //this.ButtonStyle = (Style)this.FindResource("IconButton");
        }

        private void Button_OnClick(object sender, RoutedEventArgs e) { this.OnButtonClick(this, e); }
        private void OnButtonClick(object sender, RoutedEventArgs e) { this.ButtonClick.Raise(sender, e); }
        public event EventHandler<RoutedEventArgs> ButtonClick;

        public ILibCommand Command { get { return this.Button.Command.As<LibCommand>(); } set { this.Button.Command = value; } }

        /// <summary>Gets or sets the element on which to raise the specified command.  </summary>
        /// <returns>Element on which to raise a command.</returns>
        [Bindable(true)]
        [Category("Action")]
        public ILibInputElement CommandTarget
        {
            get { return (ILibInputElement) this.GetValue(CommandTargetProperty); }
            set { this.SetValue(CommandTargetProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the parameter to pass to the
        ///     <see cref="P:System.Windows.Controls.Primitives.ButtonBase.Command" /> property.
        /// </summary>
        /// <returns>Parameter to pass to the <see cref="P:System.Windows.Controls.Primitives.ButtonBase.Command" /> property.</returns>
        [Bindable(true)]
        [Category("Action")]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public object CommandParameter { get { return this.Button.CommandParameter; } set { this.Button.CommandParameter = value; } }
    }
}