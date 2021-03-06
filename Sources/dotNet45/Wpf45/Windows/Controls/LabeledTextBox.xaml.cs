﻿// Created on     2018/07/25
// Last update on 2018/09/05 by Mohammad Mir mostafa 

using System;
using System.ComponentModel;
using System.Windows;
using Mohammad.Helpers;
using Mohammad.Wpf.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for LabeledTextBox.xaml
    /// </summary>
    public partial class LabeledTextBox : IFlickable, IBindable
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(LabeledTextBox), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty HeaderWidthProperty =
            DependencyProperty.Register("HeaderWidth", typeof(double), typeof(LabeledTextBox), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(LabeledTextBox), new PropertyMetadata(default(string)));

        public LabeledTextBox()
        {
            this.InitializeComponent();
            this.HeaderWidth = 80;
        }

        [DefaultValue("")]
        [Localizability(LocalizationCategory.Title)]
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

        public double HeaderWidth
        {
            get => ((double)this.GetValue(HeaderWidthProperty)).NotNull((DefaultHeaderWidth ?? 80L).ToLong());
            set
            {
                this.SetValue(HeaderWidthProperty, value);
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the text contents of the text box.
        /// </summary>
        /// <returns>
        ///     A string containing the text contents of the text box. The default is an empty string ("").
        /// </returns>
        [Localizability(LocalizationCategory.Text)]
        [DefaultValue("")]
        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set
            {
                if (!this.Set(TextProperty, value))
                {
                    return;
                }

                this.OnTextChanged(EventArgs.Empty);
                this.OnPropertyChanged();
                if (this.AutoFlick)
                {
                    this.TextBlock.Flick();
                }
            }
        }

        public Style TextBlockStyle
        {
            get => this.TextBlock.TextBlockStyle;
            set
            {
                this.TextBlock.TextBlockStyle = value;
                this.OnPropertyChanged();
            }
        }

        public static double? DefaultHeaderWidth { get; set; }

        public bool AcceptsReturn
        {
            get => this.TextBox.AcceptsReturn;
            set
            {
                this.TextBox.AcceptsReturn = value;
                this.OnPropertyChanged();
            }
        }

        public bool AutoFlick { get; set; }

        public bool IsReadOnly
        {
            get => this.TextBox.IsReadOnly;
            set
            {
                this.TextBox.IsReadOnly = value;
                this.OnPropertyChanged();
            }
        }

        public DependencyProperty BindingFieldProperty => TextProperty;
        public FrameworkElement FlickerTextBlock => this.TextBlock.FlickerTextBlock;

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.AutoFlick && e.Property != null && e.Property.Name == "Text")
            {
                this.Flick();
            }

            base.OnPropertyChanged(e);
        }

        private void OnTextChanged(EventArgs e)
        {
            this.TextBox.Text = this.Text;
            this.TextChanged.Raise(this, e);
        }

        private void LabeledTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            this.TextBox.Focus();
        }

        public event EventHandler TextChanged;
    }
}