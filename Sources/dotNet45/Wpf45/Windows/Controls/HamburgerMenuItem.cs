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

        public ImageSource Icon { get { return (ImageSource) this.GetValue(IconProperty); } set { this.SetValue(IconProperty, value); } }

        public static readonly DependencyProperty SelectionIndicatorColorProperty = DependencyProperty.Register("SelectionIndicatorColor",
            typeof(Brush),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(Brushes.Blue));

        public Brush SelectionIndicatorColor
        {
            get { return (Brush) this.GetValue(SelectionIndicatorColorProperty); }
            set { this.SetValue(SelectionIndicatorColorProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(string.Empty));

        public string Text { get { return (string) this.GetValue(TextProperty); } set { this.SetValue(TextProperty, value); } }

        static HamburgerMenuItem() { DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenuItem), new FrameworkPropertyMetadata(typeof(HamburgerMenuItem))); }

        public ILibCommand Command { get { return (ILibCommand) this.GetValue(CommandProperty); } set { this.SetValue(CommandProperty, value); } }

        public object CommandParameter { get { return this.GetValue(CommandParameterProperty); } set { this.SetValue(CommandParameterProperty, value); } }

        /// <summary>Gets or sets the element on which to raise the specified command.  </summary>
        /// <returns>Element on which to raise a command.</returns>
        [Bindable(true)]
        [Category("Action")]
        public ILibInputElement CommandTarget
        {
            get { return (ILibInputElement) this.GetValue(CommandTargetProperty); }
            set { this.SetValue(CommandTargetProperty, value); }
        }
    }
}