// Created on     2018/07/25
// Last update on 2018/08/12 by Mohammad Mir mostafa 

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Mohammad.Helpers;
using Mohammad.Wpf.Windows.Input.LibCommands;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for NavigationBar.xaml
    /// </summary>
    public partial class NavigationBar
    {
        public static readonly DependencyProperty FrameProperty = DependencyProperty.Register("Frame",
            typeof(LibFrame),
            typeof(NavigationBar),
            new PropertyMetadata(default(Frame),
                (sender, _) =>
                {
                    var me = sender.As<NavigationBar>();
                    foreach (var cmd in me.NavigationCommands)
                    {
                        cmd.Frame = me.Frame;
                    }
                }));

        public static readonly DependencyProperty ToggleButtonsStyleProperty =
            DependencyProperty.Register("ToggleButtonsStyle", typeof(Style), typeof(NavigationBar), new PropertyMetadata(null));

        private bool _IsLoaded;

        public NavigationBar()
        {
            this.InitializeComponent();
        }

        public LibFrame Frame
        {
            get => (LibFrame)this.GetValue(FrameProperty);
            set => this.SetValue(FrameProperty, value);
        }

        public Style ToggleButtonsStyle
        {
            get => (Style)this.GetValue(ToggleButtonsStyleProperty);
            set => this.SetValue(ToggleButtonsStyleProperty, value);
        }

        public List<NavigationCommand> NavigationCommands { get; } = new List<NavigationCommand>();

        public override void OnApplyTemplate()
        {
            this.Init();
            base.OnApplyTemplate();
        }

        private void Init()
        {
            this.PageButtonPanel.Children.Clear();
            foreach (var cmd in this.NavigationCommands)
            {
                cmd.Executed += (sender, _) =>
                {
                    this.NavigationCommands.Except(sender.As<LibCommand>()).ForEach(c => c.IsChecked = false);
                    sender.As<LibCommand>().IsChecked = true;
                };
                var btn = new ToggleButton();
                LibCommand.SetMyCommand(btn, cmd);
                if (this.ToggleButtonsStyle != null)
                {
                    btn.Style = this.ToggleButtonsStyle;
                }

                this.PageButtonPanel.Children.Add(btn);
            }
        }

        private void NavigationBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this._IsLoaded)
            {
                return;
            }

            this.NavigationCommands.FirstOrDefault(cmd => cmd.IsDefault)?.Execute();
            this._IsLoaded = true;
        }
    }
}