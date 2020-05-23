#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using Mohammad.ProgressiveOperations;
using Mohammad.Wpf.Helpers;
using Mohammad.Wpf.Windows;
using Mohammad.Wpf.Windows.Dialogs;
using Mohammad.Wpf.Windows.Output;

namespace TestWpfApp45.Pages
{
    /// <summary>
    ///     Interaction logic for DefaultPage.xaml
    /// </summary>
    public partial class DefaultPage
    {
        public DefaultPage() { this.InitializeComponent(); }

        private async void OperationWatcherTest_OnExecuted(object sender, RoutedEventArgs e)
        {
            LibraryApplicationCodePack.Status.Set(true);
            LibraryApplicationCodePack.Status.Set("A multi-step operation started.");
            var steps = new MultiStepOperationStepCollection();
            var operation = MultiStepOperation.CreateInstance(steps);
            this.Watcher.Operation = operation;
            this.ProgressView.Operation = operation;
            const int max = 9;
            Action<string> sumlation = arg =>
            {
                operation.StartCurrentOperation(max);
                LibraryApplicationCodePack.Status.Set(arg);
                for (var i = 0; i < max / 3; i++)
                {
                    var index = 0;
                    Thread.Sleep(60);
                    operation.CurrentOperationIncreased($"{arg} - {++index}");
                    Thread.Sleep(60);
                    operation.CurrentOperationIncreased($"{arg} - {++index}");
                    Thread.Sleep(60);
                    operation.CurrentOperationIncreased($"{arg} - {++index}");
                }
                operation.EndCurrentOperation();
            };
            steps.Add(operation, sumlation, "Step 1", "Initializing...");
            steps.Add(operation, sumlation, "Step 2", "Gathering info...");
            steps.Add(operation, sumlation, "Step 3", "Copying files...");
            steps.Add(operation, sumlation, "Step 4", "Copying files...");
            steps.Add(operation, sumlation, "Step 5", "Copying files...");
            steps.Add(operation, sumlation, "Step 6", "Copying files...");
            steps.Add(operation, sumlation, "Step 7", "Copying files...");
            steps.Add(operation, sumlation, "Step 8", "Copying files...");
            steps.Add(operation, sumlation, "Step 9", "Copying files...");
            steps.Add(operation, sumlation, "Step 10", "Copying files...");
            steps.Add(operation, sumlation, "Step 11", "Copying files...");
            steps.Add(operation, sumlation, "Step 12", "Copying files...");
            steps.Add(operation, sumlation, "Step 13", "Copying files...");
            steps.Add(operation, sumlation, "Step 14", "Updating registry");
            steps.Add(operation, sumlation, "Step 15", "Finalizing...");
            operation.MainOperationStarted += (_, __) => LibraryApplicationCodePack.Status.Set("Doing...");
            operation.MainOperationEnded += (_, __) =>
            {
                LibraryApplicationCodePack.Status.Set();
                LibraryApplicationCodePack.Status.Set("Done.");
                Toast.Inform("Multi-operation simulation is done.", scale: ToastScale.Wide);
            };
            await operation.Start();
        }

        private void CommonTestsButton_OnClick(object sender, RoutedEventArgs e) { }

        private void DefaultPage_OnLoaded(object sender, RoutedEventArgs e) { }

        private void CalloutTextBox_OnTextChanged(object sender, EventArgs e) { }

        private void Watch_OnExecuted(object sender, EventArgs e) { }

        private void MsgBoxCheckButton_OnClick(object sender, RoutedEventArgs e) { MsgBox.Error("This is an error", "Hello", this.Window); }

        private void ShowProgressTestButton_OnClick(object sender, RoutedEventArgs e)
        {
            var counter = 0;
            Action<TaskDialog, Func<bool>, Func<bool>> action = (dlg, isCancelled, isBackgroundWorking) =>
            {
                LibraryApplicationCodePack.Status.Set(true);
                for (var i = 0; i < 100; i++)
                {
                    if (isCancelled())
                        break;
                    if (dlg.ProgressBar != null)
                        dlg.Set(DateTime.Now.ToLongTimeString(), prograssValue: ++counter, details: $"{counter} of 100 is done.");
                    LibraryApplicationCodePack.Status.SetProgressStep(counter, 100);
                    Thread.Sleep(500);
                }
                dlg.Set(text: isCancelled() ? "Cancelled by user." : "Done.");
                LibraryApplicationCodePack.Status.Set(false);
                //dlg.Close();
            };
            MsgBoxEx.ShowProgress(action,
                100,
                "Sending Commands",
                "Setting server IP for selected clients",
                "Initializing...",
                "Starting...",
                footerText: "For more information, see details",
                footerIcon: TaskDialogStandardIcon.Information,
                isCancallable: true,
                supportsBackgroundWorking: true);
        }

        private void MsgBoxTestButton_OnClick(object sender, RoutedEventArgs e)
        {
            LibraryApplicationCodePack.Status.Set(true);
            MsgBoxEx.ShowProgress(Enumerable.Range(1, 50000), (dlg, num) => dlg.Set(DateTime.Now.ToLongTimeString()));
            LibraryApplicationCodePack.Status.Set(false);
        }
    }
}

#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed