using Mohammad.Wmi.Internals;

namespace Mohammad.Wmi.Win32
{
    [WmiClass(ClassName = "Win32_Process")]
    public class Win32Process : WmiBase
    {
        [WmiProp]
        public string Caption { get; set; }

        [WmiProp]
        public string CommandLine { get; set; }

        [WmiProp]
        public string CreationDate { get; set; }

        [WmiProp(Name = "CSCreationClassName")]
        public string CsCreationClassName { get; set; }

        [WmiProp(Name = "CSName")]
        public string CsName { get; set; }

        [WmiProp]
        public string Description { get; set; }

        [WmiProp]
        public string ExecutablePath { get; set; }

        [WmiProp]
        public string ExecutionState { get; set; }

        [WmiProp]
        public string Handle { get; set; }

        [WmiProp]
        public uint HandleCount { get; set; }

        [WmiProp]
        public string InstallDate { get; set; }

        [WmiProp]
        public ulong KernelModeTime { get; set; }

        [WmiProp]
        public uint MaximumWorkingSetSize { get; set; }

        [WmiProp]
        public uint MinimumWorkingSetSize { get; set; }

        [WmiProp(Name = "OSCreationClassName")]
        public string OsCreationClassName { get; set; }

        [WmiProp(Name = "OSName")]
        public string OsName { get; set; }

        [WmiProp]
        public ulong OtherOperationCount { get; set; }

        [WmiProp]
        public ulong OtherTransferCount { get; set; }

        [WmiProp]
        public uint PageFaults { get; set; }

        [WmiProp]
        public uint PageFileUsage { get; set; }

        [WmiProp]
        public uint ParentProcessId { get; set; }

        [WmiProp]
        public uint PeakPageFileUsage { get; set; }

        [WmiProp]
        public ulong PeakVirtualSize { get; set; }

        [WmiProp]
        public ulong PeakWorkingSetSize { get; set; }

        [WmiProp]
        public uint Priority { get; set; }

        [WmiProp]
        public ulong PrivatePageCount { get; set; }

        [WmiProp]
        public uint ProcessId { get; set; }

        [WmiProp]
        public uint QuotaNonPagedPoolUsage { get; set; }

        [WmiProp]
        public uint QuotaPagedPoolUsage { get; set; }

        [WmiProp]
        public uint QuotaPeakNonPagedPoolUsage { get; set; }

        [WmiProp]
        public uint QuotaPeakPagedPoolUsage { get; set; }

        [WmiProp]
        public ulong ReadOperationCount { get; set; }

        [WmiProp]
        public ulong ReadTransferCount { get; set; }

        [WmiProp]
        public uint SessionId { get; set; }
    }
}