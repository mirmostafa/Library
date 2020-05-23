#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library40.Wpf.Dialogs
{
	/// <summary>
	/// </summary>
	public static class ConnectionStringDialog
	{
		public static bool? Show(ref string connectionstring, string prompt = null)
		{
			var box = new Internals.Dialogs.ConnectionStringDialog
			          {
				          ConnectionString = connectionstring,
				          Prompt = prompt
			          };
			var result = box.ShowDialog();
			if (result ?? false)
				connectionstring = box.ConnectionString;
			return result;
		}
	}
}