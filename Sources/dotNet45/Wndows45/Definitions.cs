using System;
using System.Windows.Forms;

namespace Mohammad.Win
{
    /// <summary>
    ///     The size kind of each block in ProgressDisk
    /// </summary>
    public enum ProgressDiskBlockSize
    {
        /// <summary>
        ///     Smaller
        /// </summary>
        XSmall,

        /// <summary>
        ///     Small
        /// </summary>
        Small,

        /// <summary>
        ///     Normal
        /// </summary>
        Medium,

        /// <summary>
        ///     Large
        /// </summary>
        Large,

        /// <summary>
        ///     Larger
        /// </summary>
        XLarge,

        /// <summary>
        ///     Largest
        /// </summary>
        XXLarge
    }

    public enum DataChartType
    {
        Stick,
        Line
    }

    public enum NotifyType
    {
        Information,
        Warning,
        Error
    }

    public class NavigatingEventArgs : EventArgs
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public NavigatingEventArgs(int pageRowCount, int currentPageIndex)
        {
            this.PageRowCount = pageRowCount;
            this.CurrentPageIndex = currentPageIndex;
        }

        /// <summary>
        ///     Count of rows in each page. -1: all rows (no paging)
        /// </summary>
        public int PageRowCount { get; }

        /// <summary>
        ///     Property description
        /// </summary>
        public int CurrentPageIndex { get; }
    }

    public class ReportGeneratingEventArgs : EventArgs
    {
        /// <summary>
        ///     Property description
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        ///     Constructor
        ///     <summary>
        internal ReportGeneratingEventArgs()
        {
        }
    }

    internal enum Direction
    {
        Forward,
        Reverse
    }

    public class FieldChangedEventArgs : EventArgs
    {
        public int FieldIndex { get; set; }
        public string Text { get; set; }
    }

    internal class CedeFocusEventArgs : EventArgs
    {
        public Direction Direction { get; set; }
        public int FieldIndex { get; set; }
        public Selection Selection { get; set; }
    }

    internal enum Selection
    {
        None,
        All
    }

    internal class TextChangedEventArgs : EventArgs
    {
        public int FieldIndex { get; set; }
        public string Text { get; set; }
    }

    internal class SpecialKeyEventArgs : EventArgs
    {
        public int FieldIndex { get; set; }
        public Keys KeyCode { get; set; }
    }
}