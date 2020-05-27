#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.DesignPatterns.DecisionRequiringPattern
{
    [Flags]
    public enum ResponseType
    {
        Cancel = 2,
        Retry = 4,
        Continue = 8
    }

    public enum DecisionLevel
    {
        Information,
        Warning,
        Error
    }

    public interface INotifyDecisionRequired
    {
        event EventHandler<DecisionRequiredEventArgs> DecisionRequired;
    }

    public class DecisionRequiredEventArgs : EventArgs
    {
        public Exception Exception { get; }
        public ResponseType Response { get; set; }
        public DecisionLevel Level { get; }
        public ResponseType AllowedResponseTypes { get; set; }

        public DecisionRequiredEventArgs(Exception exception,
            DecisionLevel level,
            ResponseType allowedResponseTypes = ResponseType.Cancel | ResponseType.Continue | ResponseType.Retry)
        {
            this.Level = level;
            this.AllowedResponseTypes = allowedResponseTypes;
            this.Exception = exception;
        }
    }

    public class NotifyDecisionRequired : INotifyDecisionRequired
    {
        public event EventHandler<DecisionRequiredEventArgs> DecisionRequired;

        public ResponseType Ask(Exception exception,
            DecisionLevel level = DecisionLevel.Warning,
            bool throwIfNotHandled = true,
            ResponseType allowedResponseTypes = ResponseType.Cancel | ResponseType.Continue | ResponseType.Retry)
        {
            var e = new DecisionRequiredEventArgs(exception, level, allowedResponseTypes);
            this.OnDecisionRequired(e, throwIfNotHandled);
            return e.Response;
        }

        protected virtual void OnDecisionRequired(DecisionRequiredEventArgs e, bool throwIfNotHandled)
        {
            var handler = this.DecisionRequired;
            if (handler != null)
            {
                handler(this, e);
            }
            else if (throwIfNotHandled)
            {
                throw e.Exception;
            }
        }
    }
}