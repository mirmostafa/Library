using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mohammad.Threading.Tasks
{
    public static class Processor
    {
        /// <summary>
        ///     Wastes CPU cores' time by essentially putting the processor into a very tight loop.
        /// </summary>
        /// <param name="callbackForEach">first integer: step. second integer: max (Environment.ProcessorCount)</param>
        /// <param name="iterations"></param>
        /// <param name="callbackSynchronizationContext">
        ///     The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the
        ///     continuation task and to use for its execution.
        /// </param>
        public static void SpinWait(int? iterations, Action<int, int> callbackForEach = null, TaskScheduler callbackSynchronizationContext = null)
        {
            var actions = new Task[Environment.ProcessorCount];
            for (var index = 0; index < actions.Length; index++)
            {
                var index1 = index;
                actions[index] = Async.Run(() => Thread.SpinWait(iterations ?? int.MaxValue));
                if (callbackForEach == null)
                    continue;
                if (callbackSynchronizationContext == null)
                    actions[index].ContinueWith(t => callbackForEach?.Invoke(index1, actions.Length));
                else
                    actions[index].ContinueWith(t => callbackForEach?.Invoke(index1, actions.Length), callbackSynchronizationContext);
            }
            Async.WaitAll(actions);
        }

        public static void SpinWait() { SpinWait(null); }

        public static string GetUsingProcessorsByMask(string processName = null)
            => Convert.ToString((int) GetProcessByName(processName).ProcessorAffinity, 2).PadLeft(Environment.ProcessorCount, '0');

        private static Process GetProcessByName(string processName = null)
            => string.IsNullOrEmpty(processName) ? Process.GetCurrentProcess() : Process.GetProcessesByName(processName).FirstOrDefault();

        public static void UseProcessorsByMask(string affinityMask, string processName = null)
        {
            GetProcessByName(processName).ProcessorAffinity = new IntPtr(Convert.ToInt32(affinityMask, 2));
        }
    }
}