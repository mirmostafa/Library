// Created on     2018/07/25
// Last update on 2018/09/05 by Mohammad Mir mostafa 

using System;
using System.Windows;
using System.Windows.Media;
using Mohammad.Helpers;
using Mohammad.Wpf.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for CalloutTextBox.xaml
    /// </summary>
    public partial class CalloutTextBox : IFlickable, IBindable
    {
        public static readonly DependencyProperty DescriptionProperty = ControlHelper.GetDependencyProperty<string, CalloutTextBox>("Description",
            onPropertyChanged: (sender, value) =>
            {
                var me = sender.As<CalloutTextBox>();
                me.DescriptionBlock.Height = value.IsNullOrEmpty() ? 0 : double.NaN;
                me.OnPropertyChanged("Description");
            });

        public static readonly DependencyProperty HeaderProperty =
            ControlHelper.GetDependencyProperty<string, CalloutTextBox>("Header",
                onPropertyChanged: (sender, _) => sender.As<CalloutTextBox>().OnPropertyChanged("Header"));

        public static readonly DependencyProperty IsReaOnlyProperty = ControlHelper.GetDependencyProperty<bool, CalloutTextBox>("IsReadOnly",
            onPropertyChanged: (sender, e) =>
            {
                var me = sender.As<CalloutTextBox>();
                me.LabeledTextBox.IsReadOnly = me.IsReaOnly;
                me.OnPropertyChanged("IsReaOnlyProperty");
                me.OnIsReadOnlyChanged(sender);
            });

        public static readonly DependencyProperty IsValidProperty = ControlHelper.GetDependencyProperty<bool, CalloutTextBox>("IsValid",
            (sender, value) =>
            {
                var me = sender.As<CalloutTextBox>();
                me.LineControl.LineBrush = value ? Brushes.RoyalBlue : Brushes.Red;
            },
            (sender, propName) => sender.As<CalloutTextBox>().OnPropertyChanged("IsValid"),
            defaultValue: true);

        public static readonly DependencyProperty TextProperty = ControlHelper.GetDependencyProperty<string, CalloutTextBox>("Text",
            onPropertyChanged: (sender, _) =>
            {
                var me = sender.As<CalloutTextBox>();
                me.OnPropertyChanged("Text");
                me.OnTextChanged(sender);
            });

        public CalloutTextBox()
        {
            this.InitializeComponent();
        }

        public string Description
        {
            get => (string)this.GetValue(DescriptionProperty);
            set => this.SetValue(DescriptionProperty, value);
        }

        public string Header
        {
            get => (string)this.GetValue(HeaderProperty);
            set => this.SetValue(HeaderProperty, value);
        }

        public bool IsReaOnly
        {
            get => (bool)this.GetValue(IsReaOnlyProperty);
            set => this.SetValue(IsReaOnlyProperty, value);
        }

        public bool IsValid
        {
            get => (bool)this.GetValue(IsValidProperty);
            set => this.SetValue(IsValidProperty, value);
        }

        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
        }

        public DependencyProperty BindingFieldProperty => this.LabeledTextBox.BindingFieldProperty;
        public FrameworkElement FlickerTextBlock => this.LabeledTextBox;

        protected virtual void OnTextChanged(object sender)
        {
            this.TextChanged?.Invoke(sender, EventArgs.Empty);
        }

        protected virtual void OnIsReadOnlyChanged(object sender)
        {
            this.IsReadOnlyChanged?.Invoke(sender, EventArgs.Empty);
        }

        private void CalloutTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            this.LabeledTextBox.Focus();
        }

        public event EventHandler TextChanged;
        public event EventHandler IsReadOnlyChanged;
    }
}