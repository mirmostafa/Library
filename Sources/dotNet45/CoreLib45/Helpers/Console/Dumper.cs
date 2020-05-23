#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

#region

using System.IO;

#endregion

namespace Mohammad.Helpers.Console
{
    internal abstract class Dumper<TType>
    {
        #region Fields

        internal readonly TextWriter Log;

        #endregion

        internal Dumper(int depth)
            : this(depth, System.Console.Out)
        {
        }

        internal Dumper(int depth, TextWriter log)
        {
            this.Depth = depth;
            this.Log = log;
        }

        protected void Write(string s)
        {
            if (s == null)
            {
                return;
            }

            this.Log.Write(s);
            this.Pos += s.Length;
        }

        protected void WriteIndent()
        {
            for (var i = 0; i < this.Level; i++)
            {
                this.Log.Write("  ");
            }
        }

        protected void WriteLine()
        {
            this.Log.WriteLine();
            this.Pos = 0;
        }

        protected void WriteTab()
        {
            this.Write("  ");
            while (this.Pos % 8 != 0)
            {
                this.Write(" ");
            }
        }

        internal abstract void Write(TType element, string prefix);

        internal abstract void WriteLine(TType element, string prefix);

        #region Fields

        protected int Depth;
        protected int Level;
        protected int Pos;

        #endregion
    }
}