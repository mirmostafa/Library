#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Windows.Forms;
using Library40.Helpers;
using Library40.Win.Forms.Internals;

namespace Library40.Win.Forms
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