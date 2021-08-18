using System.Security.Principal;

namespace Library.Security.Principal
{
    public static class Impersonation
    {
        public static bool IsRunningAsAdministrator()
        {
            var windowsIdentity = WindowsIdentity.GetCurrent();
            var windowsPrincipal = new WindowsPrincipal(windowsIdentity);
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
