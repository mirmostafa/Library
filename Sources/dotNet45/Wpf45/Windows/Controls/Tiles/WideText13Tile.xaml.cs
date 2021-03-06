﻿using System.Windows;

namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for WideText01Tile.xaml
    /// </summary>
    public partial class WideText13Tile
    {
        public static readonly DependencyProperty BodyProperty = DependencyProperty.Register("Body",
            typeof(string),
            typeof(WideText13Tile),
            new PropertyMetadata(default(string)));

        public WideText13Tile()
        {
            this.InitializeComponent();
        }

        public string Body
        {
            get => (string)this.GetValue(BodyProperty);
            set
            {
                this.SetValue(BodyProperty, value);
                this.textBlock2.Text = value;
            }
        }

        public string Badge
        {
            get => this.textBlockBadge.Text;
            set => this.textBlockBadge.Text = value;
        }

        protected override void HookCommand()
        {
            var libCommand = this.Command;
            if (libCommand == null)
            {
                return;
            }

            this.Body = libCommand.Body ?? (libCommand.Content ?? string.Empty).ToString();
        }
    }
}