#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Windows.Forms;

namespace Library35.Windows.Controls
{
	public partial class PersianDateTimePicker : TextBox, IPermissionalControl
	{
		public PersianDateTimePicker()
		{
			this.InitializeComponent();
		}

		#region IPermissionalControl Members
		public string PermissionKey { get; set; }
		#endregion
	}
}