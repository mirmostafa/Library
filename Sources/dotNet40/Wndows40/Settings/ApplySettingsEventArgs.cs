#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Windows.Forms;

namespace Library40.Win.Settings
{
	public class ApplySettingsEventArgs<TForm> : EventArgs
		where TForm : Form
	{
		public readonly TForm Form;

		public ApplySettingsEventArgs(TForm form)
		{
			this.Form = form;
		}
	}
}