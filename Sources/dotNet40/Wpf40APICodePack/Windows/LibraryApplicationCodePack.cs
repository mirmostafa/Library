#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Windows;
using Library40.Helpers;
using Library40.Threading;
using Library40.Wpf.Windows.Dialogs;
using Microsoft.WindowsAPICodePack.ApplicationServices;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Library40.Wpf.Windows
{
	public class LibraryApplicationCodePack : LibraryApplication
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			this.RegisterForRestart();
			this.RegisterForRecovery();
			if (e.Args.Contains("/crashed"))
				this.OnCreashed();
			Async.Run(() =>
			          {
				          Async.Sleep(1000);
				          var jumpList = JumpList.CreateJumpList();
				          this.UpdateJumpList(jumpList);
			          });
		}

		protected virtual void OnCreashed()
		{
			MsgBoxEx.Error("Application crashed last time");
		}

		protected virtual void UpdateJumpList(JumpList jumpList)
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
			if (jumpList.MaxSlotsInList > 0)
				jumpList.Refresh();
		}

		protected virtual void RegisterForRestart()
		{
			ApplicationRestartRecoveryManager.RegisterForApplicationRestart(new RestartSettings("/crashed", RestartRestrictions.None));
		}

		protected virtual void RegisterForRecovery()
		{
			ApplicationRestartRecoveryManager.RegisterForApplicationRecovery(new RecoverySettings(new RecoveryData(RecoveryProcedure, null), 0));
		}

		private static int RecoveryProcedure(object state)
		{
			var isCanceled = ApplicationRestartRecoveryManager.ApplicationRecoveryInProgress();

			if (isCanceled)
				Environment.Exit(2);
			ApplicationRestartRecoveryManager.ApplicationRecoveryFinished(true);
			return 0;
		}
	}
}