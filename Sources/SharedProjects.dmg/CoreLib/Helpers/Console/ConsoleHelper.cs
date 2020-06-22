using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mohammad.Diagnostics;
using Mohammad.Logging;
using static Mohammad.Helpers.CodeHelper;

namespace Mohammad.Helpers.Console
{
    public static class ConsoleHelper
    {
        internal const ConsoleColor DEFAULT_CONSTANT_CONSOLE_COLOR = ConsoleColor.Cyan;
        public static ConsoleColor DefaultConsoleColor = ConsoleColor.Gray;
        public static ConsoleColor DefaultHighlightColor = ConsoleColor.Magenta;
        private static ILogger _Out;

        public static ILogger Out
        {
            get
            {
                if (_Out != null)
                    return _Out;
                _Out = new Logger("ConsoleHelper.Out");
                _Out.Logged += (_, e) => { WriteLine(Logger.FormatLogText(e), ConsoleColor.DarkGray); };
#if DEBUG
                _Out.IsDebugModeEnabled = true;
#endif
                return _Out;
            }
        }

        public static TResult Exec<TResult>(Func<TResult> func, string startPrompt = null, string endPrompt = "Done")
        {
            if (!startPrompt.IsNullOrEmpty())
                startPrompt.Write();
            var result = func.BeginInvoke(null, null);

            if (!endPrompt.IsNullOrEmpty())
                endPrompt.WriteLine();
            return func.EndInvoke(result);
        }

        public static ConsoleKey PressKeyInRange(Action prompter, ConsoleKey[] consoleKeys)
        {
            ConsoleKey key;
            prompter();
            while (!EnumHelper.IsEnumInRange(key = System.Console.ReadKey(true).Key, consoleKeys))
                prompter();
            return key;
        }

        public static ConsoleKey PressKeyInRange(string prompt, params ConsoleKey[] consoleKeys) { return PressKeyInRange(() => prompt.WriteLine(), consoleKeys); }

        public static void Write(params object[] objs) { objs.ForEach(obj => obj.Write()); }

        public static void Write(IEnumerable obj, ConsoleColor consoleColor = DEFAULT_CONSTANT_CONSOLE_COLOR) { obj.ForEach(item => item.Write(consoleColor)); }

        public static void WriteAndPause(params object[] objs)
        {
            objs.ForEach(obj => obj.WriteLine());
            Pause();
        }

        public static void WriteLineDump(object obj, ConsoleColor consoleColor = DEFAULT_CONSTANT_CONSOLE_COLOR)
        {
            if (!(obj is string || !(obj is IEnumerable)))
                ((IEnumerable) obj).ForEach(item => item.WriteLine(consoleColor));
            else
                WriteLine(obj, consoleColor);
        }

        public static void WriteLineReflect(object obj)
        {
            foreach (var property in ObjectHelper.ReflectProperties(obj))
                $"{property.Key.SeparateCamelCase()} = {property.Value}".WriteLine();
        }

        public static void Highlight(string s) { s.WriteLine(DefaultHighlightColor); }

        public static void Inform(string s) { s.WriteLine(ConsoleColor.White); }

        public static void Error(string s) { s.WriteLine(ConsoleColor.Red); }

        public static ConsoleKeyInfo AskKey(string prompt, bool intercept = false, ConsoleColor consoleColor = DEFAULT_CONSTANT_CONSOLE_COLOR)
        {
            prompt.Write(consoleColor);
            return System.Console.ReadKey(intercept);
        }

        public static string AskLine(string prompt, ConsoleColor consoleColor = DEFAULT_CONSTANT_CONSOLE_COLOR)
        {
            prompt.Write(consoleColor);
            return System.Console.ReadLine();
        }

        public static void LineFeed() { WriteLine(); }

        public static void Pause() { System.Console.ReadKey(true); }

        public static void Write(this object obj, ConsoleColor consoleColor = DEFAULT_CONSTANT_CONSOLE_COLOR)
        {
            var forgroundColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = consoleColor == DEFAULT_CONSTANT_CONSOLE_COLOR ? DefaultConsoleColor : consoleColor;
            System.Console.Write(obj);
            System.Console.ForegroundColor = forgroundColor;
        }

        public static void WriteAndPause(object obj, ConsoleColor consoleColor = DEFAULT_CONSTANT_CONSOLE_COLOR)
        {
            obj.Write(consoleColor);
            Pause();
        }

        public static void WriteLine() { "".WriteLine(); }

        public static void WriteLine(this object value, ConsoleColor consoleColor = DEFAULT_CONSTANT_CONSOLE_COLOR)
        {
            var forgroundColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = consoleColor == DEFAULT_CONSTANT_CONSOLE_COLOR ? DefaultConsoleColor : consoleColor;
            System.Console.WriteLine(value);
            System.Console.ForegroundColor = forgroundColor;
        }

