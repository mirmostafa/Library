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
        #region Methods

        #region Constructor

        /// <summary>
        ///     Constructor
        /// </summary>
        public NavigatingEventArgs(int pageRowCount, int currentPageIndex)
        {
            this.PageRowCount = pageRowCount;
            this.CurrentPageIndex = currentPageIndex;
        }

        #endregion

        #endregion

        #region Properties

        #region PageRowCount

        /// <summary>
        ///     Count of rows in each page. -1: all rows (no paging)
        /// </summary>
        public int PageRowCount { get; private set; }

        #endregion

        #region CurrentPageIndex

        /// <summary>
        ///     Property description
        /// </summary>
        public int CurrentPageIndex { get; private set; }

        #endregion

        #endregion
    }

    public class ReportGeneratingEventArgs : EventArgs
    {
        #region Handled

        /// <summary>
        ///     Property description
        /// </summary>
        public bool Cancel { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        ///     Constructor
        ///     <summary>
        internal ReportGeneratingEventArgs() { }

        #endregion
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