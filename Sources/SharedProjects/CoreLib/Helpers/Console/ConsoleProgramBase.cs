using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mohammad.Primitives;
using Mohammad.EventsArgs;
using Mohammad.Globalization;
using Mohammad.Helpers.Console.Interpret;
using Mohammad.Logging;

namespace Mohammad.Helpers.Console
{
    public abstract class ConsoleProgramBase<TProgram> : ApplicationMainThreadBase<TProgram>
        where TProgram : ConsoleProgramBase<TProgram>
    {
        public bool CatchExceptions { get; set; }
        public IReadOnlyList<string> Args { get; private set; }
        public object CommandsObject { get; set; }
        public bool RunInTask { get; set; }

        protected override ILogger OnInitializingLogger()
        {
            var logger = base.OnInitializingLogger();
            logger.Out = System.Console.Out;
            return logger;
        }

        protected virtual void ExceptionHandlingExceptionOccurred(object sender, ExceptionOccurredEventArgs<Exception> e)
        {
            $"[{PersianDateTime.Now}] Exception: {e.Exception.GetBaseException().Message}".WriteLine(ConsoleColor.Red);
        }

        public static void CallMeOnMain(params string[] args)
        {
            Instance.CatchExceptions = true;
            Instance.Args = new List<string>(args.Merge(" ").Split('-', '/').Compact().Select(arg => arg.Trim())).AsReadOnly();
            Instance.Start();
        }

        protected override void OnStartingup()
        {
            Action run;
            if (this.CommandsObject == null)
            {
                System.Console.Title = $"{this.AppName} C=Clear, X=End, R=Repeat, S=Stop";
                if (this.RunInTask)
                    run = () =>
                    {
                        "Running on Task.".WriteLine(ConsoleColor.Gray);
                        var task = Task.Run(() => this.Execute(), this.CancellationTokenSource.Token);
                        task.ContinueWith(t =>
                        {
                            Instance.CancellationTokenSource?.Dispose();
                            this.CancellationTokenSource = null;
                            "Ready.".WriteLine(ConsoleColor.Gray);
                        });
                    };
                else
                    run = Instance.Execute;
            }
            else
            {
                System.Console.Title = $"{this.AppName} C=Clear, X=End, R=Repeat, H=Help";
                run = () =>
                {
                    var commands =
                        this.CommandsObject.GetType()
                            .GetMethods()
                            .Where(m => m.GetCustomAttributes(typeof(ProgramCommandAttribute), true).Length == 1)
                            .ToDictionary(
                                m => char.ToLower(m.GetCustomAttributes(typeof(ProgramCommandAttribute), true).First().As<ProgramCommandAttribute>().ShortKey));
                    var respKey = System.Console.ReadKey(true);
                    while (commands.ContainsKey(char.ToLower(respKey.KeyChar)))
                    {
                        commands[respKey.KeyChar].Invoke(this.CommandsObject, null);
                        respKey = System.Console.ReadKey(true);
                    }
                };
            }
            System.Console.WindowWidth = System.Console.LargestWindowWidth - 3;
            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            System.Console.ForegroundColor = ConsoleColor.White;
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
                        "Stopping...".WriteLine(ConsoleColor.DarkRed);
                        this.CancellationTokenSource.Cancel();
                        break;
                    case ConsoleKey.R:
                        ConsoleHelper.LineFeed();
                        "Repeat-----------------".WriteLine(ConsoleColor.Gray);
                        this.Run(run);
                        //ConsoleHelper.LineFeed();
                        //"Ready.".WriteLine(ConsoleColor.Gray);
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

        protected override void OnExceptionOccurred(object sender, ExceptionOccurredEventArgs<Exception> e)
        {
            base.OnExceptionOccurred(sender, e);
            ConsoleHelper.Error($"{e.Exception.GetBaseException()}{Environment.NewLine}{e.MoreInfo}");
        }

        protected abstract void Execute();
    }
}