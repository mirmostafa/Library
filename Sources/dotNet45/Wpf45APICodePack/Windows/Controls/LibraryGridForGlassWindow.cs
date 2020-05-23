using System;
using System.Windows;
using System.Windows.Controls;
using Mohammad.Helpers;

namespace Mohammad.Wpf.Windows.Controls
{
    public class LibraryGridForGlassWindow : Grid
    {
        public static readonly string Row1SharedSizeGroup = "Row1SharedSizeGroup";
        public static readonly string Row2SharedSizeGroup = "Row2SharedSizeGroup";
        public static readonly string Row3SharedSizeGroup = "Row3SharedSizeGroup";
        public static readonly string Row4SharedSizeGroup = "Row4SharedSizeGroup";
        public static readonly string Row5SharedSizeGroup = "Row5SharedSizeGroup";
        public ProgressBar ModernProgressBar { get; private set; }

        public LibraryGridForGlassWindow()
        {
            var row0 = new RowDefinition {Height = new GridLength(1, GridUnitType.Auto), SharedSizeGroup = Row1SharedSizeGroup, ToolTip = "ProgressBar"};
            var row1 = new RowDefinition {Height = new GridLength(1, GridUnitType.Auto), SharedSizeGroup = Row2SharedSizeGroup, ToolTip = "PageHeader"};
            var row2 = new RowDefinition {Height = new GridLength(1, GridUnitType.Star), SharedSizeGroup = Row3SharedSizeGroup, ToolTip = "ClientGrid"};
            var row3 = new RowDefinition {Height = new GridLength(1, GridUnitType.Auto), SharedSizeGroup = Row4SharedSizeGroup, ToolTip = "ButtonBar"};
            var row4 = new RowDefinition {Height = new GridLength(1, GridUnitType.Auto), SharedSizeGroup = Row5SharedSizeGroup, ToolTip = "StatusBar"};
            this.RowDefinitions.Add(row0);
            this.RowDefinitions.Add(row1);
            this.RowDefinitions.Add(row2);
            this.RowDefinitions.Add(row3);
            this.RowDefinitions.Add(row4);
            this.Style = this.FindResource("LayoutRoot").As<Style>();
        }

        protected override void OnInitialized(EventArgs e)
        {
            SetRow(this.ModernProgressBar = new ProgressBar {Style = this.FindResource("ModernProgressBar").As<Style>()}, 0);
            base.OnInitialized(e);
        }
    }
}