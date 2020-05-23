#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Diagnostics;

namespace Mohammad.Helpers
{
    partial class ApplicationHelper
    {
        public static int CurrentThreadCount => Process.GetCurrentProcess().Threads.Count;
    }
}