﻿using System.Windows;
using System.Windows.Controls;
using Mohammad.Helpers;
using Mohammad.Wpf.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for SeparatorBlock.xaml
    /// </summary>
    public partial class SeparatorBlock
    {
        public static readonly DependencyProperty HeaderProperty = ControlHelper.GetDependencyProperty<string, SeparatorBlock>("Header",
            (s, e) =>
            {
                s.As<SeparatorBlock>().SeparatorLabel.Header.As<TextBlock>().Text = e;
            });

        public SeparatorBlock()
        {
            this.InitializeComponent();
            this.SeparatorLabel.Header.As<TextBlock>().Foreground = this.SeparatorLabel.SepratorColor;
        }

        public string Header
        {
            get => (string)this.GetValue(HeaderProperty);
            set => this.SetValue(HeaderProperty, value);
        }
    }
}