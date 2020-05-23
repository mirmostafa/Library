#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using Library35.Win32.Natives;

namespace Library35.Win32
{
	[Flags]
	public enum WindowAnimateFlags : long
	{
		HorizentalPositive = 0x00000001,
		HorizentalNegative = 0x00000002,
		VerticalPositive = 0x00000004,
		VerticalNegative = 0x00000008,
		Center = 0x00000010,
		Hide = 0x00010000,
		Activate = 0x00020000,
		Slide = 0x00040000,
		Blend = 0x00080000,
	}

	public enum LogonType
	{
		Interactive = Constants.LOGON32_LOGON_INTERACTIVE,
		Network = Constants.LOGON32_LOGON_NETWORK,
		Batch = Constants.LOGON32_LOGON_BATCH,
		Service = Constants.LOGON32_LOGON_SERVICE,
		Unlock = Constants.LOGON32_LOGON_UNLOCK,
		NetworkClearText = Constants.LOGON32_LOGON_NETWORK_CLEARTEXT,
		NewCredentials = Constants.LOGON32_LOGON_NEW_CREDENTIALS,
	}

	public enum LogonProvider
	{
		Default = Constants.LOGON32_PROVIDER_DEFAULT,
		WinNt35 = Constants.LOGON32_PROVIDER_WINNT35,
		WinNt40 = Constants.LOGON32_PROVIDER_WINNT40,
		WinNt50 = Constants.LOGON32_PROVIDER_WINNT50,
	}
}