


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.Helpers;
using Mohammad.Threading.Tasks;
using static Mohammad.Helpers.CodeHelper;

namespace Mohammad.ServiceProcess
{
    public static class WindowsServiceApplicationHelper
    {
        private static ManualResetEvent _Mrs;

        public static void Run<TWinService>(Action intractiveAction, ExceptionHandling exceptionHandling = null, bool autoStop = false)
            where TWinService : ServiceBase, new()
            => Run(new TWinService(), intractiveAction, exceptionHandling, autoStop);

        public static void Run(ServiceBase winService, Action intractiveAction, ExceptionHandling exceptionHandling = null, bool autoStop = false)
            => Run(EnumerableHelper.ToEnumerable(winService), intractiveAction, exceptionHandling, autoStop);

        public static void Run(IEnumerable<ServiceBase> winServices,
            Action intractiveAction,
            ExceptionHandling exceptionHandling = null,
            bool autoStop = false)
        {
            if (!Debugger.IsAttached)
            {
                Catch(() => ServiceBase.Run(winServices.ToArray()), handling: exceptionHandling);
            }
            else
            {
                var task = Async.Run(() =>
                {
                    Catch(intractiveAction, handling: exceptionHandling);
                });
                if (autoStop)
                {
                    task.ContinueWith(t => Exit());
                }

                _Mrs = new ManualResetEvent(false);
                _Mrs.WaitOne();
                _Mrs.Dispose();
                task.Dispose();
            }
        }

        public static void Exit() => Catch(() => _Mrs.Set());
    }
}