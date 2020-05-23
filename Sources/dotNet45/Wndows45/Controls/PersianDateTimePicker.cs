using System.Windows.Forms;

namespace Mohammad.Win.Controls
{
    public partial class PersianDateTimePicker : TextBox, IPermissionalControl
    {
        #region IPermissionalControl Members

        public string PermissionKey { get; set; }

        #endregion

        public PersianDateTimePicker()
        {
            this.InitializeComponent();
        }
    }
}