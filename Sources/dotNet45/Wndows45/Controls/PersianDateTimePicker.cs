using System.Windows.Forms;

namespace Mohammad.Win.Controls
{
    public partial class PersianDateTimePicker : TextBox, IPermissionalControl
    {
        public PersianDateTimePicker()
        {
            this.InitializeComponent();
        }

        public string PermissionKey { get; set; }
    }
}