using System.Diagnostics;

namespace Mohammad.Helpers
{
    partial class ApplicationHelper
    {
        public static int CurrentThreadCount => Process.GetCurrentProcess().Threads.Count;
    }
}