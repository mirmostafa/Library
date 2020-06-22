using Microsoft.Win32;

namespace Mohammad.Net.Internals
{
    internal class RegUtils
    {
        // byte[]
        internal void GetKeyValue(RegKeyType keyType, string regKey, string name, out byte[] value)
        {
            RegistryKey oRegKey = null;

            switch ((int) keyType)
            {
                case 1:
                    oRegKey = Registry.CurrentUser;
                    break;
                case 2:
                    oRegKey = Registry.LocalMachine;
                    break;
            }
            oRegKey = oRegKey.OpenSubKey(regKey);

            value = (byte[]) oRegKey.GetValue(name);

            oRegKey.Close();
        }

        // int
        internal void GetKeyValue(RegKeyType keyType, string regKey, string name, out int value)
        {
            RegistryKey oRegKey = null;

            switch ((int) keyType)
            {
                case 1:
                    oRegKey = Registry.CurrentUser;
                    break;
                case 2:
                    oRegKey = Registry.LocalMachine;
                    break;
            }
            oRegKey = oRegKey.OpenSubKey(regKey);

            value = (int) oRegKey.GetValue(name, null);

            oRegKey.Close();
        }

        // string
        internal void GetKeyValue(RegKeyType keyType, string regKey, string name, out string value)
        {
            RegistryKey oRegKey = null;

            switch ((int) keyType)
            {
                case 1:
                    oRegKey = Registry.CurrentUser;
                    break;
                case 2:
                    oRegKey = Registry.LocalMachine;
                    break;
            }
            oRegKey = oRegKey.OpenSubKey(regKey);

            value = (string) oRegKey.GetValue(name);

            oRegKey.Close();
        }

        // byte[]
        internal void SetKeyValue(RegKeyType keyType, string regKey, string name, byte[] value)
        {
            RegistryKey oRegKey = null;

            switch ((int) keyType)
            {
                case 1:
                    oRegKey = Registry.CurrentUser;
                    break;
                case 2:
                    oRegKey = Registry.LocalMachine;
                    break;
            }

            oRegKey = oRegKey.OpenSubKey(regKey, true);
            oRegKey.SetValue(name, value);
            oRegKey.Close();
            User32Utils.Notify_SettingChange();
            WinINetUtils.Notify_OptionSettingChanges();
        }

        // string
        internal void SetKeyValue(RegKeyType keyType, string regKey, string name, string value)
        {
            RegistryKey oRegKey = null;

            switch ((int) keyType)
            {
                case 1:
                    oRegKey = Registry.CurrentUser;
                    break;
                case 2:
                    oRegKey = Registry.LocalMachine;
                    break;
            }

            oRegKey = oRegKey.OpenSubKey(regKey, true);
            oRegKey.SetValue(name, value);
            oRegKey.Close();
            User32Utils.Notify_SettingChange();
            WinINetUtils.Notify_OptionSettingChanges();
        }

        // int
        internal void SetKeyValue(RegKeyType keyType, string regKey, string name, int value)
        {
            RegistryKey oRegKey = null;

            switch ((int) keyType)
            {
                case 1:
                    oRegKey = Registry.CurrentUser;
                    break;
                case 2:
                    oRegKey = Registry.LocalMachine;
                    break;
            }

            oRegKey = oRegKey.OpenSubKey(regKey, true);
            oRegKey.SetValue(name, value);
            oRegKey.Close();
            User32Utils.Notify_SettingChange();
            WinINetUtils.Notify_OptionSettingChanges();
        }

        internal enum RegKeyType
        {
            CurrentUser = 1,
            LocalMachine = 2
        }
    }
}