#region File Notice
// Created at: 2013/12/24 3:46 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Linq;
using System.Windows;
using Library40.Wpf.Windows;
using Library40.Wpf.Windows.Dialogs;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace TestWpfApp40
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App : LibraryApplicationCodePack
	{
		protected override void OnCreashed()
		{
			base.OnCreashed();
			MsgBoxEx.Inform(null, "Crashed");
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
		}

		protected override void UpdateJumpList(JumpList jumpList)
		{
			//jumpList.ClearAllUserTasks();
			//jumpList.AddUserTasks(new JumpListLink(Path.Combine(Environment.CurrentDirectory, "MiMail.exe"), "Mail")
			//{
			//    Arguments = "mail",
			//});
			//jumpList.AddUserTasks(new JumpListLink(Path.Combine(Environment.CurrentDirectory, "MiMail.exe"), "Contacts")
			//{
			//    Arguments = "contacts"
			//});
			//jumpList.AddUserTasks(new JumpListLink(Path.Combine(Environment.CurrentDirectory, "MiMail.exe"), "Calendar")
			//{
			//    Arguments = "calendar"
			//});
			//var category = new JumpListCustomCategory("Mail");
			//category.AddJumpListItems(new JumpListLink(Path.Combine(Environment.CurrentDirectory, "MiMail.exe"), "Inbox")
			//{
			//    Arguments = "inbox"
			//});
			//category.AddJumpListItems(new JumpListLink(Path.Combine(Environment.CurrentDirectory, "MiMail.exe"), "New Message")
			//{
			//    Arguments = "newMessage"
			//});
			//jumpList.AddCustomCategories(category);
			base.UpdateJumpList(jumpList);
		}
	}
}