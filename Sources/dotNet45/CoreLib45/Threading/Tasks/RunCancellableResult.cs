#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Threading;

namespace Mohammad.Threading.Tasks
{
    public class RunCancellableResult
    {
        public bool                    IsDone                  { get; internal set; }
        public CancellationTokenSource CancellationTokenSource { get; internal set; }
    }

    public class RunCancellableResult<TResult> : RunCancellableResult
    {
        public TResult Result { get; internal set; }
    }
}