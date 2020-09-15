using Mohammad.Wmi.Internals;

namespace Mohammad.Wmi.Win32
{
    [WmiClass(ClassName = "Win32_OperatingSystem")]
    public class OperatingSystem : WmiBase
    {
        [WmiProp]
        public string Caption { get; protected set; }
    }
}