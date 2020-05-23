#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library40.Win32.Natives
{
	public static class Constants
	{
		public const uint SV_TYPE_AFP = 0x00000040;
		public const uint SV_TYPE_ALL = 0xFFFFFFFF; /* handy for NetServerEnum2 */
		public const uint SV_TYPE_ALTERNATE_XPORT = 0x20000000; /* return list for alternate transport */
		public const uint SV_TYPE_BACKUP_BROWSER = 0x00020000;
		public const uint SV_TYPE_CLUSTER_NT = 0x01000000; /* NT Cluster */
		public const uint SV_TYPE_CLUSTER_VS_NT = 0x04000000; /* NT Cluster Virtual Server Name */
		public const uint SV_TYPE_DCE = 0x10000000; /* IBM DSS (Directory and Security Services) or equivalent */
		public const uint SV_TYPE_DFS = 0x00800000; /* Root of a DFS tree */
		public const uint SV_TYPE_DIALIN_SERVER = 0x00000400;
		public const uint SV_TYPE_DOMAIN_BAKCTRL = 0x00000010;
		public const uint SV_TYPE_DOMAIN_CTRL = 0x00000008;
		public const uint SV_TYPE_DOMAIN_ENUM = 0x80000000;
		public const uint SV_TYPE_DOMAIN_MASTER = 0x00080000;
		public const uint SV_TYPE_DOMAIN_MEMBER = 0x00000100;
		public const uint SV_TYPE_LOCAL_LIST_ONLY = 0x40000000; /* Return local list only */
		public const uint SV_TYPE_MASTER_BROWSER = 0x00040000;
		public const uint SV_TYPE_NOVELL = 0x00000080;
		public const uint SV_TYPE_NT = 0x00001000;
		public const uint SV_TYPE_POTENTIAL_BROWSER = 0x00010000;
		public const uint SV_TYPE_PRuintQ_SERVER = 0x00000200;
		public const uint SV_TYPE_SERVER = 0x00000002;
		public const uint SV_TYPE_SERVER_MFPN = 0x00004000;
		public const uint SV_TYPE_SERVER_NT = 0x00008000;
		public const uint SV_TYPE_SERVER_OSF = 0x00100000;
		public const uint SV_TYPE_SERVER_UNIX = SV_TYPE_XENIX_SERVER;
		public const uint SV_TYPE_SERVER_VMS = 0x00200000;
		public const uint SV_TYPE_SQLSERVER = 0x00000004;
		public const uint SV_TYPE_TERMINALSERVER = 0x02000000; /* Terminal Server(Hydra) */
		public const uint SV_TYPE_TIME_SOURCE = 0x00000020;
		public const uint SV_TYPE_WFW = 0x00002000;
		public const uint SV_TYPE_WINDOWS = 0x00400000; /* Windows95 and above */
		public const uint SV_TYPE_WORKSTATION = 0x00000001;
		public const uint SV_TYPE_XENIX_SERVER = 0x00000800;
		public const int GENERIC_ALL_ACCESS = 0x10000000;
		public const int PrivilegeEnabled = 0x00000002;
		public const int TokenQuery = 0x00000008;
		public const int AdjustPrivileges = 0x00000020;
		public const string ShutdownPrivilege = "SeShutdownPrivilege";

		public const int LOGON32_LOGON_INTERACTIVE = 2;
		public const int LOGON32_LOGON_NETWORK = 3;
		public const int LOGON32_LOGON_BATCH = 4;
		public const int LOGON32_LOGON_SERVICE = 5;
		public const int LOGON32_LOGON_UNLOCK = 7;
		public const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8;
		public const int LOGON32_LOGON_NEW_CREDENTIALS = 9;

		public const int LOGON32_PROVIDER_DEFAULT = 0;
		public const int LOGON32_PROVIDER_WINNT35 = 1;
		public const int LOGON32_PROVIDER_WINNT40 = 2;
		public const int LOGON32_PROVIDER_WINNT50 = 3;

		public const int MOD_ALT = 0x0001;
		public const int MOD_CONTROL = 0x0002;
		public const int MOD_SHIFT = 0x0004;
		public const int MOD_WIN = 0x0008;
		public const int MOD_NOREPEAT = 0x4000;

		public const int GW_HWNDNEXT = 2;
		public const int GW_HWNDPREV = 3;
		public const int GW_CHILD = 5;
		public const int MF_BYPOSITION = 0x400;
	}
}