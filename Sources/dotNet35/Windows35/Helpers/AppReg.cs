#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Windows.Forms;
using Microsoft.Win32;

namespace Library35.Windows.Helpers
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

		//public static IEnumerable<dynamic> GetRegisteredApplications()
		//{
		//    foreach (RegistryKey versionKey in
		//        from product in GetProductsKey().GetSubKeyNames().Select(product => GetProductsKey().OpenSubKey(product)).Where(product => product != null)
		//        from version in product.GetSubKeyNames()
		//        select product.OpenSubKey(version))
		//        yield return new
		//        {
		//            Name = versionKey.GetValue("Name"),
		//            Description = versionKey.GetValue("Description"),
		//            ExecutablePath = versionKey.GetValue("ExecutablePath"),
		//            ProductVersion = versionKey.GetValue("ProductVersion"),
		//        };
		//}
	}
}