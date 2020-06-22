using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using Mohammad.Helpers;

namespace Mohammad.ServiceProcess
{
    public class WindowsServiceContriller
    {
        private readonly ServiceController _ServiceController;
        public string DisplayName => this._ServiceController.DisplayName;
        public IEnumerable<WindowsServiceContriller> DependentServices => this._ServiceController.DependentServices.Select(ds => new WindowsServiceContriller(ds));
        public string ServiceName { get; internal set; }

        public IEnumerable<WindowsServiceContriller> ServicesDependedOn => this._ServiceController.ServicesDependedOn.Select(ds => new WindowsServiceContriller(ds));

        public SafeHandle ServiceHandle => this._ServiceController.ServiceHandle;

        public WindowsServiceStatus Status => Enum.Parse(typeof(WindowsServiceStatus), this._ServiceController.Status.ToString()).To<WindowsServiceStatus>();

        public ServiceType ServiceType => Enum.Parse(typeof(ServiceType), this._ServiceController.Status.ToString()).To<ServiceType>();
        internal WindowsServiceContriller(ServiceController serviceController) { this._ServiceController = serviceController; }
        public static IEnumerable<WindowsServiceContriller> GetServices() { return ServiceController.GetServices().Select(sc => new WindowsServiceContriller(sc)); }

        public static IEnumerable<WindowsServiceContriller> GetServicesByDisplayName(string displayName)
            => ServiceController.GetServices().Where(sc => sc.DisplayName == displayName).Select(sc => new WindowsServiceContriller(sc));

        public void Pause() { this._ServiceController.Pause(); }
        public void Continue() { this._ServiceController.Continue(); }
        public void ExecuteCommand(int command) { this._ServiceController.ExecuteCommand(command); }
        public void Refresh() { this._ServiceController.Refresh(); }
        public void Start() { this.Start(new string[0]); }
        public void Start(IEnumerable<string> args) { this._ServiceController.Start(args.ToArray()); }
        public void Stop() { this._ServiceController.Stop(); }
        public void WaitForStatus(ServiceControllerStatus desiredStatus) { this.WaitForStatus(desiredStatus, TimeSpan.MaxValue); }
        public void WaitForStatus(ServiceControllerStatus desiredStatus, TimeSpan timeout) { this._ServiceController.WaitForStatus(desiredStatus, timeout); }
        public override string ToString() => this.DisplayName;
    }
}