using System.Windows.Forms;

namespace Mohammad.Win.Controls
{
    public partial class PersianDateTimePicker : TextBox, IPermissionalControl
    {
        public PersianDateTimePicker() { this.InitializeComponent(); }

        #region IPermissionalControl Members

        public string PermissionKey { get; set; }

        #endregion
    }
}