        public static void WriteInLine(this object value, ConsoleColor consoleColor = DEFAULT_CONSTANT_CONSOLE_COLOR, bool addBackspace = true)
        {
            if (addBackspace)
                Write($"{"\b".Repeat(value?.ToString().Length ?? 0)}", consoleColor);
            System.Console.Write($"{value}", consoleColor);
        }

        public static void WriteLine(this string value, ConsoleColor consoleColor = DEFAULT_CONSTANT_CONSOLE_COLOR)
        {
            var forgroundColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = consoleColor == DEFAULT_CONSTANT_CONSOLE_COLOR ? DefaultConsoleColor : consoleColor;
            System.Console.WriteLine(value);
            System.Console.ForegroundColor = forgroundColor;
        }

        public static void WriteLine(this IEnumerable value, ConsoleColor consoleColor = DEFAULT_CONSTANT_CONSOLE_COLOR)
        {
            var forgroundColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = consoleColor == DEFAULT_CONSTANT_CONSOLE_COLOR ? DefaultConsoleColor : consoleColor;
            foreach (var item in value)
                System.Console.WriteLine(item);
            System.Console.ForegroundColor = forgroundColor;
        }

        public static void WriteLine(this Stopwatch value, ConsoleColor consoleColor = DEFAULT_CONSTANT_CONSOLE_COLOR)
        {
            var forgroundColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = consoleColor == DEFAULT_CONSTANT_CONSOLE_COLOR ? DefaultConsoleColor : consoleColor;
            System.Console.WriteLine(value.Elapsed);
            System.Console.ForegroundColor = forgroundColor;
        }

        public static void WriteLine(this IList values, ConsoleColor consoleColor = DEFAULT_CONSTANT_CONSOLE_COLOR)
        {
            foreach (var value in values)
                WriteLine(value, consoleColor);
        }

        public static void WriteLine(this string format, object arg0, ConsoleColor consoleColor = DEFAULT_CONSTANT_CONSOLE_COLOR)
        {
            var forgroundColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = consoleColor == DEFAULT_CONSTANT_CONSOLE_COLOR ? DefaultConsoleColor : consoleColor;
            System.Console.WriteLine(format, arg0);
            System.Console.ForegroundColor = forgroundColor;
        }

        public static void WriteLine(this string format, object arg0, object arg1, ConsoleColor consoleColor = DEFAULT_CONSTANT_CONSOLE_COLOR)
        {
            var forgroundColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = consoleColor == DEFAULT_CONSTANT_CONSOLE_COLOR ? DefaultConsoleColor : consoleColor;
            System.Console.WriteLine(format, arg0, arg1);
            System.Console.ForegroundColor = forgroundColor;
        }

        public static void WriteLineAndPause(object obj, ConsoleColor consoleColor = DEFAULT_CONSTANT_CONSOLE_COLOR)
        {
            obj.WriteLine(consoleColor);
            Pause();
        }

        public static ConsoleKeyInfo ShowMenu(IDictionary<char, string> items, string caption = null, string prompt = null, bool? showSelectedKey = null)
        {
            WriteLine(caption, ConsoleColor.White);
            WriteSeparatorLine();
            foreach (var item in items)
            {
                "\t".Write();
                item.Key.Write(ConsoleColor.Green);
                " - ".Write();
                Inform(item.Value);
            }
            while (true)
            {
                var result = AskKey(prompt + " ", !showSelectedKey ?? string.IsNullOrEmpty(prompt));
                var key = result.KeyChar;
                LineFeed();
                if (items.ContainsKey(key))
                    return result;
                Error("Invalid key.");
            }
        }

        public static void WriteSeparatorLine(char separatorchar = '=', int count = 20) { Inform(separatorchar.ToString().Repeat(count)); }
        public static void ShowElapsedTime(Action action) { $"Took: {Diag.Stopwatch(action).Elapsed}".WriteLine(); }

        public static bool ProcessCommandArguments(string[] args, IDictionary<string, Action> actions, bool continueOnException = true)
        {
            var result = false;
            var argsBuffer = (args ?? Enumerable.Empty<string>()).Select(arg => arg.ToLower());
            var actionsBugger = actions.Select(action => new KeyValuePair<string, Action>(action.Key.Replace("/", "-").ToLower(), action.Value)).ToDictionary();
            foreach (var arg in argsBuffer)
            {
                if (!actionsBugger.ContainsKey(arg))
                    continue;
                result = true;
                if (HasException(actionsBugger[arg]) && !continueOnException)
                    break;
            }
            return result;
        }
    }
}