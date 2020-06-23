using System.Windows.Forms;

namespace Mohammad.Win.Controls
{
    public partial class PersianDateTimePicker : TextBox, IPermissionalControl
    {
        public string PermissionKey { get; set; }

        public PersianDateTimePicker()
        {
            this.InitializeComponent();
        }
    }
}