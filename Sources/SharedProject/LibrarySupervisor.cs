#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using Mohammad.Logging;

namespace Mohammad
{
    public static class LibrarySupervisor
    {
        private static ILogger _Logger;

        public static ILogger Logger
        {
            get => _Logger ?? Logging.Logger.Empty;
            set => _Logger = value;
        }
    }
}