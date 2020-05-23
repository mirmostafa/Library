#region File Notice
// Created at: 2013/12/24 3:46 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Linq;
using System.Windows.Forms;
using Library40.Win.Dialogs;

namespace TestWinApp40
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			this.InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			//MultiStepOperation.GetMultiStepOperation(new Dictionary<Action, string>
			//{
			//    {
			//        () => Thread.Sleep(2000), "Initalizations"
			//    },
			//    {
			//        () => Thread.Sleep(2000), "Updater initiated. Starting..."
			//    },
			//    {
			//        () => Thread.Sleep(2000), "Extracting..."
			//    },
			//    {
			//        () => Thread.Sleep(2000), "Stopping IIS..."
			//    },
			//    {
			//        () => Thread.Sleep(2000), "Runing scripts..."
			//    },
			//    {
			//        () => Thread.Sleep(2000), "Copying new files..."
			//    },
			//    {
			//        () => Thread.Sleep(2000), "Starting IIS..."
			//    },
			//    {
			//        () => Thread.Sleep(2000), "Deleting temporary files..."
			//    }
			//}).ShowProgress(instructionText: "Insaltting patches", donePrompt: "Patch installed", cancelable: false, cancelButtonText: "OK");
			//Thread.Sleep(500);
			MsgBoxEx.Inform("Hello", detailsExpandedText: "Test");
		}
	}
}