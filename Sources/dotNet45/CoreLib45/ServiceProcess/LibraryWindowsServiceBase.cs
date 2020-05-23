#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

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