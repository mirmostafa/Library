using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Mohammad.Win.Internals;

namespace Mohammad.Win.Controls
{
    [ToolboxItem(typeof(TabStripToolboxItem))]
    public class TabStrip : ToolStrip
    {
        private Font boldFont = new Font(SystemFonts.MenuFont, FontStyle.Bold);
        private Tab currentSelection;
        private int tabOverlap;
        private const int EXTRA_PADDING = 0;

        protected override Padding DefaultPadding
        {
            get
            {
                var padding = base.DefaultPadding;
                padding.Top += EXTRA_PADDING;
                padding.Bottom += EXTRA_PADDING;

                return padding;
            }
        }

        public Tab SelectedTab
        {
            get { return this.currentSelection; }
            set
            {
                if (this.currentSelection != value)
                {
                    this.currentSelection = value;

                    if (this.currentSelection != null)
                    {
                        this.PerformLayout();
                        if (this.currentSelection.TabStripPage != null)
                            this.currentSelection.TabStripPage.Activate();
                    }
                }
            }
        }

        protected override Size DefaultSize
        {
            get
            {
                var size = base.DefaultSize;
                // size.Height += EXTRA_PADDING*2;
                return size;
            }
        }

        [DefaultValue(10)]
        public int TabOverlap
        {
            get { return this.tabOverlap; }
            set
            {
                if (this.tabOverlap != value)
                {
                    this.tabOverlap = value;
                    // call perform layout so we 
                    this.PerformLayout();
                }
            }
        }

        public TabStrip()
        {
            this.Renderer = new TabStripProfessionalRenderer();
            this.Padding = new Padding(60, 3, 30, 0);
            this.AutoSize = false;
            this.Size = new Size(this.Width, 26);
            this.BackColor = Color.FromArgb(191, 219, 255);
            this.GripStyle = ToolStripGripStyle.Hidden;

            this.ShowItemToolTips = false;
        }

        protected override ToolStripItem CreateDefaultItem(string text, Image image, EventHandler onClick) { return new Tab(text, image, onClick); }

        protected override void OnItemClicked(ToolStripItemClickedEventArgs e)
        {
            for (var i = 0; i < this.Items.Count; i++)
            {
                var currentTab = this.Items[i] as Tab;
                this.SuspendLayout();
                if (currentTab != null)
                    if (currentTab != e.ClickedItem)
                    {
                        currentTab.Checked = false;
                        currentTab.Font = this.Font;
                        currentTab.b_active = false;
                    }
                    else
                        // currentTab.Font = boldFont;
                    {
                        currentTab.b_active = true;
                    }
                this.ResumeLayout();
            }
            this.SelectedTab = e.ClickedItem as Tab;

            base.OnItemClicked(e);
        }

        protected override void SetDisplayedItems()
        {
            base.SetDisplayedItems();
            for (var i = 0; i < this.DisplayedItems.Count; i++)
                if (this.DisplayedItems[i] == this.SelectedTab)
                {
                    this.DisplayedItems.Add(this.SelectedTab);
                    break;
                }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            var preferredSize = Size.Empty;
            proposedSize -= this.Padding.Size;

            foreach (ToolStripItem item in this.Items)
                preferredSize = LayoutUtils.UnionSizes(preferredSize, item.GetPreferredSize(proposedSize) + item.Padding.Size);
            return preferredSize + this.Padding.Size;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
    }
}