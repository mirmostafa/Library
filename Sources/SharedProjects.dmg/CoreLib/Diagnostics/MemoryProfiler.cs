using System;
using Mohammad.Helpers;

namespace Mohammad.Diagnostics
{
    public class MemoryProfiler
    {
        private readonly string _DefaultSender;
        private long _StartMemory;
        private long? _StopMemory;
        public long UsedMemory => (this._StopMemory ?? GC.GetTotalMemory(false)) - this._StartMemory;
        public long UsingMemory => GC.GetTotalMemory(false) - this._StartMemory;

        public MemoryProfiler() { this._DefaultSender = "Memory Profiler"; }

        public void Reset()
        {
            LibrarySupervisor.Logger.Debug("Resetting", sender: this._DefaultSender);
            this._StopMemory = null;
        }

        public void Start()
        {
            LibrarySupervisor.Logger.Debug($"Starting. Current memory: {GC.GetTotalMemory(false).ToMesuranceSystem()}", sender: this._DefaultSender);
            this._StartMemory = GC.GetTotalMemory(false);
        }

        public void Stop()
        {
            this._StopMemory = GC.GetTotalMemory(false);
            LibrarySupervisor.Logger.Debug($"Stopped. Memory used: {this.UsedMemory.ToMesuranceSystem()}", sender: this._DefaultSender);
        }

        public void Cleanup(object o = null)
        {
            GC.Collect();
            GC.Collect(0, GCCollectionMode.Forced, true);
            GC.Collect(1, GCCollectionMode.Forced, true);
            GC.Collect(2, GCCollectionMode.Forced, true);
            if (o != null)
                GC.Collect(GC.GetGeneration(o), GCCollectionMode.Forced, true);
            LibrarySupervisor.Logger.Debug($"Cleaned up. Memory used: {this.UsedMemory.ToMesuranceSystem()}", sender: this._DefaultSender);
        }

        public static MemoryProfiler StartNew()
        {
            var result = new MemoryProfiler();
            result.Start();
            return result;
        }
    }
}