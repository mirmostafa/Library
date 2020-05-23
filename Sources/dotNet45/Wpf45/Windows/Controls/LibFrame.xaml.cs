using System.Windows;
using System.Windows.Navigation;
using Mohammad.Helpers;
using Mohammad.Wpf.Interfaces;
using Mohammad.Wpf.Windows.Media;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for LibFrame.xaml
    /// </summary>
    public partial class LibFrame : IWindowHosted
    {
        public static bool UseAnimationByDefault = true;

        public static readonly DependencyProperty UseAnimationProperty = DependencyProperty.Register("UseAnimation",
            typeof(bool),
            typeof(LibFrame),
            new PropertyMetadata(default(bool)));

        public bool UseAnimation { get { return (bool) this.GetValue(UseAnimationProperty); } set { this.SetValue(UseAnimationProperty, value); } }

        public static readonly DependencyProperty WindowProperty = DependencyProperty.Register("Window",
            typeof(Window),
            typeof(LibFrame),
            new PropertyMetadata(default(Window)));

        public LibFrame()
        {
            this.UseAnimation = UseAnimationByDefault;
            this.InitializeComponent();
        }

        private void LibFrame_OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (this.Window != null && e.Content is IWindowHosted)
                e.Content.As<IWindowHosted>().Window = this.Window;
            if (!this.UseAnimation)
                return;
            CodeHelper.Catch(() => Animations.MoveIn(this));
        }

        public Window Window { get { return (Window) this.GetValue(WindowProperty); } set { this.SetValue(WindowProperty, value); } }
    }
}