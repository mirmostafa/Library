#region

using System.IO;

#endregion

namespace Mohammad.Helpers.Console
{
    internal abstract class Dumper<TType>
    {
        internal readonly TextWriter Log;
        protected int Depth;
        protected int Level;
        protected int Pos;

        internal Dumper(int depth)
            : this(depth, System.Console.Out) { }

        internal Dumper(int depth, TextWriter log)
        {
            this.Depth = depth;
            this.Log = log;
        }

        internal abstract void Write(TType element, string prefix);
        internal abstract void WriteLine(TType element, string prefix);

        protected void WriteIndent()
        {
            for (var i = 0; i < this.Level; i++)
                this.Log.Write("  ");
        }

        protected void WriteLine()
        {
            this.Log.WriteLine();
            this.Pos = 0;
        }

        protected void Write(string s)
        {
            if (s == null)
                return;
            this.Log.Write(s);
            this.Pos += s.Length;
        }

        protected void WriteTab()
        {
            this.Write("  ");
            while (this.Pos % 8 != 0)
                this.Write(" ");
        }
    }
}