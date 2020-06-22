using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mohammad.Helpers
{
    public static class EventHelper
    {
        public static void Raise(this Delegate handler, object sender, EventArgs e = null) { handler?.DynamicInvoke(sender, e ?? EventArgs.Empty); }

        public static TEvenetArgs Raise<TEvenetArgs>(this Delegate handler, object sender, TEvenetArgs e)
        {
            if (handler == null)
                return e;
            handler.DynamicInvoke(sender, e);
            return e;
        }

        public static TEvenetArgs Raise<TEvenetArgs>(this EventHandler<TEvenetArgs> handler, object sender, TEvenetArgs e)
        {
            if (handler == null)
                return e;
            handler(sender, e);
            return e;
        }

        public static Task RaiseAsync(this Delegate handler, object sender, TaskScheduler scheduler = null, Task task = null)
        {
            if (handler == null)
                return null;
            if (scheduler == null)
                return task?.ContinueWith(t => handler.DynamicInvoke(sender, EventArgs.Empty)) ?? Task.Run(() => handler.DynamicInvoke(sender, EventArgs.Empty));
            return task?.ContinueWith(t => handler.DynamicInvoke(sender, EventArgs.Empty), scheduler) ??
                   Task.Factory.StartNew(() => handler.DynamicInvoke(sender, EventArgs.Empty), CancellationToken.None, TaskCreationOptions.None, scheduler);
        }

        public static Task RaiseAsync(this EventHandler handler, object sender, Task task, TaskScheduler scheduler)
        {
            if (handler == null)
                return null;
            var e = EventArgs.Empty;
            if (task != null)
                return scheduler == null
                    ? task.ContinueWith(t =>
                    {
                        handler(sender, e);
                        return e;
                    })
                    : task.ContinueWith(t =>
                        {
                            handler(sender, e);
                            return e;
                        },
                        scheduler);
            if (scheduler == null)
                return Task.Run(() =>
                {
                    handler(sender, e);
                    return e;
                });
            return Task.Factory.StartNew(e1 =>
                {
                    handler(sender, e);
                    return e;
                },
                e,
                CancellationToken.None,
                TaskCreationOptions.None,
                scheduler);
        }

        public static Task RaiseAsync(this EventHandler handler, object sender, Task task) => RaiseAsync(handler, sender, task, null);
        public static Task RaiseAsync(this EventHandler handler, object sender, TaskScheduler scheduler) => RaiseAsync(handler, sender, null, scheduler);

        public static Task<TEvenetArgs> RaiseAsync<TEvenetArgs>(this EventHandler<TEvenetArgs> handler, object sender, TEvenetArgs e, Task task,
            TaskScheduler scheduler)
        {
            if (handler == null)
                return null;
            if (task == null)
            {
                if (scheduler == null)
                    return Task.Run(() =>
                    {
                        handler(sender, e);
                        return e;
                    });
                return Task.Factory.StartNew(e1 =>
                    {
                        handler(sender, e);
                        return e;
                    },
                    e,
                    CancellationToken.None,
                    TaskCreationOptions.None,
                    scheduler);
            }
            if (scheduler == null)
                return task.ContinueWith(t =>
                {
                    handler(sender, e);
                    return e;
                });
            return task.ContinueWith(t =>
                {
                    handler(sender, e);
                    return e;
                },
                scheduler);
        }

        public static Task<TEvenetArgs> RaiseAsync<TEvenetArgs>(this EventHandler<TEvenetArgs> handler, object sender, TEvenetArgs e, Task task)
            => RaiseAsync(handler, sender, e, task, null);

        public static Task<TEvenetArgs> RaiseAsync<TEvenetArgs>(this EventHandler<TEvenetArgs> handler, object sender, TEvenetArgs e, TaskScheduler scheduler)
            => handler != null ? RaiseAsync(handler, sender, e, null, scheduler) : null;

        public static Task<TEventArgs> RaiseAsync<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs e)
            => RaiseAsync(handler, sender, e, null, null);
    }
}