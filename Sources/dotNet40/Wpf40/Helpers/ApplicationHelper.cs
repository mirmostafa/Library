#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Windows;
using System.Windows.Threading;

namespace Library40.Wpf.Helpers
{
	public static class ApplicationHelper
	{
		public static void DoEvents(this Application application)
		{
			// Create new nested message pump.
			var nestedFrame = new DispatcherFrame();

			// Dispatch a callback to the current message queue, when getting called,
			// this callback will end the nested message loop.
			// note that the priority of this callback should be lower than that of UI event messages.
			var exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
				new DispatcherOperationCallback(state =>
				                                {
					                                {
						                                var frame = state as DispatcherFrame;

						                                // Exit the nested message loop.
						                                frame.Continue = false;
						                                return null;
					                                }
				                                }),
				nestedFrame);

			// pump the nested message loop, the nested message loop will immediately
			// process the messages left inside the message queue.
			Dispatcher.PushFrame(nestedFrame);

			// If the "exitFrame" callback is not finished, abort it.
			if (exitOperation.Status != DispatcherOperationStatus.Completed)
				exitOperation.Abort();
		}

		public static void DoEvents()
		{
			// Create new nested message pump.
			var nestedFrame = new DispatcherFrame();

			// Dispatch a callback to the current message queue, when getting called,
			// this callback will end the nested message loop.
			// note that the priority of this callback should be lower than that of UI event messages.
			var exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
				new DispatcherOperationCallback(state =>
				                                {
					                                {
						                                var frame = state as DispatcherFrame;

						                                // Exit the nested message loop.
						                                frame.Continue = false;
						                                return null;
					                                }
				                                }),
				nestedFrame);

			// pump the nested message loop, the nested message loop will immediately
			// process the messages left inside the message queue.
			Dispatcher.PushFrame(nestedFrame);

			// If the "exitFrame" callback is not finished, abort it.
			if (exitOperation.Status != DispatcherOperationStatus.Completed)
				exitOperation.Abort();
		}
	}
}