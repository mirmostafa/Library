#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using Mohammad.EventsArgs;

namespace Mohammad.Interfaces
{
    public interface IProgressiveOperation<T>
    {
        event EventHandler<ProgressiveOperationEventArgs<T>> ProgressChanged;
    }
}