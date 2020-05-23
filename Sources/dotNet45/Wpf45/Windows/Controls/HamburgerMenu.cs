using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mohammad.Wpf.Windows.Controls
{
    public class HamburgerMenu : ContentControl
    {
        public new static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content",
            typeof(List<HamburgerMenuItem>),
            typeof(HamburgerMenu),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen",
            typeof(bool),
            typeof(HamburgerMenu),
            new PropertyMetadata(true));

        public static readonly DependencyProperty MenuIconColorProperty = DependencyProperty.Register("MenuIconColor",
            typeof(Brush),
            typeof(HamburgerMenu),
            new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty MenuItemForegroundProperty = DependencyProperty.Register("MenuItemForeground",
            typeof(Brush),
            typeof(HamburgerMenu),
            new PropertyMetadata(Brushes.Transparent));

        public static readonly DependencyProperty SelectionIndicatorColorProperty = DependencyProperty.Register("SelectionIndicatorColor",
            typeof(Brush),
            typeof(HamburgerMenu),
            new PropertyMetadata(Brushes.Red));

        public new List<HamburgerMenuItem> Content
        {
            get => (List<HamburgerMenuItem>)this.GetValue(ContentProperty);
            set => this.SetValue(ContentProperty, value);
        }

        public bool IsOpen
        {
            get => (bool)this.GetValue(IsOpenProperty);
            set => this.SetValue(IsOpenProperty, value);
        }

        public Brush MenuIconColor
        {
            get => (Brush)this.GetValue(MenuIconColorProperty);
            set => this.SetValue(MenuIconColorProperty, value);
        }

        public Brush MenuItemForeground
        {
            get => (Brush)this.GetValue(MenuItemForegroundProperty);
            set => this.SetValue(MenuItemForegroundProperty, value);
        }

        public Brush SelectionIndicatorColor
        {
            get => (Brush)this.GetValue(SelectionIndicatorColorProperty);
            set => this.SetValue(SelectionIndicatorColorProperty, value);
        }

        static HamburgerMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenu), new FrameworkPropertyMetadata(typeof(HamburgerMenu)));
        }

        public override void BeginInit()
        {
            this.Content = new List<HamburgerMenuItem>();
            base.BeginInit();
        }
    }
}