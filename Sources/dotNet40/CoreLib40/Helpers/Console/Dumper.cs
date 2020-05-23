#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

#region
using System.IO;
#endregion

namespace Library40.Helpers.Console
{
	internal abstract class Dumper<TType>
	{
		#region Fields

		#region Depth
		protected int Depth;
		#endregion

		#region Level
		protected int Level;
		#endregion

		#region Log
		internal readonly TextWriter Log;
		#endregion

		#region Pos
		protected int Pos;
		#endregion

		#endregion

		#region Methods

		#region Dumper
		internal Dumper(int depth)
			: this(depth, System.Console.Out)
		{
		}
		#endregion

		#region Dumper
		internal Dumper(int depth, TextWriter log)
		{
			this.Depth = depth;
			this.Log = log;
		}
		#endregion

		#region Write
		internal abstract void Write(TType element, string prefix);
		internal abstract void WriteLine(TType element, string prefix);
		#endregion

		#region WriteIndent
		protected void WriteIndent()
		{
			for (var i = 0; i < this.Level; i++)
				this.Log.Write("  ");
		}
		#endregion

		#region WriteLine
		protected void WriteLine()
		{
			this.Log.WriteLine();
			this.Pos = 0;
		}
		#endregion

		#region Write
		protected void Write(string s)
		{
			if (s == null)
				return;
			this.Log.Write(s);
			this.Pos += s.Length;
		}
		#endregion

		#region WriteTab
		protected void WriteTab()
		{
			this.Write("  ");
			while (this.Pos % 8 != 0)
				this.Write(" ");
		}
		#endregion

		#endregion
	}
}