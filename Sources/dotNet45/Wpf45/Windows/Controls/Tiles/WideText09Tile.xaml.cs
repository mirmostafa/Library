﻿using System.Windows;

namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for WideText01Tile.xaml
    /// </summary>
    public partial class WideText09Tile
    {
        public static readonly DependencyProperty BodyProperty = DependencyProperty.Register("Body",
            typeof(string),
            typeof(WideText09Tile),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header",
            typeof(string),
            typeof(WideText09Tile),
            new PropertyMetadata(default(string)));

        public WideText09Tile()
        {
            this.InitializeComponent();
        }

        public string Body
        {
            get => (string)this.GetValue(BodyProperty);
            set
            {
                if (!this.Set(BodyProperty, value))
                {
                    return;
                }

                this.OnPropertyChanged();
            }
        }

        public string Header
        {
            get => (string)this.GetValue(HeaderProperty);
            set
            {
                if (!this.Set(HeaderProperty, value))
                {
                    return;
                }

                this.OnPropertyChanged();
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

            var header = libCommand.Header;
            if (header != null)
            {
                this.Header = header.StartsWith("_") ? header.Substring(1) : header;
            }
            else if (libCommand.Content is string)
            {
                var s = this.Command.Content.ToString();
                this.Header = s.StartsWith("_") ? s.Substring(1) : s;
            }

            this.Body = libCommand.Body;
        }
    }
}