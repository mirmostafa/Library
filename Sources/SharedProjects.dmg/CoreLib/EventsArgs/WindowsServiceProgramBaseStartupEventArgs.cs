namespace Mohammad.EventsArgs
{
    public class WindowsServiceProgramBaseStartupEventArgs : ActingEventArgs
    {
        public bool? IsConsoleApp { get; set; } = null;

        public string[] Args { get; }
        public bool CanHandleCommandArguments { get; set; } = true;

        public WindowsServiceProgramBaseStartupEventArgs(string[] args) { this.Args = args; }
    }
}