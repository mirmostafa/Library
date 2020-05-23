#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Library40.EventsArgs;
using Library40.ExceptionHandlingPattern;
using Library40.Globalization;

namespace Library40.Helpers.Console
{
	public abstract class ProgramBase<TProgram> : IExceptionHandlerContainer
		where TProgram : ProgramBase<TProgram>, new()
	{
		private ExceptionHandling _ExceptionHandling;
		public IEnumerable<string> Args { get; private set; }
		public static TProgram Instance { get; protected set; }

		#region IExceptionHandlerContainer Members
		public ExceptionHandling ExceptionHandling
		{
			get
			{
				if (this._ExceptionHandling == null)
				{
					this._ExceptionHandling = new ExceptionHandling
					                          {
						                          RaiseExceptions = false
					                          };
					this._ExceptionHandling.ExceptionOccurred += this.ExceptionHandlingExceptionOccurred;
					return this._ExceptionHandling;
				}
				return this._ExceptionHandling;
			}
			set { this._ExceptionHandling = value; }
		}
		#endregion

		protected virtual void ExceptionHandlingExceptionOccurred(object sender, ExceptionOccurredEventArgs<Exception> e)
		{
			Helper.WriteLine(string.Format("[{0}] Exception: {1}", PersianDateTime.Now, e.Exception.GetBaseException().Message), ConsoleColor.Red);
		}

		public static void CallMeOnMain(params string[] args)
		{
			Instance = new TProgram
			           {
				           Args = args.Merge(" ").Split(new[]
				                                        {
					                                        '-', '/'
				                                        }).Compact()
			           };
			Instance.Args = Instance.Args.Select(arg => arg.Trim());
			Action run = Instance.Execute;
			ApplicationHelper.Prepare("fa-IR");

			System.Console.Title = "Mohammad Lab Console, (C) 2004 MOHAMMAD. C=Clear, E=End, R=Repeat";
			System.Console.WindowWidth = System.Console.LargestWindowWidth - 3;
			System.Console.BackgroundColor = ConsoleColor.White;
			System.Console.ForegroundColor = ConsoleColor.DarkBlue;
			Helper.DefaultConsoleColor = System.Console.ForegroundColor;
			System.Console.Clear();

			MessageBox.DefaultWidth = System.Console.WindowWidth - 1;

			try
			{
				Instance.OnStarted();
				run();
			}
			catch (Exception ex)
			{
				Instance.ExceptionHandling.HandleException(ex);
			}
			Helper.LineFeed();
			"Ended.".WriteLine(consoleColor: ConsoleColor.Gray);
			var key = System.Console.ReadKey(true);
			while (key.Key != ConsoleKey.E)
			{
				switch (key.Key)
				{
					case ConsoleKey.C:
						System.Console.Clear();
						break;
					case ConsoleKey.R:
						Helper.LineFeed();
						Helper.WriteLine("Repeat-----------------", ConsoleColor.Gray);
						try
						{
							run();
						}
						catch (Exception ex)
						{
							Instance.ExceptionHandling.HandleException(ex);
						}
						Helper.LineFeed();
						Helper.WriteLine("Ended.",consoleColor: ConsoleColor.Gray);
						break;
				}
				key = System.Console.ReadKey(true);
			}
			try
			{
				Instance.OnEnded();
			}
			catch (Exception ex)
			{
				Instance.ExceptionHandling.HandleException(ex);
			}
		}

		protected abstract void Execute();

		protected virtual void OnEnded()
		{
		}

		protected virtual void OnStarted()
		{
		}
	}
}