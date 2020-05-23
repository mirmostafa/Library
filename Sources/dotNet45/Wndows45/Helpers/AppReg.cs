using System.Windows.Forms;
using Microsoft.Win32;

namespace Mohammad.Win.Helpers
{
    public static class AppReg
    {
        public static RegistryKey RegisterMe()
        {
            var result = GetProductsKey().CreateSubKey(WindowsApplicationHelper.ProductName).CreateSubKey(WindowsApplicationHelper.Version);
            result.SetValue("Name", WindowsApplicationHelper.Title ?? WindowsApplicationHelper.ProductName);
            result.SetValue("Description", WindowsApplicationHelper.Description);
            result.SetValue("ExecutablePath", Application.ExecutablePath);
            result.SetValue("ProductVersion", Application.ProductVersion);
            result.Flush();
            return result;
        }

        private static RegistryKey GetProductsKey()
        {
            return Registry.LocalMachine.OpenSubKey("SOFTWARE", true).CreateSubKey(WindowsApplicationHelper.CompanyName).CreateSubKey("Products");
        }
    }
}