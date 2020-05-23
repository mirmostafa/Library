#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.Threading
{
    [Obsolete("Use async/await instead.")]
    public enum AsyncStatus
    {
        Initializing,
        Waiting,
        Running,
        Ended
    }
}