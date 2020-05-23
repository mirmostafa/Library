#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using Library40.Win.Forms;

namespace Library40.Win.EventsArgs
{
	public sealed class DecidingEventArgs : EventArgs
	{
		public DecidingEventArgs(string question,
			string footerText = null,
			string moreInfo = null,
			MsgBoxExDialogIcon mainIcon = MsgBoxExDialogIcon.Warning,
			MsgBoxExDialogIcon footerIcon = MsgBoxExDialogIcon.Information,
			Action okAction = null,
			Action cancelAction = null,
			Action continueAction = null,
			Action retryAction = null,
			IEnumerable<KeyValuePair<string, Action>> moreDecisions = null,
			bool showSameAnswerForAll = false)
		{
			this.FooterIcon = footerIcon;
			this.OkAction = okAction;
			this.CancelAction = cancelAction;
			this.ContinueAction = continueAction;
			this.MoreDecisions = moreDecisions;
			this.ShowSameAnswerForAll = showSameAnswerForAll;
			this.Question = question;
			this.FooterText = footerText;
			this.MoreInfo = moreInfo;
			this.MainIcon = mainIcon;
			this.RetryAction = retryAction;
		}

		public string Question { get; private set; }
		public string FooterText { get; private set; }
		public string MoreInfo { get; private set; }
		public MsgBoxExDialogIcon MainIcon { get; private set; }
		public MsgBoxExDialogIcon FooterIcon { get; private set; }
		public Action OkAction { get; private set; }
		public Action CancelAction { get; private set; }
		public Action ContinueAction { get; set; }
		public Action RetryAction { get; private set; }
		public IEnumerable<KeyValuePair<string, Action>> MoreDecisions { get; private set; }
		public bool ShowSameAnswerForAll { get; private set; }
		public bool SameAnswerForAllChecked { get; set; }
	}
}