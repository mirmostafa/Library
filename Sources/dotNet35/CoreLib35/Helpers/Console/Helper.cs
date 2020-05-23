#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;

namespace Library35.Helpers.Console
{
	public static class Helper
	{
		// Fields
		public static ConsoleColor DefaultConsoleColor = ConsoleColor.Gray;

		// Methods
		public static ConsoleKeyInfo AskKey(this string prompt, bool intercept = false, ConsoleColor consoleColor = ConsoleColor.Cyan)
		{
			prompt.Write(consoleColor);
			return System.Console.ReadKey(intercept);
		}

		public static string AskLine(this string prompt, ConsoleColor consoleColor = ConsoleColor.Cyan)
		{
			prompt.Write(consoleColor);
			return System.Console.ReadLine();
		}

		public static TResult Exec<TResult>(this Func<TResult> func, string startPrompt = null, string endPrompt = "Done")
		{
			if (!startPrompt.IsNullOrEmpty())
				startPrompt.Write();
			var result = func.BeginInvoke(null, null);

			if (!endPrompt.IsNullOrEmpty())
				endPrompt.WriteLine();
			return func.EndInvoke(result);
		}

		public static void LineFeed()
		{
			Environment.NewLine.WriteLine();
		}

		public static void Pause()
		{
			System.Console.ReadKey();
		}

		public static ConsoleKey PressKeyInRange(Action prompter, ConsoleKey[] consoleKeys)
		{
			ConsoleKey key;
			prompter();
			while (!(key = System.Console.ReadKey(true).Key).IsEnumInRange(consoleKeys))
				prompter();
			return key;
		}

		public static ConsoleKey PressKeyInRange(string prompt, params ConsoleKey[] consoleKeys)
		{
			return PressKeyInRange(() => prompt.WriteLine(), consoleKeys);
		}

		public static void Write(params object[] objs)
		{
			objs.ForEach(obj => obj.Write());
		}

		public static void Write(IEnumerable obj, ConsoleColor consoleColor = ConsoleColor.Cyan)
		{
			obj.ForEach(item => item.Write(consoleColor));
		}

		public static void Write(this object obj, ConsoleColor consoleColor = ConsoleColor.Cyan)
		{
			var forgroundColor = System.Console.ForegroundColor;
			System.Console.ForegroundColor = (consoleColor == ConsoleColor.Cyan) ? DefaultConsoleColor : consoleColor;
			System.Console.Write(obj);
			System.Console.ForegroundColor = forgroundColor;
		}

		public static void WriteAndPause(params object[] objs)
		{
			objs.ForEach(obj => obj.WriteLine());
			Pause();
		}

		public static void WriteAndPause(this object obj, ConsoleColor consoleColor = ConsoleColor.Cyan)
		{
			obj.Write(consoleColor);
			Pause();
		}

		public static void WriteLine()
		{
			"".WriteLine();
		}

		public static void WriteLine(params object[] objs)
		{
			objs.ForEach(obj => obj.WriteLine());
		}

		public static void WriteLine(this object value, ConsoleColor consoleColor = ConsoleColor.Cyan)
		{
			var forgroundColor = System.Console.ForegroundColor;
			System.Console.ForegroundColor = (consoleColor == ConsoleColor.Cyan) ? DefaultConsoleColor : consoleColor;
			System.Console.WriteLine(value);
			System.Console.ForegroundColor = forgroundColor;
		}

		public static void WriteLine(this string format, object arg0, ConsoleColor consoleColor = ConsoleColor.Cyan)
		{
			var forgroundColor = System.Console.ForegroundColor;
			System.Console.ForegroundColor = (consoleColor == ConsoleColor.Cyan) ? DefaultConsoleColor : consoleColor;
			System.Console.WriteLine(format, arg0);
			System.Console.ForegroundColor = forgroundColor;
		}

		public static void WriteLine(this string format, object arg0, object arg1, ConsoleColor consoleColor = ConsoleColor.Cyan)
		{
			var forgroundColor = System.Console.ForegroundColor;
			System.Console.ForegroundColor = (consoleColor == ConsoleColor.Cyan) ? DefaultConsoleColor : consoleColor;
			System.Console.WriteLine(format, arg0, arg1);
			System.Console.ForegroundColor = forgroundColor;
		}

		public static void WriteLineAndPause(this object obj, ConsoleColor consoleColor = ConsoleColor.Cyan)
		{
			obj.WriteLine(consoleColor);
			Pause();
		}

		public static void WriteLineDump(this object obj)
		{
			ObjectDumper.WriteLineDump(obj);
			System.Console.WriteLine();
		}

		public static void WriteLineDump(this object obj, ConsoleColor consoleColor = ConsoleColor.Cyan)
		{
			if (!((obj is string) || !(obj is IEnumerable)))
				(obj as IEnumerable).ForEach(item => item.WriteLine());
			else
				System.Console.WriteLine(obj);
		}
	}
}