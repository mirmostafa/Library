#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using Mohammad.Wmi.Internals;

namespace Mohammad.Wmi.Win32
{
    [WmiClass(ClassName = "Win32_OperatingSystem")]
    public class OperatingSystem : WmiBase
    {
        [WmiProp] public string Caption { get; protected set; }
    }
}