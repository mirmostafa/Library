using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Mohammad.Helpers;
using Mohammad.Wpf.Windows;

namespace Mohammad.Wpf.Helpers
{
    public static class ApplicationHelper
    {
        public static void DoEvents()
        {
            var nestedFrame = new DispatcherFrame();

            var exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(state =>
                {
                    {
                        var frame = state as DispatcherFrame;
                        if (frame != null)
                            frame.Continue = false;
                        return null;
                    }
                }),
                nestedFrame);
            Dispatcher.PushFrame(nestedFrame);

            if (exitOperation.Status != DispatcherOperationStatus.Completed)
                exitOperation.Abort();
        }

        public static void DoEvents(this Application app) { DoEvents(); }

        public static bool AmIAlone(this Application app)
        {
            var libApp = app as LibraryApplication;
            if (libApp == null)
                throw new TypeAccessException("Current App this not inherited form LibraryApplication.");
            return LibraryApplication.AmIAlone;
        }

        public static void RunInUiThread(this Application app, Action action) { app.Dispatcher.Invoke(DispatcherPriority.Render, action); }

        public static TResult RunInUiThread<TResult>(this Application app, Func<TResult> action)
        {
            return app.Dispatcher.Invoke(DispatcherPriority.Render, action).To<TResult>();
        }

        public static ImageSource GetImageSource(string relativePath) { return GetImageSource(relativePath, true); }

        public static ImageSource GetImageSource(string relativePath, bool imageInCallingAssembly)
        {
            if (string.IsNullOrEmpty(relativePath))
                return null;

            var path = imageInCallingAssembly ? GetCallingAssemblyPackPath() + relativePath : GetApplicationPackPath() + relativePath;

            var streamResourceInfo = Application.GetResourceStream(new Uri(path));
            return streamResourceInfo != null ? GenerateImage(relativePath, streamResourceInfo.Stream) : null;
        }

        public static string GetApplicationPackPath()
        {
            var assemblyName = GetExecutingAssemblyName();
            return "pack://application:,,,/" + assemblyName + ";component/";
        }

        public static string GetExecutingAssemblyName() { return Assembly.GetExecutingAssembly().FullName.Split(',')[0]; }

        internal static ImageSource GenerateImage(string relativePath, Stream stream)
        {
            BitmapDecoder decoder = null;
            if (relativePath.ToLower().EndsWith(".gif"))
                decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            else if (relativePath.ToLower().EndsWith(".png"))
                decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            else if (relativePath.ToLower().EndsWith(".ico"))
                decoder = new IconBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            else if (relativePath.ToLower().EndsWith(".jpg"))
                decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            return decoder?.Frames[0];
        }

        public static string GetCallingAssemblyPackPath()
        {
            var assemblyName = GetEntryAssemblyName();
            return "pack://application:,,,/" + assemblyName + ";component/";
        }

        public static string GetEntryAssemblyName() => Assembly.GetEntryAssembly().FullName.Split(',')[0];

        public static int GetThreadCount(this Application current) => Mohammad.Helpers.ApplicationHelper.CurrentThreadCount;
    }
}