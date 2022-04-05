using System.Runtime.Versioning;
using System.Security.Principal;

namespace Library.Security.Principal;

[SupportedOSPlatform("windows")]
public static class Impersonation
{
    public static bool IsRunningAsAdministrator()
    {
        using var windowsIdentity = WindowsIdentity.GetCurrent();
        var windowsPrincipal = new WindowsPrincipal(windowsIdentity);
        return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}
