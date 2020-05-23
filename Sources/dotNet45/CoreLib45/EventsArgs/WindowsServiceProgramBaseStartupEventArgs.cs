#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

namespace Mohammad.EventsArgs
{
    public class WindowsServiceProgramBaseStartupEventArgs : ActingEventArgs
    {
        public WindowsServiceProgramBaseStartupEventArgs(string[] args) => this.Args = args;
        public bool? IsConsoleApp { get; set; } = null;

        public string[] Args                      { get; }
        public bool     CanHandleCommandArguments { get; set; } = true;
    }
}