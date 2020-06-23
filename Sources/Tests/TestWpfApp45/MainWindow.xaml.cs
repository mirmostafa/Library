// Created on     2018/07/25
// Last update on 2018/08/12 by Mohammad Mir mostafa 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Mohammad.EventsArgs;
using Mohammad.IO;
using Mohammad.Logging;
using Mohammad.Primitives;
using Mohammad.Validation.Exceptions;
using Mohammad.Wpf.Specialized;
using Mohammad.Wpf.Windows;
using Mohammad.Wpf.Windows.Dialogs;
using Mohammad.Wpf.Windows.Output;
using Mohammad.Wpf.Windows.Settings;
using TestWpfApp45.Pages;

namespace TestWpfApp45
{
    public partial class MainWindow
    {
        private readonly List<DirectoryWatcher> _Watchers = new List<DirectoryWatcher>();

        public MainWindow()
        {
            this.InitializeComponent();
            TestWpfApp45.Commands.ShowMainWindow.Instance = this;
        }

        protected override WindowSettings OnWindowSettingsRequired() => App.Injector.Settings.MainWindow;

        protected override void OnInitializingStatus(out ProgressBar progressBar, out StatusBarItem statusBarItem, out ISimpleLogger logger)
        {
            //logger = this.Watcher;
            progressBar = this.ProgressBar;
            statusBarItem = null;
            logger = App.Injector.Logger;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Status.Set($"Setting on {Os.GetVersion()}");
            //await Run(this.InitializeWatchers);
            this.Status.Set("Ready", level: LogLevel.Status);
        }

        private void EventNavigation_OnNavigating(object sender, ItemActingEventArgs<object> e) => e.Item = App.WatcherPage;

        private void ShowTestPage2_OnExecuted(object sender, EventArgs e) => LibraryDialog.ShowDialog<Page2>(this);

        //Break();
        private void CommonTests_OnExecuted(object sender, EventArgs e) => ValidationException.WrapThrow("", "No machine selected.");

        private async void LoadBingImage_OnExecuted(object sender, EventArgs routedEventArgs)
        {
            var o = this.Resources["Assets/DefaultTrayIcon.ico"];
            //Resources.Source
            LibraryApplicationCodePack.Status.Set("Please wait while Bing image is being loaded.", true);
            var toast = Toast.Inform("Getting image info", "Loading Image...", "Bing Image");

            this.CommandBar.CommandManager["LoadBing"].IsEnabled = false;
            //Toast.Show(null, "Bing Image", "Getting image info");
            this.BackgroundImage.Source = await BingImage.GetBingImageAsync();
            Toast.Inform(toast, "Downloading", "Loading Image...", "Bing Image", false);
            //Toast.Show(null, "Bing Image", "Downloading");
            this.BackgroundImage.Source.Changed += (_, __) =>
            {
                this.CommandBar.CommandManager["LoadBing"].IsEnabled = true;
                LibraryApplicationCodePack.Status.Set("Image Loaded.", false);
                Toast.Inform(toast, "Image Loaded.", "Background has been set to new Bing image.", "Bing Image");
                Toast.Show(null, "Bing Image", "Background has been set to new Bing image.");
            };
        }

        private void InitializeWatchers()
        {
            //if (!App.Settings.WatchList.Any())
            //	DriveInfo.GetDrives().Where(drv => drv.IsReady).ForEach(drv => App.Settings.WatchList.Add(new WatchItem { Path = drv.RootDirectory.FullName }));

            this._Watchers.Clear();
            var paths = DriveInfo.GetDrives().Where(drv => drv.IsReady).Select(drv => drv.RootDirectory.FullName);
            foreach (var watcher in paths.Select(path => new DirectoryWatcher(path)))
            {
                watcher.Added += this.Watcher_OnAdded;
                watcher.Deleted += this.Watcher_OnDeleted;
                watcher.Modified += this.Watcher_OnModified;
                watcher.Renamed += this.Watcher_OnRenamed;
                watcher.Start();
                this._Watchers.Add(watcher);
            }
        }

        private void Watcher_OnRenamed(object sender, ChangedEventArgs<string> e) => this.Status.Set($"{e.OldValue} to {e.NewValue}", detail: "renamed");

        private void Watcher_OnModified(object sender, ItemActedEventArgs<string> e)
        {
            if (e.Item.EndsWith("Logs.txt"))
            {
                return;
            }

            this.Status.Set(e.Item, detail: "modified");
        }

        private void Watcher_OnDeleted(object sender, ItemActedEventArgs<string> e) => this.Status.Set(e.Item, detail: "deleted");

        private void Watcher_OnAdded(object sender, ItemActedEventArgs<string> e) => this.Status.Set(e.Item, detail: "added");
        //}
        //        this._Watchers.ForEach(w => CodeHelper.Catch(w.Stop));
        //    else
        //        this._Watchers.ForEach(w => w.Start());
        //    if (e.Item ?? false)
        //{

        //private void LibCommand_OnIsCheckedChanged(object sender, ItemActedEventArgs<bool?> e)
    }
}