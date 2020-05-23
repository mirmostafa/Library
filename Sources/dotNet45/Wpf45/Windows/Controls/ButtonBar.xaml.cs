using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Mohammad.Wpf.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for ButtonBar.xaml
    /// </summary>
    public partial class ButtonBar
    {
        public Style ButtonsStyle { get; set; }
        public List<Button> AppButtons { get; } = new List<Button>();
        public List<Button> PageButtons { get; } = new List<Button>();

        public IEnumerable<Button> Buttons
        {
            get
            {
                foreach (var button in this.AppButtons)
                    yield return button;
                foreach (var button in this.PageButtons)
                    yield return button;
            }
        }

        public ButtonBar() { this.InitializeComponent(); }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (ControlHelper.IsDesignTime())
                return;
            var appButtons = this.AppButtons;
            var pageButtons = this.PageButtons;
            this.AddButtons(appButtons, pageButtons);
        }

        internal void AddButtons(IEnumerable<Button> appButtons, IEnumerable<Button> pageButtons)
        {
            foreach (var button in appButtons)
            {
                if (this.ButtonsStyle != null)
                    button.Style = this.ButtonsStyle;
                this.AppButtonPanel.Children.Add(button);
            }

            foreach (var button in pageButtons)
            {
                if (this.ButtonsStyle != null)
                    button.Style = this.ButtonsStyle;
                this.PageButtonPanel.Children.Add(button);
            }
        }
    }
}