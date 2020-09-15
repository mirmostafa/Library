using System;
using System.Collections.Generic;
using Mohammad.Win.Forms;

namespace Mohammad.Win.EventsArgs
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

        public string Question { get; }
        public string FooterText { get; }
        public string MoreInfo { get; }
        public MsgBoxExDialogIcon MainIcon { get; }
        public MsgBoxExDialogIcon FooterIcon { get; }
        public Action OkAction { get; }
        public Action CancelAction { get; }
        public Action ContinueAction { get; set; }
        public Action RetryAction { get; }
        public IEnumerable<KeyValuePair<string, Action>> MoreDecisions { get; }
        public bool ShowSameAnswerForAll { get; }
        public bool SameAnswerForAllChecked { get; set; }
    }
}