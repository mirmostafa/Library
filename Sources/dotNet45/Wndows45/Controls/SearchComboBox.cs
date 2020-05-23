using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Mohammad.Helpers;

namespace Mohammad.Win.Controls
{
    [DefaultProperty("EmptyText")]
    public partial class SearchComboBox : ComboBox
    {
        private string _EmptyText;

        [DefaultValue(true)]
        public bool SaveMru { get; set; }

        [DefaultValue("Search")]
        public string EmptyText { get { return this._EmptyText.IfNullOrEmpty("Search"); } set { this._EmptyText = value; } }

        [DefaultValue(8)]
        public int MaxMruCount { get; set; }

        public SearchComboBox()
        {
            this.InitializeComponent();
            //this.DropDownStyle = ComboBoxStyle.Simple;
            this.SaveMru = true;
            this.MaxMruCount = 8;
            this.Text = this.EmptyText;
            this.Font = new Font(this.Font, FontStyle.Italic);
        }

        protected override void OnEnter(EventArgs e)
        {
            if (this.Text.Equals(this.EmptyText))
            {
                this.ResetFont();
                this.ResetText();
            }
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (this.SaveMru && !this.Text.IsNullOrEmpty())
            {
                while (this.Items.Count > this.MaxMruCount - 1)
                    this.Items.RemoveAt(0);
                this.Items.Add(this.Text);
            }
            if (this.Text.IsNullOrEmpty())
            {
                this.Text = this.EmptyText;
                this.Font = new Font(this.Font, FontStyle.Italic);
            }
        }
    }
}