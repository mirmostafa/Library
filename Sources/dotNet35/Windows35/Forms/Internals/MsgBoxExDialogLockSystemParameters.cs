#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Drawing;

namespace Library35.Windows.Forms.Internals
{
	internal class MsgBoxExDialogLockSystemParameters
	{
		#region Fields

		#region NewDesktop
		public IntPtr NewDesktop;
		#endregion

		#region Background
		public Bitmap Background;
		#endregion

		#endregion

		#region Methods

		#region MsgBoxExDialogLockSystemParameters
		public MsgBoxExDialogLockSystemParameters(IntPtr newDesktop, Bitmap background)
		{
			this.NewDesktop = newDesktop;
			this.Background = background;
		}
		#endregion

		#endregion
	}
}