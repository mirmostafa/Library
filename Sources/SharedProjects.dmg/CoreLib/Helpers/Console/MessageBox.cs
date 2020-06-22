using System;
using Mohammad.Globalization;

namespace Mohammad.Helpers.Console
{
    public static class MessageBox
    {
        private const ConsoleColor DefaultConstConsoleColor = ConsoleHelper.DEFAULT_CONSTANT_CONSOLE_COLOR;
        private static int _Width;
        public static bool InBox { get; private set; }
        public static char SeparatorChar { get; set; } = '*';
        public static int? DefaultWidth { get; set; }
        public static void Open() { Open(DefaultWidth ?? 1); }
        public static void Open(int width) { Open(string.Empty, width); }
        public static void Open(string title) { Open(title, 0); }

        public static void Open(string title, int width)
        {
            _Width = width;
            InBox = true;
            string.Concat("*", title).Write();
            InsertSeparatorLine(title);
        }

        public static void Close()
        {
            InBox = false;
            InsertSeparatorLine();
        }

        public static void Show(object obj, object title)
        {
            Show(obj, title, obj.ToString().Split(new[] {Environment.NewLine}, StringSplitOptions.None).GetLongest().Length + 4);
        }

        public static void Show(object obj, object title, int width)
        {
            Open(width);
            string.Concat("*", title).Write();
            InsertSeparatorLine(title);
            Show(obj);
            Close();
        }

        public static void Show(object obj, ConsoleColor foregroundColor = DefaultConstConsoleColor)
        {
            if (obj == null)
                return;
            var width = DefaultWidth ?? (InBox ? _Width : obj.ToString().Length + 4);
            if (!InBox)
                InsertSeparatorLine(width);
            var colorBuffer = System.Console.ForegroundColor;
            if (foregroundColor != DefaultConstConsoleColor)
                System.Console.ForegroundColor = foregroundColor;
            foreach (var str in obj.ToString().Split(new[] {Environment.NewLine}, StringSplitOptions.None))
                string.Format("{0} {1}{0}", SeparatorChar, str.AddSpace(width - str.Length - 4)).WriteLine();
            System.Console.ForegroundColor = colorBuffer;
            if (!InBox)
                InsertSeparatorLine(width);
        }

        public static void InsertSeparatorLine(object text = null) { InsertSeparatorLine(_Width, text); }

        public static void InsertSeparatorLine(int width, object text = null)
        {
            if (text != null)
                SeparatorChar.ToString().Repeat(width - text.ToString().Length - 1).WriteLine();
            else
                SeparatorChar.ToString().Repeat(width).WriteLine();
        }

        public static void WriteLineTimeStamp(this string text, ConsoleColor foregroundColor = DefaultConstConsoleColor)
        {
            //string.Format("  [{0}] {1}", PersianDateTime.Now, text).WriteLine(foregroundColor);
            string.Format("[{0}] {1}", PersianDateTime.Now, text).WriteLine(foregroundColor);
        }

        public static void WriteInBoxTimeStamp(this string text) { Show(string.Format("[{0}] {1}", PersianDateTime.Now, text)); }
    }
}