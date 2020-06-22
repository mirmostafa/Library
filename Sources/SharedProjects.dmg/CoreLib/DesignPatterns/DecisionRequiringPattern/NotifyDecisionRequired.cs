using System;

namespace Mohammad.DesignPatterns.DecisionRequiringPattern
{
    [Flags]
    public enum ResponceType
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
        public ResponceType Responce { get; set; }
        public DecisionLevel Level { get; private set; }
        public ResponceType AllowedResponceTypes { get; set; }

        public DecisionRequiredEventArgs(Exception exception, DecisionLevel level,
            ResponceType allowedResponceTypes = ResponceType.Cancel | ResponceType.Continue | ResponceType.Retry)
        {
            this.Level = level;
            this.AllowedResponceTypes = allowedResponceTypes;
            this.Exception = exception;
        }
    }

    public class NotifyDecisionRequired : INotifyDecisionRequired
    {
        protected virtual void OnDecisionRequired(DecisionRequiredEventArgs e, bool throwIfNotHandled)
        {
            var handler = this.DecisionRequired;
            if (handler != null)
                handler(this, e);
            else if (throwIfNotHandled)
                throw e.Exception;
        }

        public ResponceType Ask(Exception exception, DecisionLevel level = DecisionLevel.Warning, bool throwIfNotHandled = true,
            ResponceType allowedResponceTypes = ResponceType.Cancel | ResponceType.Continue | ResponceType.Retry)
        {
            var e = new DecisionRequiredEventArgs(exception, level, allowedResponceTypes);
            this.OnDecisionRequired(e, throwIfNotHandled);
            return e.Responce;
        }

        public event EventHandler<DecisionRequiredEventArgs> DecisionRequired;
    }
}