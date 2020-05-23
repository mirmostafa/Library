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

        public new List<HamburgerMenuItem> Content
        {
            get { return (List<HamburgerMenuItem>) this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen",
            typeof(bool),
            typeof(HamburgerMenu),
            new PropertyMetadata(true));

        public bool IsOpen { get { return (bool) this.GetValue(IsOpenProperty); } set { this.SetValue(IsOpenProperty, value); } }

        public static readonly DependencyProperty MenuIconColorProperty = DependencyProperty.Register("MenuIconColor",
            typeof(Brush),
            typeof(HamburgerMenu),
            new PropertyMetadata(Brushes.White));

        public Brush MenuIconColor { get { return (Brush) this.GetValue(MenuIconColorProperty); } set { this.SetValue(MenuIconColorProperty, value); } }

        public static readonly DependencyProperty MenuItemForegroundProperty = DependencyProperty.Register("MenuItemForeground",
            typeof(Brush),
            typeof(HamburgerMenu),
            new PropertyMetadata(Brushes.Transparent));

        public Brush MenuItemForeground
        {
            get { return (Brush) this.GetValue(MenuItemForegroundProperty); }
            set { this.SetValue(MenuItemForegroundProperty, value); }
        }

        public static readonly DependencyProperty SelectionIndicatorColorProperty = DependencyProperty.Register("SelectionIndicatorColor",
            typeof(Brush),
            typeof(HamburgerMenu),
            new PropertyMetadata(Brushes.Red));

        public Brush SelectionIndicatorColor
        {
            get { return (Brush) this.GetValue(SelectionIndicatorColorProperty); }
            set { this.SetValue(SelectionIndicatorColorProperty, value); }
        }

        static HamburgerMenu() { DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenu), new FrameworkPropertyMetadata(typeof(HamburgerMenu))); }

        public override void BeginInit()
        {
            this.Content = new List<HamburgerMenuItem>();
            base.BeginInit();
        }
    }
}