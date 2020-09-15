using System.ServiceProcess;

namespace Mohammad.ServiceProcess
{
    public class LibraryWindowsServiceBase : ServiceBase
    {
        protected string[] Args { get; set; }

        public void StartManually(params string[] args)
        {
            this.Args = args;
            this.OnStart(this.Args);
        }
    }
}