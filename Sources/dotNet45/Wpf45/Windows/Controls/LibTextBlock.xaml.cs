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
        public static readonly DependencyProperty BlockStyleProperty = DependencyProperty.Register("BlockStyle",
            typeof(Style),
            typeof(LibTextBlock),
            new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty ShadowForegroundProperty = DependencyProperty.Register("ShadowForeground",
            typeof(Brush),
            typeof(LibTextBlock),
            new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string),
            typeof(LibTextBlock),
            new PropertyMetadata(default(string),
                (s, __) =>
                {
                    var me = s.As<LibTextBlock>();
                    me.TextChanged();
                }));

        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping",
            typeof(TextWrapping),
            typeof(LineBlock),
            new PropertyMetadata(default(TextWrapping)));

        private bool _AutoFlick;

        public LibTextBlock()
        {
            this.InitializeComponent();
            this.ShadowForeground = Brushes.Navy;
        }

        public Style BlockStyle
        {
            get => (Style)this.GetValue(BlockStyleProperty);
            set
            {
                if (!this.Set(BlockStyleProperty, value))
                {
                    return;
                }

                this.OnPropertyChanged();
            }
        }

        public Brush ShadowForeground
        {
            get => (Brush)this.GetValue(ShadowForegroundProperty);
            set => this.SetValue(ShadowForegroundProperty, value);
        }

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
            get => (string)this.GetValue(TextProperty);
            set
            {
                if (!this.Set(TextProperty, value))
                {
                    return;
                }

                this.OnPropertyChanged();
                if (this.AutoFlick)
                {
                    this.Flick();
                }
            }
        }

        public TextWrapping TextWrapping
        {
            get => (TextWrapping)this.GetValue(TextWrappingProperty);
            set
            {
                if (!this.Set(TextWrappingProperty, value))
                {
                    return;
                }

                this.OnPropertyChanged();
            }
        }

        public Style TextBlockStyle
        {
            get => this.TextBlock.Style;
            set => this.TextBlock.Style = value;
        }

        public bool AutoFlick
        {
            get => this._AutoFlick;
            set
            {
                if (value.Equals(this._AutoFlick))
                {
                    return;
                }

                this._AutoFlick = value;
                this.OnPropertyChanged();
            }
        }

        public DependencyProperty BindingFieldProperty => TextProperty;
        public FrameworkElement FlickerTextBlock => this.ShadowTextBlock;

        protected virtual void TextChanged()
        {
            if (this.AutoFlick)
            {
                this.Flick();
            }
        }
    }
}