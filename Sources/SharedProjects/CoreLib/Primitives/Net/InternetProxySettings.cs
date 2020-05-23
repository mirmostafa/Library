using System.Text;
using Mohammad.Net.Internals;

namespace Mohammad.Net
{
    public static class InternetProxySettings
    {
        private static readonly RegUtils _RegUtils = new RegUtils();

        public static void SetProxyName(string proxyAddress)
        {
            const string szRegKey = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings\";
            const string szName = "ProxyServer";

            _RegUtils.SetKeyValue(RegUtils.RegKeyType.CurrentUser, szRegKey, szName, proxyAddress);
        }

        public static void EnableProxy(string proxyAddress)
        {
            var szRegKey = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings\";
            const string szName = "ProxyEnable";
            string szValue;

            _RegUtils.GetKeyValue(RegUtils.RegKeyType.CurrentUser, szRegKey, "ProxyServer", out szValue);

            if (szValue != proxyAddress)
                SetProxyName(proxyAddress);

            _RegUtils.SetKeyValue(RegUtils.RegKeyType.CurrentUser, szRegKey, szName, 1);

            byte[] abValue;
            szRegKey = szRegKey + "Connections";
            _RegUtils.GetKeyValue(RegUtils.RegKeyType.CurrentUser, szRegKey, "DefaultConnectionSettings", out abValue);
            var aaValue = new char[abValue.Length];

            for (var i = 0; i < abValue.Length; i++)
            {
                if (i == 8)
                    abValue[i] = 3;
                aaValue[i] = (char) abValue[i];
            }

            _RegUtils.SetKeyValue(RegUtils.RegKeyType.CurrentUser,
                szRegKey,
                "DefaultConnectionSettings",
                Encoding.ASCII.GetBytes(new string(aaValue).Replace(szValue, proxyAddress)));
        }

        public static void DisableProxy(string proxyAddress)
        {
            var szRegKey = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings\";
            const string szName = "ProxyEnable";
            _RegUtils.SetKeyValue(RegUtils.RegKeyType.CurrentUser, szRegKey, szName, 0);

            byte[] abValue;
            szRegKey = szRegKey + "Connections";
            _RegUtils.GetKeyValue(RegUtils.RegKeyType.CurrentUser, szRegKey, "DefaultConnectionSettings", out abValue);

            for (var i = 0; i < abValue.Length; i++)
                if (i == 8)
                {
                    abValue[i] = 0;
                    break;
                }

            _RegUtils.SetKeyValue(RegUtils.RegKeyType.CurrentUser, szRegKey, "DefaultConnectionSettings", abValue);
        }

        public static string GetProxyName()
        {
            const string szRegKey = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings\";
            const string szName = "ProxyServer";
            string szProxyAddress;

            _RegUtils.GetKeyValue(RegUtils.RegKeyType.CurrentUser, szRegKey, szName, out szProxyAddress);

            return szProxyAddress;
        }

        public static int GetProxyStatus()
        {
            const string szRegKey = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings\";
            const string szName = "ProxyEnable";
            int iProxyStatus;

            _RegUtils.GetKeyValue(RegUtils.RegKeyType.CurrentUser, szRegKey, szName, out iProxyStatus);

            return iProxyStatus;
        }
    }
}