using System;
using System.Linq;
using Mohammad.Collections.ObjectModel;
using Mohammad.Helpers;

namespace Mohammad.Threading
{
    [Obsolete("Use async/await instead.")]
    public class AsyncCollection : EventualCollection<Async>
    {
        public Async this[string name] => (from async1 in this.Items
                                           where async1.Name.Equals(name)
                                           select async1).FirstOrDefault();

        public void AbortAll()
        {
            var asycns = this.Items.ToList();
            foreach (var async1 in asycns.Where(async1 => async1.Status != AsyncStatus.Ended))
            {
                var async2 = async1;
                CodeHelper.Catch(() => async2.Abort(),handling: async1.ExceptionHandling);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}