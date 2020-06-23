// Created on     2018/07/25
// Last update on 2018/09/05 by Mohammad Mir mostafa 

using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for HeaderedPanel.xaml
    /// </summary>
    [ContentProperty("Child")]
    public partial class HeaderedPanel
    {
        public static readonly DependencyProperty ChildProperty =
            DependencyProperty.Register("Child", typeof(object), typeof(HeaderedPanel), new UIPropertyMetadata(null));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(HeaderedPanel), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty IsChildEnabledProperty =
            DependencyProperty.Register("IsChildEnabled", typeof(bool), typeof(HeaderedPanel), new PropertyMetadata(default(bool)));

        //private static void ChildPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((HeaderedPanel)d).ChildPropertyChanged(e.NewValue);
        //}
        public static readonly DependencyProperty SepratorColorProperty =
            DependencyProperty.Register("SepratorColor", typeof(Brush), typeof(HeaderedPanel), new PropertyMetadata(default(Brush)));

        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public object Child
        {
            get => this.GetValue(ChildProperty);
            set
            {
                this.SetValue(ChildProperty, value);
                this.OnPropertyChanged();
            }
        }

        [Bindable(true)]
        public object Header
        {
            get => this.GetValue(HeaderProperty);
            set => this.SetValue(HeaderProperty, value);
        }

        [Bindable(true)]
        public bool IsChildEnabled
        {
            get => (bool)this.GetValue(IsChildEnabledProperty);
            set => this.SetValue(IsChildEnabledProperty, value);
        }

        public Brush SepratorColor
        {
            get => (Brush)this.GetValue(SepratorColorProperty);
            set => this.SetValue(SepratorColorProperty, value);
        }

        public HeaderedPanel()
        {
            this.IsChildEnabled = true;
            this.InitializeComponent();
            this.SepratorColor = Brushes.RoyalBlue;
        }
    }
}