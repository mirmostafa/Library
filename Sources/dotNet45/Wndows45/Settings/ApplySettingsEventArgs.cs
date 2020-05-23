using System;
using System.Windows.Forms;

namespace Mohammad.Win.Settings
{
    public class ApplySettingsEventArgs<TForm> : EventArgs
        where TForm : Form
    {
        public readonly TForm Form;
        public ApplySettingsEventArgs(TForm form) { this.Form = form; }
    }
}