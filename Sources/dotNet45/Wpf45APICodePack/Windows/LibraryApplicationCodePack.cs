using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.WindowsAPICodePack.ApplicationServices;
using Microsoft.WindowsAPICodePack.Taskbar;
using Mohammad.EventsArgs;
using Mohammad.Exceptions;
using Mohammad.Helpers;
using Mohammad.Threading.Tasks;
using Mohammad.Validation.Exceptions;
using Mohammad.Wpf.Windows.Controls;
using Mohammad.Wpf.Windows.Dialogs;

namespace Mohammad.Wpf.Windows
{
    public class LibraryApplicationCodePack : LibraryApplication
    {
        public static Status Status
        {
            get
            {
                var window = CodeHelper.CatchFunc(() => Current.MainWindow.As<LibraryGlassWindow>());
                if (window != null)
                {
                    return window.Status;
                }

                var task = Async.Run(() => Current.MainWindow.As<LibraryGlassWindow>(), scheduler: MainTaskScheduler);
                task.Wait();
                if (task.Result == null)
                {
                    throw new WindowMismatchException("Application main window is not LibraryGlassWindow.");
                }

                return task.Result.Status;
            }
        }

        public bool CanHandleValidationExceptions { get; set; } = true;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.RegisterForRestart();
            this.RegisterForRecovery();
            if (e.Args.Contains("/crashed", true))
            {
                this.OnCreashed();
            }

            await Task.Factory.StartNew(() =>
                {
                    Task.Delay(1000);
                })
                .ContinueWith(t =>
                    {
                        var jumpList = JumpList.CreateJumpListForIndividualWindow(null, Current.MainWindow);
                        this.UpdateJumpList(jumpList);
                    },
                    TaskScheduler.FromCurrentSynchronizationContext());
        }

        protected virtual void OnCreashed()
        {
            MsgBoxEx.Error("Application crashed last time");
        }

        protected virtual void OnUpdateJumpList(JumpList jumpList)
        {
        }

        protected virtual void RegisterForRestart()
        {
            ApplicationRestartRecoveryManager.RegisterForApplicationRestart(new RestartSettings("/crashed", RestartRestrictions.None));
        }

        protected virtual void RegisterForRecovery()
        {
            ApplicationRestartRecoveryManager.RegisterForApplicationRecovery(new RecoverySettings(new RecoveryData(RecoveryProcedure, null), 0));
        }

        private void UpdateJumpList(JumpList jumpList)
        {
            //jumpList.ClearAllUserTasks();
            //jumpList.AddUserTasks(new JumpListLink(Path.Combine(Environment.CurrentDirectory, "MiMail.exe"), "Mail")
            //{
            //	Arguments = "mail",
            //});
            //jumpList.AddUserTasks(new JumpListLink(Path.Combine(Environment.CurrentDirectory, "MiMail.exe"), "Contacts")
            //{
            //	Arguments = "contacts"
            //});
            //jumpList.AddUserTasks(new JumpListLink(Path.Combine(Environment.CurrentDirectory, "MiMail.exe"), "Calendar")
            //{
            //	Arguments = "calendar"
            //});
            //var category = new JumpListCustomCategory("Mail");
            //category.AddJumpListItems(new JumpListLink(Path.Combine(Environment.CurrentDirectory, "MiMail.exe"), "Inbox")
            //{
            //	Arguments = "inbox"
            //});
            //category.AddJumpListItems(new JumpListLink(Path.Combine(Environment.CurrentDirectory, "MiMail.exe"), "New Message")
            //{
            //	Arguments = "newMessage"
            //});
            //jumpList.AddCustomCategories(category);

            this.OnUpdateJumpList(jumpList);
            if (jumpList.MaxSlotsInList > 0)
            {
                jumpList.Refresh();
            }
        }

        private static int RecoveryProcedure(object state)
        {
            var isCanceled = ApplicationRestartRecoveryManager.ApplicationRecoveryInProgress();

            if (isCanceled)
            {
                Environment.Exit(2);
            }

            ApplicationRestartRecoveryManager.ApplicationRecoveryFinished(true);
            return 0;
        }

        internal override void ExceptionOccurred(object sender, ExceptionOccurredEventArgs<Exception> e)
        {
            if (e.Exception is BreakException)
            {
                e.Handled = true;
            }
            else if (this.CanHandleValidationExceptions && e.Exception is IValidationException)
            {
                if (e.Exception is ValidationExceptionBase)
                {
                    var ex = e.Exception as ValidationExceptionBase;
                    MsgBoxEx.Error(ex.Instruction, ex.GetBaseException().Message);
                }
                else
                {
                    MsgBoxEx.Exception(e.Exception);
                }

                e.Handled = true;
            }

            base.ExceptionOccurred(sender, e);
        }
    }
}