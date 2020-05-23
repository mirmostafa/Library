using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using Mohammad.Collections.Specialized;
using Mohammad.DesignPatterns.Creational;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using static Mohammad.Helpers.CodeHelper;
using static Mohammad.Helpers.Console.ConsoleHelper;

namespace Mohammad.ServiceProcess
{
    public abstract class WindowsServiceProgramBase<TProgram> : Singleton<TProgram>
        where TProgram : WindowsServiceProgramBase<TProgram>
    {
        private readonly ManualResetEvent _HoldeResetEvent = new ManualResetEvent(false);
        private LibraryWindowsServiceBase[] _Services;
        public bool IsConsoleApp { get; private set; }
        private bool CanPauseOnEnded { get; set; } = true;

        public string[] Args { get; private set; }

        private void Startup(WindowsServiceProgramBaseStartupEventArgs e) { this.OnStartingup(e); }

        protected virtual void OnStartingup(WindowsServiceProgramBaseStartupEventArgs e) { }

        private void InitializeComponents() { this.OnInitializing(); }

        protected static void CallFromMain(params string[] args)
        {
            args = (args ?? Enumerable.Empty<string>()).Select(arg => arg.Replace("/", "-").ToLower()).ToArray();
            Instance.Args = args;
            try
            {
                var e = new WindowsServiceProgramBaseStartupEventArgs(args);
                Instance.Startup(e);
                if (e.Handled)
                    return;
                Instance.IsConsoleApp = e.IsConsoleApp ?? args.Contains("-console", true);
                if (Instance.IsConsoleApp && e.CanHandleCommandArguments)
                {
                    var location = Assembly.GetEntryAssembly().Location;
                    var actions = new Dictionary<string, Action>
                                  {
                                      {"-nopause", () => Instance.CanPauseOnEnded = false},
                                      {"-help", Instance.OnHelpRequired},
                                      {"-?", Instance.OnHelpRequired},
                                      {
                                          "-install", () =>
                                          {
                                              Instance.OnInstalling();
                                              Catch(() => ManagedInstallerClass.InstallHelper(new[] {location}));
                                          }
                                      },
                                      {
                                          "-uninstall", () =>
                                          {
                                              Instance.OnUninstalling();
                                              Catch(() => ManagedInstallerClass.InstallHelper(new[] {"/u", location}));
                                          }
                                      },
                                      {
                                          "-reinstall", () =>
                                          {
                                              Instance.OnUninstalling();
                                              Exception ex;
                                              if ((ex = Catch(() => ManagedInstallerClass.InstallHelper(new[] {"/u", location}))) != null)
                                              {
                                                  Error(
                                                      $"Error: Unable to uninstall. Reinstall operation failure.{Environment.NewLine}{ex.GetBaseException().Message}");
                                                  return;
                                              }
                                              Highlight("Uninstall operation is probably done.");
                                              Instance.OnInstalling();
                                              Catch(() => ManagedInstallerClass.InstallHelper(new[] {location}));
                                          }
                                      }
                                  };

                    if (ProcessCommandArguments(args, actions))
                    {
                        WaitForExit();
                        return;
                    }
                }

                Instance.InitializeComponents();

                if (Instance.IsConsoleApp)
                {
                    Console.Title = $"{ApplicationHelper.ApplicationTitle} - {ApplicationHelper.Version}";
                    Instance._Services = Instance.GetServices().RemoveNulls().ToArray();
                    Inform("Attempting to start services");
                    Instance._Services.ForEach(svc => Catch(() => svc.StartManually(args)));

                    WaitForExit(true);

                    Inform("Exiting.");
                    Instance.StopServices();
                }
                else
                {
                    ServiceBase.Run(Instance.GetServices().Cast<ServiceBase>().ToArray());
                }

                Instance.FinalizeComponents();
            }
            catch (Exception ex)
            {
                var e = new ItemActingEventArgs<Exception>(ex);
                Instance.OnUnhandlesExceptionOccurred(e);
                if (!e.Handled)
                    throw;
                WaitForExit(true);
            }
        }

        private void StopServices() { this._Services.ForEach(svc => Catch(svc.Stop)); }

        protected virtual void OnHelpRequired()
        {
            $"Product:\t{ApplicationHelper.ProductTitle}".WriteLine();
            "Application:\t".Write();
            Highlight($"{ApplicationHelper.ApplicationTitle}");
            "Version:\t".Write();
            Highlight($"{ApplicationHelper.Version}");
            ApplicationHelper.Description.WriteLine();
            LineFeed();
            GetDefaultCommandArguments();
            LineFeed();
            ApplicationHelper.Copyright.WriteLine();
        }

        private static void GetDefaultCommandArguments()
        {
            " -help OR -/?\tShows this message".WriteLine();
            " -console\tRuns in console mode".WriteLine();
            " -nopause\tAfter ending the application does does not wait for [X] key.".WriteLine();
            " -install\tInstalls this windows service".WriteLine();
            " -uninstall\tUninstalls this windows service".WriteLine();
            " -reinstall\tUninstalls and then installs this windows service".WriteLine();
            LineFeed();
            " Switch -console activates DEBUG Mode".WriteLine();
        }

        private static void WaitForExit(bool forcedPause = false)
        {
            Console.Title = $"{ApplicationHelper.ApplicationTitle} - {ApplicationHelper.Version}";
            var taskList = new TaskList();
            if (!Instance.CanPauseOnEnded && !forcedPause)
                return;
            Highlight("Ready. (Press [X] to exit)");
            taskList.Run(WaitForUserToExit);
            taskList.Run(WaitForInternalShutdown);
            taskList.WaitAny();
        }

        private static void WaitForInternalShutdown() { Instance._HoldeResetEvent.WaitOne(); }

        private static void WaitForUserToExit() { While(() => AskKey("", true).Key != ConsoleKey.X); }

        protected virtual void OnInstalling() { }

        protected virtual void OnUninstalling() { }

        public event EventHandler<ItemActingEventArgs<Exception>> UnhandlesExceptionOccurred;
        protected virtual void OnUnhandlesExceptionOccurred(ItemActingEventArgs<Exception> e) { this.UnhandlesExceptionOccurred?.Invoke(this, e); }

        private void FinalizeComponents() { this.OnFinalizing(); }

        protected virtual void OnInitializing() { }
        protected virtual void OnFinalizing() { }

        private IEnumerable<LibraryWindowsServiceBase> GetServices() => this.OnGettingServices();

        protected abstract IEnumerable<LibraryWindowsServiceBase> OnGettingServices();

        public void Shutdown() { this._HoldeResetEvent.Set(); }
    }
}