#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Windows.Forms;

namespace Library40.Win.Controls
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