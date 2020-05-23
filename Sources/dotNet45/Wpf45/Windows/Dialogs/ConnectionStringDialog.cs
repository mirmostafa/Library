namespace Mohammad.Wpf.Windows.Dialogs
{
    /// <summary>
    /// </summary>
    public static class ConnectionStringDialog
    {
        public static bool? Show(ref string connectionstring, string prompt = null, bool validateResult = false)
        {
            var box = new Internals.Dialogs.ConnectionStringDialog {ConnectionString = connectionstring, Prompt = prompt, ValidateResult = validateResult};
            var result = box.ShowDialog();
            if (result == true)
                connectionstring = box.ConnectionString;
            return result;
        }
    }
}