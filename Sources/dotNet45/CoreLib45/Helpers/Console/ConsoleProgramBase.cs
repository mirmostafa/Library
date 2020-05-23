#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mohammad.EventsArgs;
using Mohammad.Globalization;
using Mohammad.Helpers.Console.Interpret;
using Mohammad.Logging;
using Mohammad.Primitives;

namespace Mohammad.Helpers.Console
{
    public abstract class ConsoleProgramBase<TProgram> : ApplicationMainThreadBase<TProgram>
        where TProgram : ConsoleProgramBase<TProgram>
    {
        #region Fields

        private          bool             _IsPaused;
        private readonly ManualResetEvent _ResetEvent = new ManualResetEvent(true);
        private          Task             _Task;

        #endregion

        public IReadOnlyList<string> Args            { get; private set; }
        public bool                  CatchExceptions { get; set; }
        public object                CommandsObject  { get; set; }
        public bool                  RunInTask       { get; set; } = true;

        public static void CallMeOnMain(params string[] args)
        {
            Instance.CatchExceptions = true;
            Instance.Args            = new List<string>(args.Merge(" ").Split('-', '/').Compact().Select(arg => arg.Trim())).AsReadOnly();
            Instance.Start();
        }

        protected virtual void ExceptionHandlingExceptionOccurred(object sender, ExceptionOccurredEventArgs<Exception> e)
        {
            $"[{PersianDateTime.Now}] Exception: {e.Exception.GetBaseException().Message}".WriteLine(ConsoleColor.Red);
        }

        protected abstract void Execute();

        protected override void OnExceptionOccurred(object sender, ExceptionOccurredEventArgs<Exception> e)
        {
            base.OnExceptionOccurred(sender, e);
            ConsoleHelper.Error($"{e.Exception.GetBaseException()}{Environment.NewLine}{e.MoreInfo}");
        }

        protected override ILogger OnInitializingLogger()
        {
            var logger = base.OnInitializingLogger();
            logger.Out = System.Console.Out;
            return logger;
        }

        protected override void OnStartingup()
        {
            Action run;
            if (this.CommandsObject == null)
            {
                System.Console.Title = $"{this.AppName} C=Clear, X=End, R=Repeat, S=Stop, P=Pause/Resume";
                if (this.RunInTask)
                {
                    System.Console.Title = $"{System.Console.Title} [RunInTask]";
                    "Running on Task.".WriteLine(ConsoleColor.Gray);
                    run = () =>
                    {
                        this._Task?.Dispose();
                        this._Task = Task.Run(this.Execute, this.CancellationTokenSource.Token);
                        this._Task.ContinueWith(t =>
                        {
                            Instance.CancellationTokenSource?.Dispose();
                            this.CancellationTokenSource = null;
                            "Ready.".WriteLine(ConsoleColor.Gray);
                        });
                    };
                }
                else
                {
                    run = Instance.Execute;
                }
            }
            else
            {
                System.Console.Title = $"{this.AppName} C=Clear, X=End, R=Repeat, H=Help";
                run = () =>
                {
                    var commands = this.CommandsObject
                                       .GetType()
                                       .GetMethods()
                                       .Where(m => m.GetCustomAttributes(typeof(ProgramCommandAttribute), true).Length == 1)
                                       .ToDictionary(m => char.ToLower(m.GetCustomAttributes(typeof(ProgramCommandAttribute), true).First()
                                                                        .As<ProgramCommandAttribute>().ShortKey));
                    var respKey = System.Console.ReadKey(true);
                    while (commands.ContainsKey(char.ToLower(respKey.KeyChar)))
                    {
                        commands[respKey.KeyChar].Invoke(this.CommandsObject, null);
                        respKey = System.Console.ReadKey(true);
                    }
                };
            }

            System.Console.WindowWidth        = System.Console.LargestWindowWidth - 3;
            System.Console.BackgroundColor    = ConsoleColor.DarkBlue;
            System.Console.ForegroundColor    = ConsoleColor.White;
            ConsoleHelper.DefaultConsoleColor = System.Console.ForegroundColor;
            System.Console.Clear();

            MessageBox.DefaultWidth = System.Console.WindowWidth - 1;
            this.Run(run);
            ConsoleHelper.LineFeed();
            if (!this.RunInTask)
                "Ready.".WriteLine(ConsoleColor.Gray);
            var key = this.IsApplicationShuttingdown ? new ConsoleKeyInfo() : System.Console.ReadKey(true);
            while (!this.IsApplicationShuttingdown && key.Key != ConsoleKey.X)
            {
                switch (key.Key)
                {
                    case ConsoleKey.C:
                        System.Console.Clear();
                        break;
                    case ConsoleKey.S:
                        if (!this.RunInTask)
                            break;
                        if (this._Task?.Status != TaskStatus.Running)
                        {
                            "No task is running.".WriteLine(ConsoleColor.Red);
                            break;
                        }

                        "Stopping...".WriteLine(ConsoleColor.DarkRed);
                        this.CancellationTokenSource.Cancel();
                        break;
                    case ConsoleKey.R:
                        ConsoleHelper.LineFeed();
                        "Repeat-----------------".WriteLine(ConsoleColor.Gray);
                        this.Run(run);
                        break;
                    case ConsoleKey.P:
                        if (!this.RunInTask)
                            break;
                        if (this._Task?.Status != TaskStatus.Running)
                        {
                            "No task is running.".WriteLine(ConsoleColor.Red);
                            break;
                        }

                        if (this._IsPaused)
                        {
                            "Resuming...".WriteLine(ConsoleColor.DarkRed);
                            this._ResetEvent.Set();
                            this._IsPaused = false;
                        }
                        else
                        {
                            "Pausing...".WriteLine(ConsoleColor.DarkRed);
                            this._ResetEvent.Reset();
                            this._IsPaused = true;
                        }

                        break;
                }

                if (!this.IsApplicationShuttingdown)
                    key = System.Console.ReadKey(true);
            }

            this.Shutdown();
        }

        private void Run(Action run)
        {
            if (this.CatchExceptions)
                try
                {
                    run();
                }
                catch (Exception ex)
                {
                    this.ExceptionHandling.HandleException(ex);
                }
            else
                run();
        }

        protected void WaitIfRequired()
        {
            this._ResetEvent.WaitOne();
        }
    }
}