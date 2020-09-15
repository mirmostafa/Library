using System;

namespace Mohammad.ServiceProcess
{
    public enum WindowsServiceStatus
    {
        Stopped = 1,
        StartPending = 2,
        StopPending = 3,
        Running = 4,
        ContinuePending = 5,
        PausePending = 6,
        Paused = 7
    }

    [Flags]
    public enum ServiceType
    {
        Adapter = 4,
        FileSystemDriver = 2,
        InteractiveProcess = 256,
        KernelDriver = 1,
        RecognizerDriver = 8,
        Win32OwnProcess = 16,
        Win32ShareProcess = 32
    }
}