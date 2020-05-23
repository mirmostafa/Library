#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Windows.Forms;

namespace Library35.Windows.Settings
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