using System.Windows.Forms;
using Mohammad.Helpers;
using Mohammad.Win.Forms.Internals;

namespace Mohammad.Win.Forms
{
    public static class SqlConnectionStringBox
    {
        public static DialogResult Show(ref string connectionString, string prompt = null, string text = null)
        {
            DialogResult result;
            using (var box = new SqlConnectionStringDialogForm())
            {
                box.ConnectionString = connectionString;
                if (!text.IsNullOrEmpty())
                    box.Text = text;
                if (!prompt.IsNullOrEmpty())
                    box.promptLabel.Text = prompt;
                result = box.ShowDialog();
                connectionString = box.ConnectionString;
            }
            return result;
        }
    }
}