namespace Mohammad.EventsArgs
{
    public class WindowsServiceProgramBaseStartupEventArgs : ActingEventArgs
    {
        public WindowsServiceProgramBaseStartupEventArgs(string[] args) => this.Args = args;
        public bool? IsConsoleApp { get; set; } = null;

        public string[] Args { get; }
        public bool CanHandleCommandArguments { get; set; } = true;
    }
}