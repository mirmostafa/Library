#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Library35.Win32.NetworkList;

namespace Library35.Win32.Interop.NetworkList
{
	[ComImport]
	[Guid("DCB00000-570F-4A9B-8D69-199FDBA5723B")]
	[TypeLibType((short)0x1040)]
	internal interface INetworkListManager
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		IEnumerable GetNetworks([In] NetworkConnectivityLevels Flags);

		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		INetwork GetNetwork([In] Guid gdNetworkId);

		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		IEnumerable GetNetworkConnections();

		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		INetworkConnection GetNetworkConnection([In] Guid gdNetworkConnectionId);

		bool IsConnectedToInternet { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

		bool IsConnected { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		Connectivity GetConnectivity();
	}
}