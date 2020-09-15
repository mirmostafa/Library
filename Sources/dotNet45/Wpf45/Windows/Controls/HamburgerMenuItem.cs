using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Mohammad.Wpf.Helpers;
using Mohammad.Wpf.Windows.Input;
using Mohammad.Wpf.Windows.Input.LibCommands;

namespace Mohammad.Wpf.Windows.Controls
{
    public class HamburgerMenuItem : ListBoxItem, ILibCommandSource
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
            typeof(ILibCommand),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter",
            typeof(object),
            typeof(HamburgerMenuItem));

        public static readonly DependencyProperty CommandTargetProperty = ControlHelper.GetDependencyProperty<ILibInputElement, HamburgerMenuItem>("CommandTarget");

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon",
            typeof(ImageSource),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SelectionIndicatorColorProperty = DependencyProperty.Register("SelectionIndicatorColor",
            typeof(Brush),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(Brushes.Blue));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(string.Empty));

        static HamburgerMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenuItem), new FrameworkPropertyMetadata(typeof(HamburgerMenuItem)));
        }

        public ImageSource Icon
        {
            get => (ImageSource)this.GetValue(IconProperty);
            set => this.SetValue(IconProperty, value);
        }

        public Brush SelectionIndicatorColor
        {
            get => (Brush)this.GetValue(SelectionIndicatorColorProperty);
            set => this.SetValue(SelectionIndicatorColorProperty, value);
        }

        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
        }

        public ILibCommand Command
        {
            get => (ILibCommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => this.GetValue(CommandParameterProperty);
            set => this.SetValue(CommandParameterProperty, value);
        }

        /// <summary>Gets or sets the element on which to raise the specified command.  </summary>
        /// <returns>Element on which to raise a command.</returns>
        [Bindable(true)]
        [Category("Action")]
        public ILibInputElement CommandTarget
        {
            get => (ILibInputElement)this.GetValue(CommandTargetProperty);
            set => this.SetValue(CommandTargetProperty, value);
        }
    }
}