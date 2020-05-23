using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Mohammad.EventsArgs;
using Mohammad.Helpers;

namespace Mohammad.Win.Controls
{
    [DefaultProperty("WatermarkText")]
    [DefaultEvent("LookingForFirstMatch")]
    public partial class SearchBox : UserControl
    {
        [DefaultValue("Search")]
        public string WatermarkText { get; set; } = "Serach";

        public SearchBox()
        {
            this.InitializeComponent();
            this.SetWatermark();
        }

        public event EventHandler<ItemActingEventArgs<string>> LookingForFirstMatch;
        public event EventHandler<ItemActingEventArgs<string>> LookingForNextMatch;
        public event EventHandler<ItemActingEventArgs<string>> LookingForPrevMatch;
        public event EventHandler<ItemActingEventArgs<string>> LookingForLastMatch;

        private void ParentTreeSerahBox_Enter(object sender, EventArgs e)
        {
            this.SearchTextBox.ResetFont();
            this.SearchTextBox.ResetForeColor();
            if (this.SearchTextBox.Text == this.WatermarkText)
                this.SearchTextBox.ResetText();
        }

        protected virtual void OnLookingForLastMatch(ItemActingEventArgs<string> e) { this.LookingForLastMatch.Raise(this, e); }
        protected virtual void OnLookingForPrevMatch(ItemActingEventArgs<string> e) { this.LookingForPrevMatch.Raise(this, e); }
        protected virtual void OnLookingForNextMatch(ItemActingEventArgs<string> e) { this.LookingForNextMatch.Raise(this, e); }
        protected virtual void OnLookingForFirstMatch(ItemActingEventArgs<string> e) { this.LookingForFirstMatch.Raise(this, e); }
        private void ParentTreeSerahBox_Leave(object sender, EventArgs e) { this.SetWatermark(); }

        private void SetWatermark()
        {
            if (!string.IsNullOrEmpty(this.SearchTextBox.Text))
                return;
            this.SearchTextBox.Font = new Font(DefaultFont, FontStyle.Italic);
            this.SearchTextBox.ForeColor = Color.Gray;
            this.SearchTextBox.Text = this.WatermarkText;
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Shift)
                this.OnLookingForPrevMatch(new ItemActingEventArgs<string>(this.SearchTextBox.Text));
            else if (e.KeyCode == Keys.Enter)
                this.OnLookingForNextMatch(new ItemActingEventArgs<string>(this.SearchTextBox.Text));
            else
                this.OnLookingForFirstMatch(new ItemActingEventArgs<string>(this.SearchTextBox.Text));
        }
    }
}