using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Mohammad.Helpers;
using Mohammad.Wpf.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for LibTextBlock.xaml
    /// </summary>
    public partial class LibTextBlock : IFlickable, IBindable
    {
        private bool _AutoFlick;

        public static readonly DependencyProperty BlockStyleProperty = DependencyProperty.Register("BlockStyle",
            typeof(Style),
            typeof(LibTextBlock),
            new PropertyMetadata(default(Style)));

        public Style BlockStyle
        {
            get { return (Style) this.GetValue(BlockStyleProperty); }
            set
            {
                if (!this.Set(BlockStyleProperty, value))
                    return;
                this.OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty ShadowForegroundProperty = DependencyProperty.Register("ShadowForeground",
            typeof(Brush),
            typeof(LibTextBlock),
            new PropertyMetadata(default(Brush)));

        public Brush ShadowForeground { get { return (Brush) this.GetValue(ShadowForegroundProperty); } set { this.SetValue(ShadowForegroundProperty, value); } }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string),
            typeof(LibTextBlock),
            new PropertyMetadata(default(string),
                (s, __) =>
                {
                    var me = s.As<LibTextBlock>();
                    me.TextChanged();
                }));

        /// <summary>
        ///     Gets or sets the text contents of the text box.
        /// </summary>
        /// <returns>
        ///     A string containing the text contents of the text box. The default is an empty string ("").
        /// </returns>
        [Localizability(LocalizationCategory.Text)]
        [DefaultValue("")]
        [Bindable(true)]
        public string Text
        {
            get { return (string) this.GetValue(TextProperty); }
            set
            {
                if (!this.Set(TextProperty, value))
                    return;
                this.OnPropertyChanged();
                if (this.AutoFlick)
                    this.Flick();
            }
        }

        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping",
            typeof(TextWrapping),
            typeof(LineBlock),
            new PropertyMetadata(default(TextWrapping)));

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping) this.GetValue(TextWrappingProperty); }
            set
            {
                if (!this.Set(TextWrappingProperty, value))
                    return;
                this.OnPropertyChanged();
            }
        }

        public Style TextBlockStyle { get { return this.TextBlock.Style; } set { this.TextBlock.Style = value; } }

        public bool AutoFlick
        {
            get { return this._AutoFlick; }
            set
            {
                if (value.Equals(this._AutoFlick))
                    return;
                this._AutoFlick = value;
                this.OnPropertyChanged();
            }
        }

        public LibTextBlock()
        {
            this.InitializeComponent();
            this.ShadowForeground = Brushes.Navy;
        }

        protected virtual void TextChanged()
        {
            if (this.AutoFlick)
                this.Flick();
        }

        public DependencyProperty BindingFieldProperty { get { return TextProperty; } }
        public FrameworkElement FlickerTextBlock { get { return this.ShadowTextBlock; } }
    }
}