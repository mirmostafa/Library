﻿using System.Collections.Generic;
using System.Windows;
using Mohammad.Wpf.Helpers;
using Mohammad.Wpf.Interfaces;
using Mohammad.Wpf.Windows.Input.LibCommands;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for NavigationTabControl.xaml
    /// </summary>
    public partial class NavigationTabControl : IWindowHosted
    {
        public static bool UseAnimationByDefault = true;

        public static readonly DependencyProperty ToggleButtonsStyleProperty = ControlHelper.GetDependencyProperty<Style, NavigationTabControl>("ToggleButtonsStyle");

        public static readonly DependencyProperty UseAnimationProperty = ControlHelper.GetDependencyProperty<bool, NavigationTabControl>("UseAnimation");

        //public static readonly DependencyProperty WindowProperty = DependencyProperty.Register("Window", typeof(Window), typeof(NavigationTabControl), new PropertyMetadata(default(Window)));
        public static readonly DependencyProperty WindowProperty = ControlHelper.GetDependencyProperty<Window, NavigationTabControl>("Window",
            (me, value) => me.NavigationCommands.ForEach(nc => nc.Window = value));

        public NavigationTabControl()
        {
            this.InitializeComponent();
            this.Frame.UseAnimation = UseAnimationByDefault;
            this.NavigationCommands.ForEach(nc => nc.Window = this.Window);
        }

        public Style ToggleButtonsStyle
        {
            get => (Style)this.GetValue(ToggleButtonsStyleProperty);
            set => this.SetValue(ToggleButtonsStyleProperty, value);
        }

        public bool UseAnimation
        {
            get => (bool)this.GetValue(UseAnimationProperty);
            set => this.SetValue(UseAnimationProperty, value);
        }

        public List<NavigationCommand> NavigationCommands => this.NavigationBar.NavigationCommands;

        public Window Window
        {
            get => (Window)this.GetValue(WindowProperty);
            set => this.SetValue(WindowProperty, value);
        }

        private void NavigationTabControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.NavigationCommands.ForEach(nc => nc.Window = this.Window);
        }

        //private void NavigationTabControl_OnLayoutUpdated(object sender, EventArgs e) { this.NavigationCommands.FirstOrDefault(nc => nc.IsDefault)?.Execute(); }
    }
}